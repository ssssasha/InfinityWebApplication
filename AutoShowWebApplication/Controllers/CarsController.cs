using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoShowWebApplication;
using Microsoft.AspNetCore.Http;
using System.IO;
using AutoShowWebApplication.Models;
using AutoShowWebApplication.ViewModels;
using ClosedXML.Excel;

namespace AutoShowWebApplication.Controllers
{
    public class CarsController : Controller
    {
        private readonly AutoShowContext _context;
        
        public CarsController(AutoShowContext context)
        {
            _context = context;
        }

        // GET: Cars

        public async Task<IActionResult> IndexAuto()
        {
            return View(await _context.Cars.Include(j => j.BodyType).Include(j => j.Color).Include(j => j.Drive).Include(j => j.Model).ToListAsync());
        }
        public async Task<IActionResult> Index(int? id, string? name)
        {
            //if (id == null) return RedirectToAction("Drifes", "Index");
            ViewBag.DriveId = id;
            ViewBag.DriveType = name;
            var autoByDrives = _context.Cars.Where(b => b.DriveId == id).Include(b => b.Drive).Include(j => j.Model).Include(j => j.BodyType).Include(j => j.Color);
            return View(await autoByDrives.ToListAsync());
        }

        public async Task<IActionResult> IndexM(int? id, string? name)
        {
            //if (id == null) return RedirectToAction("Drifes", "IndexM");
            ViewBag.BodyTypeId = id;
            ViewBag.BodyTypeNames= name;
            var autoByBodyTypes = _context.Cars.Where(b => b.BodyTypeId == id).Include(b => b.BodyType).Include(j => j.Drive).Include(j => j.Model).Include(j => j.Color);
            return View(await autoByBodyTypes.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.BodyType)
                .Include(c => c.Color)
                .Include(c => c.Drive)
                .Include(c => c.Model)
               // .Include(c => c.Photo)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }
        // GET: Сars/Create
        public IActionResult CreateAuto()
        {
            ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType");
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName");

            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAuto(CarViewModel cvm)
        {
            Car car = new Car
            {
                BodyTypeId = cvm.BodyTypeId,
                ColorId = cvm.ColorId,
                DriveId = cvm.DriveId,
                ModelId = cvm.ModelId,

                CarId = cvm.CarId,
                Price = cvm.Price,
                GraduationYear = cvm.GraduationYear,


            };
            if (ModelState.IsValid)
            {
                if (cvm.Image != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(cvm.ImageFile.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)cvm.Image.Length);
                    }
                    // установка массива байтов
                    car.Image = imageData;
                }
                _context.Add(car);
                _context.SaveChanges();

                return RedirectToAction(nameof(IndexAuto));

            }
            ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType");
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName");
            return View(car);
        }
        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAuto([Bind("CarId,ModelId,Price,GraduationYear,BodyTypeId,ColorId,DriveId, Image")] Car car)
        {


            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexAuto));


            }
            ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames", car.BodyTypeId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType", car.DriveId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", car.ModelId);
            //  ViewData["PhotoId"] = new SelectList(_context.Photos, "PhotoId", "Image", car.PhotoId);

            return View(car);


        }*/
        public IActionResult Create(int driveId)
        {
            ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            //ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType");
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName");
            ViewData["PhotoId"] = new SelectList(_context.Models, "PhotoId", "Image");


            ViewBag.DriveId = driveId;
            ViewBag.DriveType = _context.Drives.Where(c => c.DriveId == driveId).FirstOrDefault().DriveType;

            
            return View();
        }

        public IActionResult CreateM(int bodytypeId)
        {
            //ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType");
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName");
            ViewData["PhotoId"] = new SelectList(_context.Models, "PhotoId", "Image");

            ViewBag.BodyTypeId = bodytypeId;
            ViewBag.BodyTypeNames = _context.BodyTypes.Where(c => c.BodyTypeId == bodytypeId).FirstOrDefault().BodyTypeNames;



            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int driveId, [Bind("CarId,ModelId,Price,GraduationYear,BodyTypeId,ColorId,DriveId,Image")] Car car)
        {
            car.DriveId = driveId;
      
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Cars", new { id = driveId, name = _context.Drives.Where(c => c.DriveId == driveId).FirstOrDefault().DriveType });
                
            }
            ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames", car.BodyTypeId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            //ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType", car.DriveId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", car.ModelId);
            //return View(car);
            return RedirectToAction("Index", "Cars", new { id = driveId, name = _context.Drives.Where(c => c.DriveId == driveId).FirstOrDefault().DriveType });
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateM(int bodytypeId, [Bind("CarId,ModelId,Price,GraduationYear,BodyTypeId,ColorId,DriveId,Image")] Car car)
        {
            
            car.BodyTypeId = bodytypeId;
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("IndexM", "Cars", new { id = bodytypeId, name = _context.BodyTypes.Where(c => c.BodyTypeId == bodytypeId).FirstOrDefault().BodyTypeNames });
            }
            //ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames", car.BodyTypeId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType", car.DriveId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", car.ModelId);
            //return View(car);
            
            return RedirectToAction("IndexM", "Cars", new { id = bodytypeId, name = _context.BodyTypes.Where(c => c.BodyTypeId == bodytypeId).FirstOrDefault().BodyTypeNames });
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames", car.BodyTypeId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType", car.DriveId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", car.ModelId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,ModelId,Price,GraduationYear,BodyTypeId,ColorId,DriveId,Image")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexAuto));
            }
            ViewData["BodyTypeId"] = new SelectList(_context.BodyTypes, "BodyTypeId", "BodyTypeNames", car.BodyTypeId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            ViewData["DriveId"] = new SelectList(_context.Drives, "DriveId", "DriveType", car.DriveId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", car.ModelId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.BodyType)
                .Include(c => c.Color)
                .Include(c => c.Drive)
                .Include(c => c.Model)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAuto));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
        
       
            
        


    }
}
