using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class Branch
    {
        public Branch()
        {

        }
        public Branch(string branchName, int stateId)
        {
            Description = branchName;
            StateId = stateId;
            IsDeleted = false;
        }

        public int Id { get; set; }
        public State State { get; set; }

        [Display(Name = "State")]
        public int StateId { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}