using Microsoft.EntityFrameworkCore;

namespace NotesService.Models
{
    public class NoteContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }

        public NoteContext(DbContextOptions<NoteContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
