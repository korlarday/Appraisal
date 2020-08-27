using AprraisalApplication.Models.ApiParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class ItemBreakdownResult
    {
        public int Id { get; set; }
        public int SectionDetailResultId { get; set; }
        public ItemBreakDown ItemBreakDown { get; set; }
        public int ItemBreakDownId { get; set; }
        public double Score { get; set; }
    }
}