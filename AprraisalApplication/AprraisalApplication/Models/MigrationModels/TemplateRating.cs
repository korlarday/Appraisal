using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class TemplateRating
    {
        public TemplateRating()
        {

        }
        public TemplateRating(DefaultRating item, int appraisalTemplateId)
        {
            AppraisalTemplateId = appraisalTemplateId;
            Score = item.Score;
            Rating = item.Rating;
            Description = item.Description;
        }

        public int Id { get; set; }
        public int AppraisalTemplateId { get; set; }
        [Required]
        public int Score { get; set; }

        [MaxLength(255)]
        [Required]
        public string Rating { get; set; }

        [Required]
        public string Description { get; set; }
    }
}