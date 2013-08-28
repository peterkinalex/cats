﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Helpers;
using Cats.Models;
using Cats.Models.PSNP;
using Cats.Models.ViewModels;
using Cats.Services.EarlyWarning;
using Cats.Services.PSNP;
using Cats.Services.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Cats.Areas.PSNP.Controllers
{
    public class RegionalPSNPPledgeController : Controller
    {
        private readonly IDonorService _donorService;
        private readonly IFDPService _fdpService;
        private readonly IRegionalPSNPPlanDetailService _regionalPSNPPlanDetailService;
        private readonly IRegionalPSNPPlanService _regionalPSNPPlanService;
        private readonly IRegionalPSNPPledgeService _regionalPSNPPledgeService;
        private readonly IAdminUnitService _adminUnitService;
        private readonly ICommodityService _commodityService;
        private readonly IRationDetailService _rationDetailService;
        private readonly IUnitService _unitService;

        public RegionalPSNPPledgeController(IDonorService donorService, IFDPService fdpService,
            IRegionalPSNPPlanDetailService regionalPSNPPlanDetailService,IRegionalPSNPPlanService regionalPSNPPlanService,
            IRegionalPSNPPledgeService regionalPSNPPledgeService, IAdminUnitService adminUnitService,
            ICommodityService commodityService, IRationDetailService rationDetailService, IUnitService unitService)
        {
            _donorService = donorService;
            _fdpService = fdpService;
            _regionalPSNPPlanDetailService = regionalPSNPPlanDetailService;
            _regionalPSNPPlanService = regionalPSNPPlanService;
            _regionalPSNPPledgeService = regionalPSNPPledgeService;
            _adminUnitService = adminUnitService;
            _commodityService = commodityService;
            _rationDetailService = rationDetailService;
            _unitService = unitService;
        }
        //
        // GET: /PSNP/RegionalPSNPPledge/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Issue()
        {
            var regionalPSNPPledge = new RegionalPSNPPledge();
            var regionalPSNPPlanList = new List<String>();
            var alreadyReadIDs = new List<int>();
            foreach (var regionalPSNPPlanDetail in _regionalPSNPPlanDetailService.GetAllRegionalPSNPPlanDetail())
            {
                if(alreadyReadIDs.Contains(regionalPSNPPlanDetail.RegionalPSNPPlanID))
                    continue;
                var regionalPSNPPlan = _regionalPSNPPlanService.FindById(regionalPSNPPlanDetail.RegionalPSNPPlanID);
                var region = _adminUnitService.FindById(regionalPSNPPlan.RegionID);
                var regionalPSNPPlanName = regionalPSNPPlan.Year + " - " + region.Name;
                regionalPSNPPlanList.Add(regionalPSNPPlanName);
                alreadyReadIDs.Add(regionalPSNPPlanDetail.RegionalPSNPPlanID);
            }
            ViewBag.RegionalPSNPPlanDetail = new SelectList(regionalPSNPPlanList, "", "", regionalPSNPPledge.RegionalPSNPPlanDetailID = 1);
            ViewBag.DonorID = new SelectList(_donorService.GetAllDonor(), "DonorID", "Name");
            ViewBag.CommodityID = new SelectList(_commodityService.GetAllCommodity(), "CommodityID", "Name");
            ViewBag.UnitID = new SelectList(_unitService.GetAllUnit(), "UnitID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Issue(RegionalPSNPPledge regionalPSNPPledge, string pledgeDate)
        {
            regionalPSNPPledge.PledgeDate = GetGregorianDate(pledgeDate);

            if (ModelState.IsValid)
            {
                _regionalPSNPPledgeService.AddRegionalPSNPPledge(regionalPSNPPledge);
            }

            return RedirectToAction("Index");
        }

        public ActionResult FDPsCoveredByDonorsPostBack([DataSourceRequest]DataSourceRequest request)
        {
            var coveredFDPsList = new List<FDPsCoveredByDonors>();
            var regionalPSNPPledges = _regionalPSNPPledgeService.GetAllRegionalPSNPPledge();
            foreach (var regionalPSNPPledge in regionalPSNPPledges)
            {
                var coveredFDPs = new FDPsCoveredByDonors();
                var regionalPSNPPlanDetail =
                    _regionalPSNPPlanDetailService.FindById(regionalPSNPPledge.RegionalPSNPPlanDetail.RegionalPSNPPlanDetailID);
                coveredFDPs.Donor = regionalPSNPPledge.Donor.Name;
                var fdpObj = _fdpService.FindById(regionalPSNPPlanDetail.PlanedFDPID);
                var woredaAdminUnit = _adminUnitService.FindById(fdpObj.AdminUnitID);
                coveredFDPs.FDP = fdpObj.Name;
                coveredFDPs.Woreda = woredaAdminUnit.Name;
                coveredFDPs.Zone = woredaAdminUnit.AdminUnit2.Name;
                coveredFDPs.Region = woredaAdminUnit.AdminUnit2.AdminUnit2.Name;
                coveredFDPs.Commodity = _commodityService.FindById(regionalPSNPPledge.Commodity.CommodityID).Name;
                var regionalPSNPPlan = _regionalPSNPPlanService.FindById(regionalPSNPPlanDetail.RegionalPSNPPlanID);
                var rationDetails = _rationDetailService.Get(t => t.RationID == regionalPSNPPlan.RationID);
                var pledge = regionalPSNPPledge;
                decimal neededQty = 0;
                const string neededQtyUnit = "";
                foreach (var rationDetail in rationDetails.Where(rationDetail => rationDetail.CommodityID == pledge.Commodity.CommodityID))
                {
                    neededQty = rationDetail.Amount;
                }
                coveredFDPs.NeededQty = neededQty.ToString(CultureInfo.InvariantCulture);
                coveredFDPs.NeededQtyUnit = neededQtyUnit;
                coveredFDPs.PledgedQty = regionalPSNPPledge.Quantity.ToString(CultureInfo.InvariantCulture);
                coveredFDPs.PledgedQtyUnit = regionalPSNPPledge.Unit.Name;
                coveredFDPs.PledgeDate = regionalPSNPPledge.PledgeDate.ToString(CultureInfo.InvariantCulture);

                coveredFDPsList.Add(coveredFDPs);
            }

            return Json(coveredFDPsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult FDPsCoveredByDonors()
        {
            return View();
        }

        private DateTime GetGregorianDate(string ethiopianDate)
        {
            DateTime convertedGregorianDate;
            try
            {
                convertedGregorianDate = DateTime.Parse(ethiopianDate);
            }
            catch (Exception ex)
            {
                var strEth = new getGregorianDate();
                convertedGregorianDate = strEth.ReturnGregorianDate(ethiopianDate);
            }
            return convertedGregorianDate;
        }
    }
}