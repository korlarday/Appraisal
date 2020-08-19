using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class DefaultRating
    {
        public int Id { get; set; }
        
        [Required]
        public int Score { get; set; }

        [MaxLength(255)]
        [Required]
        public string Rating { get; set; }

        [Required]
        public string Description { get; set; }
    }
}