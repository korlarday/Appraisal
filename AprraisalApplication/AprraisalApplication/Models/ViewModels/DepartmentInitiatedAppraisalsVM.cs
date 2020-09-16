using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class DepartmentInitiatedAppraisalsVM
    {
        public List<NewAppriasalAndParticipants> NewAppriasalAndParticipants { get; set; }
    }
    public class NewAppriasalAndParticipants
    {
        public NewAppraisal NewAppraisal { get; set; }
        public int NumberOfParticipants { get; set; }
    }
}