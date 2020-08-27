using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiParameters
{
    public class UserAppraiserParams
    {
        public List<AppraiserUserItem> Items { get; set; }
    }
    public class AppraiserUserItem
    {
        public string ApplicationUserId { get; set; }
        public string AppraiserId { get; set; }
        public bool ToTeamLeader { get; set; }
    }
}