using AprraisalApplication.Models.ApiParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class SectionDetailBreakdown
    {
        public SectionDetailBreakdown()
        {

        }

        public SectionDetailBreakdown(ItemBreakDown itemBreakDown, int sectionDetailId)
        {
            AppraisalSectionDetailId = sectionDetailId;
            Title = itemBreakDown.ItemText;
            MaxScore = 0;
            if(itemBreakDown.ValueType == "money")
            {
                ExpectedValueId = 2;
            }
            else
            {
                ExpectedValueId = 1;
            }
        }

        public int Id { get; set; }
        public int AppraisalSectionDetailId { get; set; }
        public string Title { get; set; }
        public int MaxScore { get; set; }
        public ExpectedValue ExpectedValue { get; set; }
        public byte ExpectedValueId { get; set; }
    }
}