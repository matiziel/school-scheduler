using System;
using System.Collections.Generic;
using Model;


namespace Persistence {
    public class AppData {
        public List<string> Rooms { get; set; }
        public List<string> Groups { get; set; }
        public List<string> Classes { get; set; }
        public List<string> Teachers { get; set; }
        public List<Activity> Activities { get; set; }
    }
}
