using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AutoShowWebApplication
{
    public partial class Model
    {
        public Model()
        {
            Cars = new HashSet<Car>();
        }

        public int ModelId { get; set; }
        [Display(Name = "Назва моделі")]
        public string ModelName { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
