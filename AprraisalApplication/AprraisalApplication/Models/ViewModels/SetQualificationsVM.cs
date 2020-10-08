using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class SetQualificationsVM
    {
        public List<Qualification> Qualifications { get; set; }
        public Qualification CreateQualification { get; set; }
    }
}