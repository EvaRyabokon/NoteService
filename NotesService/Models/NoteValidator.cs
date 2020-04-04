using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesService.Models
{
    public struct ValidationResult
    {
        private ValidationResult(bool isSuccessful, string error)
        {
            Error = error;
            IsSuccessful = isSuccessful;
        }

        public string Error { get; }
        public bool IsSuccessful { get; }

        public static ValidationResult Successful()
        {
            return new ValidationResult(true, null);
        }

        public static ValidationResult Unsuccessful(string error)
        {
            return new ValidationResult(false, error);
        }
    }


    public class NoteValidator
    {
        private const string StringIsEmptyError = "Empty String";

        public ValidationResult Validate(Note note)
        {
            if (string.IsNullOrEmpty(note.Content))
            {
                return ValidationResult.Unsuccessful(StringIsEmptyError);
            }


            return ValidationResult.Successful();
        }
    }
}
