using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class SetJobTitlesVM
    {
        public List<JobTitle> JobTitles { get; set; }
        public JobTitle CreateJobTitle { get; set; }
    }
}