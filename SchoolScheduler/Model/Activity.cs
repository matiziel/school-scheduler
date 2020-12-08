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

        public Activity() { }
        public Activity(Room room, ClassGroup classGroup, Subject subject, Slot slot, Teacher teacher) {
            ClassGroup = classGroup ?? throw new ArgumentException("Class group cannot be empty");
            Subject = subject ?? throw new ArgumentException("Subject cannot be empty");
            Teacher = teacher ?? throw new ArgumentException("Teacher cannot be empty");
            Slot = slot ?? throw new ArgumentException("Slot cannot be empty");
            Room = room ?? throw new ArgumentException("Room cannot be empty");
        }
    }
}
