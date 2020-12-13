using System.Collections.Generic;

namespace Model {
    public class Room {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public byte[] Timestamp { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public Room(string name, string comment) {
            Name = name;
            Comment = comment;
        }
        public void Update(string name, string comment) {
            Name = name;
            Comment = comment;
        }
    }
}