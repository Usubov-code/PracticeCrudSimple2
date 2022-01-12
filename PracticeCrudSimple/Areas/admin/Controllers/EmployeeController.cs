using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeCrudSimple.Data;
using PracticeCrudSimple.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeCrudSimple.Areas.admin.Controllers
{
    [Area("admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {

            return View(_context.Employees.Include(e=>e.Position).ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Position = _context.Positions.ToList();

            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee model)
        {

            ViewBag.Position = _context.Positions.ToList();

            if (ModelState.IsValid)
            {
                if (model.ImageFile.ContentType =="image/jpg" || model.ImageFile.ContentType == "image/png")
                {
                    if (model.ImageFile.Length<= 3145728)
                    {
                        string fileName = Guid.NewGuid() + "-" + model.ImageFile.FileName;
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            model.ImageFile.CopyTo(stream);
                        }

                        model.Image = fileName;
                        _context.Employees.Add(model);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "You can only upload max 3 mb file!");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "You can only upload image file!");
                    return View(model);
                }

               
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult Update(int? id)
        {

            ViewBag.Position = _context.Positions.ToList();
            Employee employee = _context.Employees.Find(id);
            return View(employee);
        }
        [HttpPost]
        public IActionResult Update(Employee model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile!=null)
                {
                    if (model.ImageFile.ContentType == "image/jpeg" || model.ImageFile.ContentType == "image/png")
                    {
                        if (model.ImageFile.Length <= 3145728)
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", model.Image);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }

                            string fileName = Guid.NewGuid() + "-" + model.ImageFile.FileName;
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                model.ImageFile.CopyTo(stream);
                            }

                            model.Image = fileName;
                        }
                        else
                        {
                            ModelState.AddModelError("", "You can only upload max 3 mb file!");
                            return View(model);
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "You can only upload image file!");
                        return View(model);
                    }
                    _context.Employees.Update(model);
                    _context.SaveChanges();
                    return RedirectToAction("index");
                }
                else
                {
                    return View(model);
                }
            }

            return View();

        }

    }
}
