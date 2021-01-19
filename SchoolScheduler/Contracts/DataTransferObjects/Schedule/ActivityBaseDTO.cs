using System.ComponentModel.DataAnnotations;

namespace Contracts.DataTransferObjects.Schedule {
    public class ActivityBaseDTO {

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
    }
}