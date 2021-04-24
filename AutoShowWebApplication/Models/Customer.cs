using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace AutoShowWebApplication
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        [Display(Name = "ПІБ")]
        public string FullName { get; set; }
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; }
        [Display(Name = "Телефон")]
        public string Telephone { get; set; }
    }
}
