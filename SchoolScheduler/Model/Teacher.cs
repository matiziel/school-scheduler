using System.Collections.Generic;

namespace Model {
    public class Teacher {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public ICollection<Activity> Activities { get; set; }

    }
}