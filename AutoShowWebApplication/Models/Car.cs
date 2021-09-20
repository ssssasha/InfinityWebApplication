using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AutoShowWebApplication
{
    public partial class Car
    {
        
        public int  CarId { get; set; }
        [Display(Name = "Модель")]
        public int? ModelId { get; set; }
        [Display(Name = "Ціна")]
        [Range(0,500000, ErrorMessage = "Введено некоректну ціну")]
        public decimal Price { get; set; }
        [Display(Name = "Рік випуску")]
        [Range(2000, 2021, ErrorMessage = "Введено некоректний рік")]
        public DateTime GraduationYear { get; set; }
        [Display(Name = "Тип кузова")]
        public int? BodyTypeId { get; set; }
        [Display(Name = "Назва кольору")]
        public int? ColorId { get; set; }
        [Display(Name = "Тип приводу")]
        public int? DriveId { get; set; }
        [Display(Name = "Фото")]
        public byte[] Image { get; set; }

        [Display(Name = "Тип кузова")]
        public virtual BodyType BodyType { get; set; }
        [Display(Name = "Назва кольору")]
        public virtual Color Color { get; set; }
        [Display(Name = "Тип приводу")]
        public virtual Drife Drive { get; set; }
        [Display(Name = "Модель")]
        public virtual Model Model { get; set; }
    }
}
