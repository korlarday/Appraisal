using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class AppraisalTemplateSetupVM
    {
        public List<SectionType> SectionTypes { get; set; }
        public List<ExpectedValue> ExpectedValues { get; set; }
        public List<DefaultRating> DefaultRatings { get; set; }

        public AppraisalTemplate AppraisalTemplate { get; set; }
    }
}