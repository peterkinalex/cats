﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cats.Models
{
    public partial class RegionalRequest
    {
        public RegionalRequest()
        {
            this.RegionalRequestDetails = new List<RegionalRequestDetail>();
        }

        public int RegionalRequestID { get; set; }
        
        public int RegionID { get; set; }
        public int ProgramId { get; set; }
        public int Month { get; set; }
        public DateTime RequistionDate { get; set; }
        public int Year { get; set; }
        public String ReferenceNumber { get; set; }
        [UIHint("AmharicTextBox")]
        public string Remark { get; set; }
        public int Status { get; set; }
        public int RationID { get; set; }
        public int? DonorID { get; set; }

        public virtual ICollection<RegionalRequestDetail> RegionalRequestDetails { get; set; }
        public virtual ICollection<ReliefRequisition> ReliefRequisitions { get; set; }
        public virtual AdminUnit AdminUnit { get; set; }
        public virtual Program Program { get; set; }
        public virtual Ration Ration { get; set; }
        public virtual  Donor Donor { get; set; }

        public string MonthName { get { return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month); } }
    }
}