using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AutoShowWebApplication.ViewModels
{
    public class CreateUserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [Range(1900, 2021, ErrorMessage = "Введено некоректний рік")]
        public int Year { get; set; }
    }
}
