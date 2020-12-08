using System.ComponentModel.DataAnnotations;

namespace Contracts.ViewModels.Schedule {
    public class ActivityDeleteViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}