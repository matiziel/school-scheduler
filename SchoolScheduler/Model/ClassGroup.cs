using System.Collections.Generic;

namespace Model {
    public class ClassGroup {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public ClassGroup(string name, string comment) {
            Name = name;
            Comment = comment;
        }
    }
}