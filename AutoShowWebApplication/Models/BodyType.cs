using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace AutoShowWebApplication
{
    public partial class BodyType
    {
        public BodyType()
        {
            Cars = new HashSet<Car>();
        }

        public int BodyTypeId { get; set; }
        [Display(Name = "Тип кузова")]
        public string BodyTypeNames { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
