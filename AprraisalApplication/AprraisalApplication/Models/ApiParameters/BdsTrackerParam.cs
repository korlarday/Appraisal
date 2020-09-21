using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiParameters
{
    public class BdsTrackerParam
    {
        public int AnnualTarget { get; set; }
        public int ExpectedRsa { get; set; }
        public int RsaAchieved { get; set; }
        public int FundedPins { get; set; }
        public int PreFundedPins { get; set; }
        public int UnfundedAccounts { get; set; }
        public decimal CashVolume { get; set; }
        public double RsaAchievedPercentage { get; set; }
        public double FundingAchievedPercentage { get; set; }
    }
}