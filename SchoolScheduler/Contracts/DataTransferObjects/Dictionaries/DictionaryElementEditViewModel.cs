using System.ComponentModel.DataAnnotations;

namespace Contracts.DataTransferObjects.Dictionaries {
    public class DictionaryElementEditViewModel {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Comment { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}