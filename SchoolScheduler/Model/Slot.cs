using System.Collections.Generic;

namespace Model {
    public class Slot {
        public int Id { get; set; }
        public int Index { get; set; }
        public string Comment { get; set; }
        public byte[] Timestamp { get; set; }

        public ICollection<Activity> Activities { get; set; }
        public Slot(int index, string comment) {
            Index = index;
            Comment = comment;
        }
        

    }
}