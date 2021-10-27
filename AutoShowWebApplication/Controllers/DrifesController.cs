using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using ClosedXML.Excel;
using AutoShowWebApplication.Models;
using AutoShowWebApplication;

namespace AutoShowWebApplication.Controllers
{
   
    public class DrifesController : Controller
    {
        private readonly AutoShowContext _context;

        public DrifesController(AutoShowContext context)
        {
            _context = context;
        }

        // GET: Drifes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Drives.ToListAsync());
        }

        // GET: Drifes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drife = await _context.Drives
                .FirstOrDefaultAsync(m => m.DriveId == id);
            if (drife == null)
            {
                return NotFound();
            }

            // return View(drife);
            return RedirectToAction("Index", "Cars", new { id = drife.DriveId, name = drife.DriveType });

        }

        // GET: Drifes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drifes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DriveId,DriveType")] Drife drife)
        {
            if (ModelState.IsValid)
            {
                _context.Add(drife);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(drife);
        }

        // GET: Drifes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drife = await _context.Drives.FindAsync(id);
            if (drife == null)
            {
                return NotFound();
            }
            return View(drife);
        }

        // POST: Drifes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DriveId,DriveType")] Drife drife)
        {
            if (id != drife.DriveId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drife);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrifeExists(drife.DriveId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(drife);
        }

        // GET: Drifes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drife = await _context.Drives
                .FirstOrDefaultAsync(m => m.DriveId == id);
            if (drife == null)
            {
                return NotFound();
            }

            return View(drife);
        }

        // POST: Drifes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drife = await _context.Drives.FindAsync(id);
            _context.Drives.Remove(drife);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DrifeExists(int id)
        {
            return _context.Drives.Any(e => e.DriveId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Drife newcat;
                                var c = (from cat in _context.Drives
                                         where cat.DriveType.Contains(worksheet.Name)
                                         select cat).ToList();
                                if (c.Count > 0)
                                {
                                    newcat = c[0];
                                }
                                else
                                {
                                    newcat = new Drife();
                                    newcat.DriveType = worksheet.Name;
                                    
                                  
                                    _context.Drives.Add(newcat);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Car car = new Car();
                                        BodyType bt = new BodyType();
                                        car.BodyType = bt;
                                        Color cl = new Color();
                                        car.Color = cl;
                                        car.BodyType.BodyTypeNames = row.Cell(2).Value.ToString();
                                        car.Color.ColorName = row.Cell(3).Value.ToString();
                                       
                                        car.Drive = newcat;
                                        _context.Cars.Add(car);
                                       
                                        
                                        
                                            if (row.Cell(1).Value.ToString().Length > 0)
                                            {
                                                Model model;

                                                var a = (from aut in _context.Models
                                                         where aut.ModelName.Contains(row.Cell(1).Value.ToString())
                                                         select aut).ToList();
                                                if (a.Count > 0)
                                                {
                                                    model = a[0];
                                                car.ModelId = model.ModelId;

                                                }
                                                else
                                                {
                                                    model = new Model();
                                                    model.ModelName = row.Cell(1).Value.ToString();
                                                    
                                                    _context.Models.Add(model);
                                                }
                                            car.Model = model;
                                            }
                                        
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);

                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var drives = _context.Drives.Include("Cars").ToList();
               
                foreach (var c in drives)
                {
                    var worksheet = workbook.Worksheets.Add(c.DriveType);

                    worksheet.Cell("A1").Value = "Модель";
                    worksheet.Cell("B1").Value = "Тип кузова";
                    worksheet.Cell("C1").Value = "Колір";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var cars = c.Cars;

                    int j = 0;
                    foreach (var b in c.Cars)
                    {
                        worksheet.Cell(j + 2, 1).Value = _context.Models.Where(m => m.ModelId == b.ModelId).FirstOrDefault().ModelName;
                        worksheet.Cell(j + 2, 2).Value = _context.BodyTypes.Where(m => m.BodyTypeId == b.BodyTypeId).FirstOrDefault().BodyTypeNames;
                        worksheet.Cell(j + 2, 3).Value = _context.Colors.Where(m => m.ColorId == b.ColorId).FirstOrDefault().ColorName;
                        j++;
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }
}
