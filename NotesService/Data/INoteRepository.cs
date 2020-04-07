using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesService.Models
{
    public interface INoteRepository
    {
        Task<List<Note>> GetAll();
        Task<Note> GetById(int id);
        Task<List<Note>> GetBySubstring(string searchString);
        Task Create(Note note);
        Task Update(Note note);
        Task DeleteById(int id);
    }
}
