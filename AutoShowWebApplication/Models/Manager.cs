using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AutoShowWebApplication
{
    public partial class Manager
    {
        public int ManagerId { get; set; }
        [Display(Name = "ПІБ")]
        public string FullName { get; set; }
        [Display(Name = "Телефон")]
        public string Telephone { get; set; }
        
    }
}
