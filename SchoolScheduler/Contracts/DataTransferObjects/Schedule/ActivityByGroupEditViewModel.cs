using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contracts.DataTransferObjects.Schedule {
    public class ActivityByGroupEditViewModel : ActivityViewModel {
        public IEnumerable<string> ListOfClasses { get; set; }
        public IEnumerable<string> ListOfRooms { get; set; }
        public IEnumerable<string> ListOfTeachers { get; set; }
    }
}