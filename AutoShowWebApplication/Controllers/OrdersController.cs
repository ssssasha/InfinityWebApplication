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
    [Authorize(Roles="admin, user")]
    public class OrdersController : Controller
    {
        private readonly AutoShowContext _context;

        public OrdersController(AutoShowContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int? id, string? name)
        {
            //if (id == null) return RedirectToAction("OrderTypes", "Index");
            ViewBag.OrderTypeId = id;
            ViewBag.OrderName = name;
            var orderByorderType = _context.Orders.Where(b => b.OrderTypeId == id).Include(b => b.OrderType).Include(b=> b.Manager).Include(b=> b.Model).Include(b=> b.Customer);
            return View(await orderByorderType.ToListAsync());
            //return View(await _context.Orders.ToListAsync());

        }

        public async Task<IActionResult> IndexOrder()
        {
            return View(await _context.Orders.Include(b => b.Manager).Include(b => b.Model).Include(b => b.Customer).Include(b => b.OrderType).ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(c=>c.Manager)
                .Include(c=>c.Model)
                .Include(c=>c.Customer)
                .Include(c=>c.OrderType)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create(int orderTypeId)
        {
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "FullName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName");
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName");
            //ViewData["OrderTypeId"] = new SelectList(_context.OrderTypes, "OrderTypeId", "OrderName");
            ViewBag.OrderTypeId = orderTypeId;
            ViewBag.OrderName = _context.OrderTypes.Where(c => c.OrderTypeId == orderTypeId).FirstOrDefault().OrderName;
            return View();
        }

        public IActionResult CreateOrder()
        {
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "FullName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName");
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName");
            ViewData["OrderTypeId"] = new SelectList(_context.OrderTypes, "OrderTypeId", "OrderName");
            
            return View();
        }


        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int orderTypeId, [Bind("OrderId,ManagerId,ModelId,CustomerId,OrderTypeId,OrderDate")] Order order)
        {
            order.OrderTypeId = orderTypeId;
            if (ModelState.IsValid)
            {
                 _context.Add(order);
                await _context.SaveChangesAsync();

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Orders", new { id = orderTypeId, name = _context.OrderTypes.Where(c => c.OrderTypeId == orderTypeId).FirstOrDefault().OrderName });
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "FullName", order.ManagerId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", order.ModelId);
            // ViewData["OrderTypeId"] = new SelectList(_context.OrderTypes, "OrderTypeId", "OrderName", order.OrderTypeId);

            // return View(order);
            return RedirectToAction("Index", "Orders", new { id = orderTypeId, name = _context.OrderTypes.Where(c => c.OrderTypeId == orderTypeId).FirstOrDefault().OrderName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder( [Bind("OrderId,ManagerId,ModelId,CustomerId,OrderTypeId,OrderDate")] Order order)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(IndexOrder));
                
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "FullName", order.ManagerId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", order.ModelId);
            ViewData["OrderTypeId"] = new SelectList(_context.OrderTypes, "OrderTypeId", "OrderName", order.OrderTypeId);

             return View(order);
            
        }


        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "FullName", order.ManagerId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", order.ModelId);
            ViewData["OrderTypeId"] = new SelectList(_context.OrderTypes, "OrderTypeId", "OrderName", order.OrderTypeId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,ManagerId,ModelId,CustomerId,OrderTypeId,OrderDate")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexOrder));
            }

            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "FullName", order.ManagerId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "ModelName", order.ModelId);
            ViewData["OrderTypeId"] = new SelectList(_context.OrderTypes, "OrderTypeId", "OrderName", order.OrderTypeId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(c => c.Manager)
                .Include(c => c.Model)
                .Include(c => c.Customer)
                .Include(c => c.OrderType)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexOrder));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
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
                                Order newcat;
                                var c = (from cat in _context.Orders
                                         where cat.Customer.Contains(worksheet.Name)
                                         select cat).ToList();
                                if (c.Count > 0)
                                {
                                    newcat = c[0];
                                }
                                else
                                {
                                    newcat = new Order();
                                    newcat.OrderDate = worksheet.Name;
                                    newcat.Info = "from EXCEL";
                                    //додати в контекст
                                    _context.Categories.Add(newcat);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Book book = new Book();
                                        book.Name = row.Cell(1).Value.ToString();
                                        book.Info = row.Cell(6).Value.ToString();
                                        book.Category = newcat;
                                        _context.Books.Add(book);
                                        //у разі наявності автора знайти його, у разі відсутності - додати
                                        for (int i = 2; i <= 5; i++)
                                        {
                                            if (row.Cell(i).Value.ToString().Length > 0)
                                            {
                                                Author author;

                                                var a = (from aut in _context.Authors
                                                         where aut.Name.Contains(row.Cell(i).Value.ToString())
                                                         select aut).ToList();
                                                if (a.Count > 0)
                                                {
                                                    author = a[0];
                                                }
                                                else
                                                {
                                                    author = new Author();
                                                    author.Name = row.Cell(i).Value.ToString();
                                                    author.Info = "from EXCEL";
                                                    //додати в контекст
                                                    _context.Add(author);
                                                }
                                                AuthorsBooks ab = new AuthorsBooks();
                                                ab.Book = book;
                                                ab.Author = author;
                                                _context.AuthorsBooks.Add(ab);
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
        */

    }
}
