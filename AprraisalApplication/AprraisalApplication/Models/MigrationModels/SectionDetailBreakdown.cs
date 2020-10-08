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
            MaxScore = itemBreakDown.Score;
            IsScore = itemBreakDown.IsScore;
            if(itemBreakDown.ValueType == "no")
            {
                ExpectedValueId = 3;
            }
            else if(itemBreakDown.ValueType == "money")
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
        public bool IsScore { get; set; }
        public bool IsDeleted { get; set; }

        internal void Update(ItemBreakDown itemBreakdown)
        {
            Title = itemBreakdown.ItemText;
            MaxScore = itemBreakdown.Score;
            IsScore = itemBreakdown.IsScore;
            if(itemBreakdown.ValueType == "no")
            {
                ExpectedValueId = 3;
            }
            if (itemBreakdown.ValueType == "money")
            {
                ExpectedValueId = 2;
            }
            else
            {
                ExpectedValueId = 1;
            }
        }
    }
}