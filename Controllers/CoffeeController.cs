using Microsoft.AspNetCore.Mvc;
using websiteCoffee.Models.Coffee;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace websiteCoffee.Controllers
{
    public class CoffeeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        // 1. PAID ORDERS STORAGE - Meel si ku meel gaar ah u haysa dalabaadka la bixiyey
        private static List<OrderViewModel> _paidOrders = new()
        {
            new OrderViewModel {
                SelectedItems = new List<CartItem> {
                    new CartItem { CoffeeId = 1, Name = "Milk Coffee", Price = 1.50m, Quantity = 2 },
                    new CartItem { CoffeeId = 2, Name = "Espresso", Price = 2.00m, Quantity = 1 }
                }
            }
        };

        // 15-ka Qaxwada ee ugu caansan (Gebi ahaanba waa laga saaray 'Type' si nadiif ah)
        private static List<CoffeeItem> _coffees = new()
        {
            new CoffeeItem { Id = 1, Name = "Milk Coffee", Price = 1.50m, ImageUrl = "Milk Coffe.jpeg" },
            new CoffeeItem { Id = 2, Name = "Espresso", Price = 2.00m, ImageUrl = "black coffee.jpeg" },
            new CoffeeItem { Id = 3, Name = "Caffè Latte", Price = 3.50m, ImageUrl = "Caffe Latte.jpeg" },
            new CoffeeItem { Id = 4, Name = "Cappuccino", Price = 3.75m, ImageUrl = "cappuccino.jpeg" },
            new CoffeeItem { Id = 5, Name = "Americano", Price = 2.50m, ImageUrl = "americano.jpeg" },
            new CoffeeItem { Id = 6, Name = "Macchiato", Price = 3.25m, ImageUrl = "macchiato.jpeg" },
            new CoffeeItem { Id = 7, Name = "Mocha", Price = 3.00m, ImageUrl = "mocha.jpeg" },
            new CoffeeItem { Id = 8, Name = "Turkish Coffee", Price = 3.00m, ImageUrl = "turkish.jpeg" },
            new CoffeeItem { Id = 9, Name = "Flat White", Price = 3.80m, ImageUrl = "flat-white.jpeg" },
            new CoffeeItem { Id = 10, Name = "Irish Coffee", Price = 4.50m, ImageUrl = "irish.jpeg" },
            new CoffeeItem { Id = 11, Name = "Affogato", Price = 1.25m, ImageUrl = "affogato.jpeg" },
            new CoffeeItem { Id = 12, Name = "Cold Brew", Price = 3.25m, ImageUrl = "cold-brew.jpeg" },
            new CoffeeItem { Id = 13, Name = "Iced Latte", Price = 3.90m, ImageUrl = "iced-latte.jpeg" },
            new CoffeeItem { Id = 14, Name = "Frappuccino", Price = 2.05m, ImageUrl = "frappuccino.jpeg" },
            new CoffeeItem { Id = 15, Name = "Vietnamese Iced Coffee", Price = 3.50m, ImageUrl = "vietnamese.jpeg" }
        };

        // Galka wwwroot/images in lala xiriiro ayuu u sahlayaa IWebHostEnvironment
        public CoffeeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        // 2. INDEX - Liiska Maamulka (Admin Dashboard)
        public IActionResult Index() => View(_coffees);

        // 3. CREATE - Foomka lagu daro alaabta cusub
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(CoffeeItem coffee, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    // XALKA SAWIRKA: Waxaan u goynaynaa magaca si nadiif ah si uusan u dhiidin browser kasta
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(fileStream);
                    }

                    coffee.ImageUrl = fileName;
                }
                else
                {
                    coffee.ImageUrl = "default.jpg";
                }

                coffee.Id = _coffees.Any() ? _coffees.Max(c => c.Id) + 1 : 1;
                _coffees.Add(coffee);
                return RedirectToAction(nameof(Index));
            }
            return View(coffee);
        }

        // 4. EDIT - Wax ka beddelka alaabta jiri (Update)
        public IActionResult Edit(int id)
        {
            var coffee = _coffees.FirstOrDefault(c => c.Id == id);
            if (coffee == null) return NotFound();
            return View(coffee);
        }

        [HttpPost]
        public IActionResult Edit(CoffeeItem coffee, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                var existing = _coffees.FirstOrDefault(c => c.Id == coffee.Id);
                if (existing != null)
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        string fileName = Path.GetFileName(ImageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            ImageFile.CopyTo(fileStream);
                        }

                        existing.ImageUrl = fileName;
                    }

                    existing.Name = coffee.Name;
                    existing.Price = coffee.Price;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(coffee);
        }

        // 5. DELETE - Tirtirista alaabta
        public IActionResult Delete(int id)
        {
            var coffee = _coffees.FirstOrDefault(c => c.Id == id);
            if (coffee != null) _coffees.Remove(coffee);
            return RedirectToAction(nameof(Index));
        }

        // 6. ORDER PAGE - Bogga Dalabka Macmiilka
        public IActionResult OrderPage()
        {
            var model = new OrderViewModel
            {
                AvailableCoffees = _coffees
            };
            return View(model);
        }

        // 7. PRINT RECEIPT - Xisaabinta, kaydinta dalabka & rasiidka
        [HttpPost]
        public IActionResult PrintReceipt(int[] coffeeIds, int[] quantities)
        {
            var model = new OrderViewModel();
            for (int i = 0; i < coffeeIds.Length; i++)
            {
                if (quantities[i] > 0)
                {
                    var coffee = _coffees.FirstOrDefault(c => c.Id == coffeeIds[i]);
                    if (coffee != null)
                    {
                        model.SelectedItems.Add(new CartItem
                        {
                            CoffeeId = coffee.Id,
                            Name = coffee.Name,
                            Price = coffee.Price,
                            Quantity = quantities[i]
                        });
                    }
                }
            }

            if (model.SelectedItems.Any())
            {
                _paidOrders.Add(model);
            }

            return View(model);
        }

        // 8. ORDERS LIST - Bogga maamulka ee lagu arko dhamaan dalabaadka la bixiyey
        public IActionResult OrdersList()
        {
            return View(_paidOrders);
        }
    }
}