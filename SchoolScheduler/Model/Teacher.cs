using System.Collections.Generic;

namespace Model {
    public class Teacher {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public Teacher(string name, string comment) {
            Name = name;
            Comment = comment;
        }

    }
}