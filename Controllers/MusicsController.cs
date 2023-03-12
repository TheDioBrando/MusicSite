using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RolesSiteMVC.Data;
using RolesSiteMVC.Models;

namespace RolesSiteMVC.Controllers
{
    [Authorize]
    public class MusicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MusicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Musics
        public async Task<IActionResult> Index()
        {
            
            var applicationDbContext = _context.Musics.Include(m => m.Author);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Musics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            HttpContext.Response.Cookies.Append("lastMusic", id.ToString());
            if (id == null || _context.Musics == null)
            {
                return NotFound();
            }

            var music = await _context.Musics
                .Include(m => m.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (music == null)
            {
                return NotFound();
            }

            return View(music);
        }

        [Authorize(Roles = "Admin")]
        // GET: Musics/Create
        public IActionResult Create()
        {
            ViewData["AuthorID"] = new SelectList(_context.Authors, "Id", "Id");
            return View();
        }

        // POST: Musics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,SongsText,Source,AuthorID")] Music music)
        {
            music.Author = _context.Authors.Find(music.AuthorID);
            ModelState["Author"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            if (ModelState.IsValid)
            {
                _context.Add(music);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Authors"] = _context.Authors.ToList();
            ViewData["AuthorID"] = new SelectList(_context.Authors, "Id", "Id", music.AuthorID);
            return View(music);
        }


        // GET: Musics/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Musics == null)
            {
                return NotFound();
            }

            var music = await _context.Musics.FindAsync(id);
            if (music == null)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.Authors, "Id", "Id", music.AuthorID);
            return View(music);
        }


        // POST: Musics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,SongsText,Source,AuthorID")] Music music)
        {
            if (id != music.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(music);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicExists(music.Id))
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
            ViewData["AuthorID"] = new SelectList(_context.Authors, "Id", "Id", music.AuthorID);
            return View(music);
        }

        // GET: Musics/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Musics == null)
            {
                return NotFound();
            }

            var music = await _context.Musics
                .Include(m => m.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (music == null)
            {
                return NotFound();
            }

            return View(music);
        }

        // POST: Musics/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Musics == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Musics'  is null.");
            }
            var music = await _context.Musics.FindAsync(id);
            if (music != null)
            {
                _context.Musics.Remove(music);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MusicExists(int id)
        {
          return (_context.Musics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
