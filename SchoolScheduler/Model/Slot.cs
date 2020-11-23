using System.Collections.Generic;

namespace Model {
    public class Slot {
        public int Id { get; set; }
        public int Index { get; set; }
        public string Comment { get; set; }
        public ICollection<Activity> Activities { get; set; }

    }
}