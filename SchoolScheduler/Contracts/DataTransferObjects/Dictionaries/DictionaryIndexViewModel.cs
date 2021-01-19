using System.ComponentModel.DataAnnotations;

namespace Contracts.DataTransferObjects.Dictionaries {
    public class DictionaryIndexViewModel {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}