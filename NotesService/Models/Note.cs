using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesService.Models
{
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Не указан текст")]
        [MaxLength(250)] 
        public string Content { get; set; }
    }
}
