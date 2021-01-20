using System.ComponentModel.DataAnnotations;

namespace Contracts.DataTransferObjects.Activities {
    public class ActivityDeleteDTO {
 
        public int Id { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}