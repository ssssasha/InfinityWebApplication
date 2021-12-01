using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace AutoShowWebApplication
{
    public partial class Color
    {
        public Color()
        {
           Cars = new HashSet<Car>();
        }

        public int ColorId { get; set; }
        [Display(Name = "Назва кольору")]
        public string ColorName { get; set; }
        
        public virtual ICollection<Car> Cars { get; set; }
    }
}
