using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AutoShowWebApplication.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [Range(1900, 2021, ErrorMessage = "Введено некоректний рік")]
        public int Year { get; set; }
    }
}
