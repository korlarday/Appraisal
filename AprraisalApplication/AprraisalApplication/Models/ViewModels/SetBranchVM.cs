using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Models.ViewModels
{
    public class SetBranchVM
    {
        public List<Branch> Branches { get; set; }
        public Branch CreateBranch { get; set; }

        // For loading the school types
        public IEnumerable<SelectListItem> States { get; set; }
    }
}