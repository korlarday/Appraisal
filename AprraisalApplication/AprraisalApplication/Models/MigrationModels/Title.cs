﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class Title
    {
        [Key]
        public byte Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}