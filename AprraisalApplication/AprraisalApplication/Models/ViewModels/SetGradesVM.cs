using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class SetGradesVM
    {
        public List<Grade> Grades { get; set; }
        public Grade CreateGrade { get; set; }
    }
}