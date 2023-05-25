using Bilet1.Areas.Admin.ViewModels;
using Bilet1.DAL;
using Bilet1.Models;
using Bilet1.Utilities.Constans;
using Bilet1.Utilities.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bilet1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public PostsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnviroment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.Where(p => !p.IsDeleted).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostVM createPost)
        {
            if (!ModelState.IsValid)
            {
                return View(createPost);
            }
            if (!createPost.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", ErrorMessages.FileMustBeImageType);
                return View(createPost);
            }
            if (!createPost.Photo.CheckFileSize(500))
            {
                ModelState.AddModelError("Photo", $"{createPost.Photo.FileName} must be size 200kb");
                return View(createPost);
            }
            string root = Path.Combine(_webHostEnviroment.WebRootPath, "assets", "images");
            string fileName = await createPost.Photo.SaveAsync(root);

   

            Post post = new Post()
            {
                Title = createPost.Title,
                Description = createPost.Description,
                Path = fileName
            };
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            Post post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();
            UpdatePostVM updatePostVM = new UpdatePostVM()
            {
                Description = post.Description,
                Id = id,
                Title = post.Title

            };
            return View(updatePostVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdatePostVM post)
        {
            if (!ModelState.IsValid)
            {
                return View(post);
            }
            if (!post.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", ErrorMessages.FileMustBeImageType);
                return View(post);
            }
            if (!post.Photo.CheckFileSize(500))
            {
                ModelState.AddModelError("Photo", $"{post.Photo.FileName} must be size 200kb");
                return View(post);
            }
            string root = Path.Combine(_webHostEnviroment.WebRootPath, "assets", "images");
            Post post1 = await _context.Posts.FindAsync(post.Id);
            string filepath = Path.Combine(root, post1.Path);
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            string newfilename= await post.Photo.SaveAsync(root);

            post1.Description = post.Description;
            post1.Title = post.Title;
            post1.Path = newfilename;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Delete(int id)
        {
            Post post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();
            string roolpath = Path.Combine(_webHostEnviroment.WebRootPath, "assets", "images", post.Path);
            if (System.IO.File.Exists(roolpath))
            {
                System.IO.File.Delete(roolpath);
            }
            _context.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
