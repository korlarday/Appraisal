using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.MigrationModels
{
    public class State
    {

        public int Id { get; set; }

        [MaxLength(255)]
        public string Code { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(255)]
        public string ZoneCode { get; set; }
        public int TransId { get; set; }
        public bool IsDeleted { get; set; }

        public State()
        {

        }
        public State(string stateName)
        {
            Code = "";
            Description = stateName;
            IsDeleted = false;
            TransId = 0;
        }
    }
}