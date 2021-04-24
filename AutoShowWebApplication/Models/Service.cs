using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AutoShowWebApplication
{
    public partial class Service
    {
        public int ServiceId { get; set; }
        [Display(Name = "Тип сервісу")]
        public string TypeOfService { get; set; }
    }
}
