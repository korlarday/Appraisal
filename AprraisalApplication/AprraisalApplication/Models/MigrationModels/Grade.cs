using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class Grade
    {
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
        public Grade()
        {

        }
        public Grade(string gradeName)
        {
            Name = gradeName;
            IsDeleted = false;
        }
    }
}