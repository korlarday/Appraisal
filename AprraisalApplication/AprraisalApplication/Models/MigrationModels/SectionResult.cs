using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class SectionResult
    {
        public int Id { get; set; }
        public int AppraieeId { get; set; }
        public AppraisalTemplateSection AppraisalTemplateSection { get; set; }
        public int AppraisalTemplateSectionId { get; set; }
        public double Score { get; set; }
        public ICollection<SectionDetailResult> SectionDetailResults { get; set; }
        public SectionResult()
        {
            SectionDetailResults = new Collection<SectionDetailResult>();
        }
    }
}