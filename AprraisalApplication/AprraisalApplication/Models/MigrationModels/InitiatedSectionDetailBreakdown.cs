using AprraisalApplication.Models.ApiParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class InitiatedSectionDetailBreakdown
    {
        public int Id { get; set; }
        public int InitiatedSectionDetailId { get; set; }
        public string Title { get; set; }
        public int MaxScore { get; set; }
        public ExpectedValue ExpectedValue { get; set; }
        public byte ExpectedValueId { get; set; }
        public bool IsScore { get; set; }
        public bool IsDeleted { get; set; }
        public InitiatedSectionDetailBreakdown()
        {

        }

        public InitiatedSectionDetailBreakdown(SectionDetailBreakdown item, int initiatedSectionDetailId)
        {
            Title = item.Title;
            MaxScore = item.MaxScore;
            InitiatedSectionDetailId = initiatedSectionDetailId;
            ExpectedValueId = item.ExpectedValueId;
            IsDeleted = item.IsDeleted;
            IsScore = item.IsScore;
        }

        internal void Update(ItemBreakDown itemBreakdown)
        {
            Title = itemBreakdown.ItemText;
            MaxScore = itemBreakdown.Score;
            IsScore = itemBreakdown.IsScore;
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