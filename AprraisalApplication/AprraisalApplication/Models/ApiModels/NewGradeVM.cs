using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ApiModels
{
    public class NewGradeVM
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string GradeName { get; set; }
        public string Feedback { get; set; }
    }
}