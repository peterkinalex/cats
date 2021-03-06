﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cats.Models;

namespace Cats.Areas.EarlyWarning.Models
{
    public class HRDWithRegionalRequestViewModel
    {
        public int HRDID { get; set; }
        public int RegionalRequestID { get; set; }
        public int FDPID { get; set; }
        public int WoredaID { get; set; }
        public string zone { get; set; }
        public string Woreda  { get; set; }
        public int RequestedBeneficiaryNo { get; set; }
        public int HRDBeneficaryNo { get; set; }
        public int Difference { get; set; }
        public int Color { get; set; }
    }
    
    public class PLANWithRegionalRequestViewModel
    {
        public int PlanID { get; set; }
        public int RegionalRequestID { get; set; }
        public int FDPID { get; set; }
        public int WoredaID { get; set; }
        public string zone { get; set; }
        public string Woreda { get; set; }
        public int RequestedBeneficiaryNo { get; set; }
        public int PlannedBeneficaryNo { get; set; }
        public int Difference { get; set; }
        public int Color { get; set; }

        public virtual ICollection<RegionalRequestDetail> RegionalRequestDetails { get; set; }
    }
}