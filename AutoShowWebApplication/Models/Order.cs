using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AutoShowWebApplication
{
    public partial class Order
    {
        [Display(Name = "Код замовлення")]
        
        public int OrderId { get; set; }
        [Display(Name = "ФІО менеджера")]
        public int ManagerId { get; set; }
        [Display(Name = "Модель")]
        public int ModelId { get; set; }
        [Display(Name = "ФІО клієнта")]
        public int CustomerId { get; set; }
        [Display(Name = "Тип замовлення")]
        public int OrderTypeId { get; set; }
        [Display(Name = "Дата замовлення")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Модель")]
        public virtual Model Model { get; set; }
        [Display(Name = "ФІО Клієнта")]
        public virtual Customer Customer { get; set; }
        [Display(Name = "ФІО Менеджера")]
        public virtual Manager Manager { get; set; }
        [Display(Name = "Тип замовлення")]
        public virtual OrderType OrderType { get; set; }

    }
}
