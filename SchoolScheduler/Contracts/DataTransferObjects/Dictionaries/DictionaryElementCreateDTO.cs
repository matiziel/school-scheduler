using System.ComponentModel.DataAnnotations;

namespace Contracts.DataTransferObjects.Dictionaries {
    public class DictionaryElementCreateDTO {
        [Required]
        public string Name { get; set; }
        public string Comment { get; set; }

    }
}