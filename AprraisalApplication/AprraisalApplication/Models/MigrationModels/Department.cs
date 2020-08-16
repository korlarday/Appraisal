using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class Department
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public Department(string departmentName)
        {
            Name = departmentName;
            IsDeleted = false;
        }
        public Department()
        {

        }
    }
}