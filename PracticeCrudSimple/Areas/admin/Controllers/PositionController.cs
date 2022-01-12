using Microsoft.AspNetCore.Mvc;
using PracticeCrudSimple.Data;
using PracticeCrudSimple.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeCrudSimple.Areas.admin.Controllers
{
    [Area("admin")]
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View(_context.Positions.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Position = _context.Positions.ToList();

            return View();
        }
        [HttpPost]
        public IActionResult Create(Position model)
        {
            if (ModelState.IsValid)
            {
                _context.Positions.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(model);
        }
    }
}
