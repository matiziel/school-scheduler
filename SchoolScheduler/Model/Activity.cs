using System;

namespace Model {
    public class Activity {
        public int Id { get; set; }
        public Room Room { get; set; }
        public ClassGroup ClassGroup { get; set; }
        public Subject Subject { get; set; }
        public Slot Slot { get; set; }
        public Teacher Teacher { get; set; }
        public byte[] Timestamp { get; set; }
    }
}
