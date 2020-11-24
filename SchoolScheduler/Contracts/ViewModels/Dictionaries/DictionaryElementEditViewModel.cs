using System.ComponentModel.DataAnnotations;

namespace Contracts.ViewModels.Dictionaries {
    public class DictionaryElementEditViewModel {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Comment { get; set; }
    }
}