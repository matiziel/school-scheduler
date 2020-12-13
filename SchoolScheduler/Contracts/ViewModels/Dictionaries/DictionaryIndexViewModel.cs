using System.ComponentModel.DataAnnotations;

namespace Contracts.ViewModels.Dictionaries {
    public class DictionaryIndexViewModel {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}