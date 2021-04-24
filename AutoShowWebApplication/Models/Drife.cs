using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace AutoShowWebApplication
{
    public partial class Drife
    {
        public Drife()
        {
            Cars = new HashSet<Car>();
        }

        public int DriveId { get; set; }
        [Display(Name = "Тип приводу")]
        public string DriveType { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
