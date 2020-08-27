using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraisalLocation
    {
        public int Id { get; set; }
        public int NewAppraisalId { get; set; }
        public State State { get; set; }
        public int StateId { get; set; }
    }
}