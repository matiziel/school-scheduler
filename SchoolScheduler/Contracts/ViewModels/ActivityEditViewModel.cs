using System.ComponentModel.DataAnnotations;

namespace Contracts.ViewModels {
    public class ActivityEditViewModel {
        public int? Id { get; set; }
        [Required]
        public DropdownModel Room { get; set; }
        [Required]
        public DropdownModel ClassGroup { get; set; }
        [Required]
        public DropdownModel Subject { get; set; }
        [Required]
        public DropdownModel Teacher { get; set; }
        [Required]
        public int Slot { get; set; }
        
        
    }
}