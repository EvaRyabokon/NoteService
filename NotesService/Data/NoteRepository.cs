using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesService.Models
{
    public class NoteRepository : INoteRepository
    {
        private NoteContext db;

        public NoteRepository(NoteContext context)
        {
            db = context;
        }

        public Task Create(Note note)
        {
            db.Notes.Add(note);
            return db.SaveChangesAsync();
        }

        public Task DeleteById(int id)
        {
            Note note = new Note { Id = id};
            db.Entry(note).State = EntityState.Deleted;

            return db.SaveChangesAsync();
        }

        public Task<List<Note>> GetAll()
        {
            return db.Notes.ToListAsync();
        }

        public Task<Note> GetById(int id)
        {
            return db.Notes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<Note>> GetBySubstring(string searchString)
        {
            return db.Notes.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper())
            || s.Content.ToUpper().Contains(searchString.ToUpper())).ToListAsync();
        }

        public Task Update(Note note)
        {
            db.Notes.Update(note);
            return db.SaveChangesAsync();
        }
    }
}
