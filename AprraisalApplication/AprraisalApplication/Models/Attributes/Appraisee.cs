using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.Attributes
{
    public class Appraisee
    {
        public int Id { get; set; }
        public int NewAppraisalId { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public AppraiseePersonalData AppraiseePersonalData { get; set; }
        public int AppraiseePersonalDataId { get; set; }
        public AppraiserPersonalData AppraiserPersonalData { get; set; }
        public int AppraiserPersonalDataId { get; set; }
        public AppraisalTemplate AppraisalTemplate { get; set; }
        public int AppraisalTemplateId { get; set; }
        public bool IsNew { get; set; }
        public bool IsCompleted { get; set; }
        public ICollection<SectionResult> SectionResults { get; set; }
        public ICollection<AppraiseeCareerHistoryWithCompany> AppraiseeCareerHistoryWithCompanies { get; set; }
        public Appraisee()
        {
            AppraiseeCareerHistoryWithCompanies = new Collection<AppraiseeCareerHistoryWithCompany>();
            SectionResults = new Collection<SectionResult>();
        }
    }
}