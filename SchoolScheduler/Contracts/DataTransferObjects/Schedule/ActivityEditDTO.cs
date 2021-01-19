using System.ComponentModel.DataAnnotations;

namespace Contracts.DataTransferObjects.Schedule {
    public class ActivityEditDTO : ActivityBaseDTO{
        public int Id { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}