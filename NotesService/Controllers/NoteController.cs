using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotesService.Models;

namespace NotesService.Controllers
{
    public class NoteController : Controller
    {
        private INoteRepository repository;

        public NoteController(INoteRepository repo)
        {
            repository = repo;
        }

        public async Task<IActionResult> List(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var notes = await repository.GetBySubstring(searchString);
                return View(notes);
            }

            var allNotes = await repository.GetAll();
            return View(allNotes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Note note)
        {
            await repository.Create(note);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var note = await repository.GetById(id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Note note)
        {
            await repository.Update(note);
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await repository.DeleteById(id);
            return RedirectToAction("List");
        }
    }
}
