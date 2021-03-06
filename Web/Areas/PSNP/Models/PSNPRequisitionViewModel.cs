﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Areas.PSNP.Models
{
    public class PSNPRequisitionViewModel
    {
        public string Number { get; set; }
        public string Commodity { get; set; }
        public int Beneficicaries { get; set; }
        public decimal Amount { get; set; }
        public int? Status { get; set; }
        public int RequisitionId { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; }
    }

    public class UtilizationViewModel
    {
        public string Region { get; set; }
        public string Commodity { get; set; }
        public int ActualBeneficicaries { get; set; }
        public decimal Amount { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; }
    
    }
}