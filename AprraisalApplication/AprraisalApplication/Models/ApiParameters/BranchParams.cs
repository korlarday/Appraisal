using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiParameters
{
    public class BranchParams
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string BranchName { get; set; }
    }
}