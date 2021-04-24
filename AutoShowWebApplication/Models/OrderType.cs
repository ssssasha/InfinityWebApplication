using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AutoShowWebApplication
{
    public partial class OrderType
    {
        public int OrderTypeId { get; set; }
        [Display(Name = "Тип замовлення")]

        public string OrderName { get; set; }
    }
}
