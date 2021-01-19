using System.ComponentModel.DataAnnotations;

namespace Contracts.DataTransferObjects.Dictionaries {
    public class DictionaryReadDTO {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}