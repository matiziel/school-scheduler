using System.ComponentModel.DataAnnotations;

namespace Contracts.DataTransferObjects.Dictionaries {
    public class DictionaryElementDeleteViewModel {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}