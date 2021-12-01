using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore.Query;


namespace AutoShowWebApplication.Models
{
    public class CarsListViewModel
    {
        public CarsListViewModel(AutoShowContext context)
        {
            Cars = context.Cars;
            Colors = context.Colors;
            Drives = context.Drives;
            //Years = context.Year;
           SelectedColor = null;
           SelectedDrive = null;
           
        }
        public IEnumerable<Car> Cars;
        public DbSet<Color> Colors;
        public int? SelectedColor { get; set; }
        public DbSet<Drife> Drives;
        public int? SelectedDrive{ get; set; }
        
    }
}
