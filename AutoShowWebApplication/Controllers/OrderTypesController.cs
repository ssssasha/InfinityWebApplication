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
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace AutoShowWebApplication.Controllers
{
   
    public class OrderTypesController : Controller
    {
        private readonly AutoShowContext _context;

        public OrderTypesController(AutoShowContext context)
        {
            _context = context;
        }

        // GET: OrderTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderTypes.ToListAsync());
        }

        // GET: OrderTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderType = await _context.OrderTypes
                .FirstOrDefaultAsync(m => m.OrderTypeId == id);
            if (orderType == null)
            {
                return NotFound();
            }

            //return View(orderType);
            return RedirectToAction("Index", "Orders", new { id = orderType.OrderTypeId, name = orderType.OrderName });
        }

        // GET: OrderTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderTypeId,OrderName")] OrderType orderType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderType);
        }

        // GET: OrderTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderType = await _context.OrderTypes.FindAsync(id);
            if (orderType == null)
            {
                return NotFound();
            }
            return View(orderType);
        }

        // POST: OrderTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderTypeId,OrderName")] OrderType orderType)
        {
            if (id != orderType.OrderTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderTypeExists(orderType.OrderTypeId))
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
            return View(orderType);
        }

        // GET: OrderTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderType = await _context.OrderTypes
                .FirstOrDefaultAsync(m => m.OrderTypeId == id);
            if (orderType == null)
            {
                return NotFound();
            }

            return View(orderType);
        }

        // POST: OrderTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderType = await _context.OrderTypes.FindAsync(id);
            _context.OrderTypes.Remove(orderType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderTypeExists(int id)
        {
            return _context.OrderTypes.Any(e => e.OrderTypeId == id);
        }


        /*[HttpPost]
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
                                OrderType newcat;
                                var c = (from cat in _context.OrderTypes
                                         where cat.OrderName.Contains(worksheet.Name)
                                         select cat).ToList();
                                if (c.Count > 0)
                                {
                                    newcat = c[0];
                                }
                                else
                                {
                                    newcat = new OrderType();
                                    newcat.OrderName = worksheet.Name;
                                    //newcat.Info = "from EXCEL";
                                    //додати в контекст
                                    _context.OrderTypes.Add(newcat);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Order order = new Order();
                                        //order.Model.ModelName = row.Cell(1).Value.ToString();
                                        order.Customer.FullName = row.Cell(2).Value.ToString();
                                        order.Manager.FullName = row.Cell(3).Value.ToString();
                                        order.OrderDate = Convert.ToDateTime(row.Cell(4).Value.ToString());
                                        //book.Info = row.Cell(6).Value.ToString();
                                        order.OrderType = newcat;
                                        _context.Orders.Add(order);
                                        //у разі наявності автора знайти його, у разі відсутності - додати
                                        for (int i = 2; i <= 5; i++)
                                        {
                                            if (row.Cell(1).Value.ToString().Length > 0)
                                            {
                                                Model model;

                                                var a = (from aut in _context.Models
                                                         where aut.ModelName.Contains(row.Cell(1).Value.ToString())
                                                         select aut).ToList();
                                                if (a.Count > 0)
                                                {
                                                    model = a[0];
                                                }
                                                else
                                                {
                                                    model = new Model();
                                                    model.ModelName = row.Cell(1).Value.ToString();
                                                    //author.Info = "from EXCEL";
                                                    //додати в контекст
                                                    _context.Add(model);
                                                }
                                                Car car = new Car();
                                                car.Model = model;
                                                
                                                //AuthorsBooks ab = new AuthorsBooks();
                                                //ab.Book = book;
                                                //ab.Author = author;
                                                _context.Cars.Add(car);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        //logging самостійно :)

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
                var orderTypes = _context.OrderTypes.Include("Orders").ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)
                foreach (var c in orderTypes)
                {
                    var worksheet = workbook.Worksheets.Add(c.OrderName);

                    worksheet.Cell("A1").Value = "Модель";
                    worksheet.Cell("B1").Value = "Клієнт";
                    worksheet.Cell("C1").Value = "Мнеджер";
                    worksheet.Cell("D1").Value = "Дата замовлення";
                    //worksheet.Cell("E1").Value = "Автор 4";
                    //worksheet.Cell("F1").Value = "Категорія";
                    //worksheet.Cell("G1").Value = "Інформація";
                    //worksheet.Row(1).Style.Font.Bold = true;
                    var orders = c.OrderName.ToList();

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < orders.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = orders[i].Model;
                        worksheet.Cell(i + 2, 7).Value = books[i].Info;

                        var ab = _context.AuthorsBooks.Where(a => a.BookId == books[i].Id).Include("Author").ToList();
                        //більше 4-ох нікуди писати
                        int j = 0;
                        foreach (var a in ab)
                        {
                            if (j < 5)
                            {
                                worksheet.Cell(i + 2, j + 2).Value = a.Author.Name;
                                j++;
                            }
                        }

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
        }*/



    }
}
