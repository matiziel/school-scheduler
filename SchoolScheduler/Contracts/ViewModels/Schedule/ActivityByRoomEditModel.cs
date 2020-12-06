using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contracts.ViewModels.Schedule {
    public class ActivityByRoomEditViewModel : ActivityViewModel {
        public IEnumerable<string> ListOfClasses { get; set; }
        public IEnumerable<string> ListOfGroups { get; set; }
        public IEnumerable<string> ListOfTeachers { get; set; }
    }
}