﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class TemplateSummaryRating
    {
        public TemplateSummaryRating()
        {

        }
        public TemplateSummaryRating(DefaultSummaryRating item, int appraisalTemplateId)
        {
            AppraisalTemplateId = appraisalTemplateId;
            Score = item.Score;
            Rating = item.Rating;
        }

        public int Id { get; set; }
        public int AppraisalTemplateId { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        [MaxLength(255)]
        public string Rating { get; set; }
    }
}