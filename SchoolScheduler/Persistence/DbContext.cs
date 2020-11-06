using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Persistence {
    public class DbContext {
        private AppData appData;
        public AppData AppData {
            get {
                if (appData is null)
                    LoadData();
                return appData;
            }
            set {
                appData = value;
            }
        }
        private readonly string pathToJsonFile;
        public DbContext() {
            pathToJsonFile = "./db.json";
        }
        public DbContext(string path)
        {
            pathToJsonFile = path;
        }
        private void LoadData() {
            using (StreamReader r = new StreamReader(pathToJsonFile)) {
                string data = r.ReadToEnd();
                appData = JsonConvert.DeserializeObject<AppData>(data);
            }
        }
        public void SaveChanges() {
            if (appData is null)
                return;
            FileInfo fi = new FileInfo(pathToJsonFile);
            using (TextWriter txtWriter = new StreamWriter(fi.Open(FileMode.Truncate))) {
                string data = JsonConvert.SerializeObject(appData);
                txtWriter.Write(data);
            }
            appData = null;
        }
    }
}