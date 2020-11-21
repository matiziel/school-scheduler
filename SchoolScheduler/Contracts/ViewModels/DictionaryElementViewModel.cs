using System.ComponentModel.DataAnnotations;

namespace Contracts.ViewModels {
    public class DictionaryElementViewModel {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Comment { get; set; }
    }
}