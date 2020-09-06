using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class AppraisalLocation
    {
        public AppraisalLocation()
        {

        }
        public AppraisalLocation(int newAppraisalId, int stateId)
        {
            NewAppraisalId = newAppraisalId;
            StateId = stateId;
        }

        public int Id { get; set; }
        public int NewAppraisalId { get; set; }
        public State State { get; set; }
        public int StateId { get; set; }
    }
}