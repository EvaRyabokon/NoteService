using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesService.Models
{
    public class Note /*: IValidatableObject*/
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        [Required(ErrorMessage = "Не указан текст")]
        public string Content { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    List<ValidationResult> errors = new List<ValidationResult>();

        //    if (string.IsNullOrWhiteSpace(this.Content))
        //        errors.Add(new ValidationResult("Не указано имя"));

           

        //    return errors;
        //}
    }
}
