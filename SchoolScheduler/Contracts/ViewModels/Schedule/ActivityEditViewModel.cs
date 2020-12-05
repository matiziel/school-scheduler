using System.ComponentModel.DataAnnotations;

namespace Contracts.ViewModels.Schedule {
    public class ActivityEditViewModel {
        public int? Id { get; set; }
        [Required]
        public int Slot { get; set; }
        [Required]
        public string Room { get; set; }
        [Required]
        public string ClassGroup { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Teacher { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}