using Bilet1.DAL;
using Bilet1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bilet1.Controllers
{
    public class HomeController : Controller
    {
       private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {

            return View( await _context.Posts.Where(p=>!p.IsDeleted).ToListAsync());
        }


    }
}