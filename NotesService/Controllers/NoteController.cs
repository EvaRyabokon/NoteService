using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesService.Models;

namespace NotesService.Controllers
{
    public class NoteController : Controller
    {
        private NoteContext db;
        public NoteController(NoteContext context)
        {
            db = context;
        }

        //[HttpGet, ActionName("List")]
        //public async Task<IActionResult> List()
        //{
        //    return View(await db.Notes.ToListAsync());
        //}

        //[Route("search")]
        public async Task<IActionResult> List(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return View(await db.Notes.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper()) 
                || s.Content.ToUpper().Contains(searchString.ToUpper())).ToListAsync());
            }

            return View(await db.Notes.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Note note)
        {

            db.Notes.Add(note);
            await db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                Note note = await db.Notes.FirstOrDefaultAsync(p => p.Id == id);
                if (note != null)
                    return View(note);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                Note note = await db.Notes.FirstOrDefaultAsync(p => p.Id == id.Value);
                if (note != null)
                    return View(note);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Note note)
        {
            db.Notes.Update(note);
            await db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Note note = await db.Notes.FirstOrDefaultAsync(p => p.Id == id);
                if (note != null)
                    return View(note);
            }
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Note note = new Note { Id = id.Value };
                db.Entry(note).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return NotFound();
        }
    }
}

