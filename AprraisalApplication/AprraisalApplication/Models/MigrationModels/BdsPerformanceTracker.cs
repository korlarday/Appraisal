using AprraisalApplication.Models.ApiParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class BdsPerformanceTracker
    {
        public int Id { get; set; }
        public int AnnualTarget { get; set; }
        public int ExpectedRSA { get; set; }
        public int RSAAchieved { get; set; }
        public int FundedPins { get; set; }
        public int PreFundedPins { get; set; }
        public int UnfundedAccounts { get; set; }
        public decimal CashVolume { get; set; }
        public double RSAAchievedPercentage { get; set; }
        public double FundingAchievedPercentage { get; set; }

        public BdsPerformanceTracker()
        {
            AnnualTarget = 0;
            ExpectedRSA = 0;
            RSAAchieved = 0;
            FundedPins = 0;
            PreFundedPins = 0;
            UnfundedAccounts = 0;
            CashVolume = 0;
            RSAAchievedPercentage = 0;
            FundingAchievedPercentage = 0;
        }


        internal void UpdateTrackerAppraisee(BdsTrackerParam bdsTracker)
        {
            AnnualTarget = bdsTracker.AnnualTarget;
            ExpectedRSA = bdsTracker.ExpectedRsa;
            RSAAchieved = bdsTracker.RsaAchieved;
            FundedPins = bdsTracker.FundedPins;
            PreFundedPins = bdsTracker.PreFundedPins;
            CashVolume = bdsTracker.CashVolume;
            //UnfundedAccounts = bdsTracker.UnfundedAccounts;
        }

        internal void UpdateTrackerAppraiser(BdsTrackerParam bdsTracker)
        {
            AnnualTarget = bdsTracker.AnnualTarget;
            ExpectedRSA = bdsTracker.ExpectedRsa;
            RSAAchieved = bdsTracker.RsaAchieved;
            FundedPins = bdsTracker.FundedPins;
            PreFundedPins = bdsTracker.PreFundedPins;
            UnfundedAccounts = bdsTracker.UnfundedAccounts;
            CashVolume = bdsTracker.CashVolume;
            RSAAchievedPercentage = bdsTracker.RsaAchievedPercentage;
            FundingAchievedPercentage = bdsTracker.FundingAchievedPercentage;
        }
    }
}