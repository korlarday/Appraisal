using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiModels
{
    public class NewBranchVM
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string BranchName { get; set; }
        public string State { get; set; }
        public int StateId { get; set; }
        public string Feedback { get; set; }
    }
}