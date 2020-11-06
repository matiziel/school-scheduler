using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Persistence {
    public class DbContext {
        private Schedule schedule;
        public Schedule Schedule {
            get {
                if (schedule is null)
                    LoadData();
                return schedule;
            }
            set {
                schedule = value;
            }
        }
        private readonly string pathToJsonFile;
        public DbContext() {
            pathToJsonFile = "./../Persistence/db.json";
        }
        public DbContext(string path) {
            pathToJsonFile = path;
        }
        private void LoadData() {
            using (StreamReader r = new StreamReader(pathToJsonFile)) {
                string data = r.ReadToEnd();
                schedule = JsonConvert.DeserializeObject<Schedule>(data);
            }
        }
        public void SaveChanges() {
            if (schedule is null)
                return;
            FileInfo fi = new FileInfo(pathToJsonFile);
            using (TextWriter txtWriter = new StreamWriter(fi.Open(FileMode.Truncate))) {
                string data = JsonConvert.SerializeObject(schedule);
                txtWriter.Write(data);
            }
            schedule = null;
        }
    }
}