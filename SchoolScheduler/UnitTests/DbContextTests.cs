using System.IO;
using Xunit;
using Persistence;

namespace UnitTests {
    public class DbContextTests {
        private static string pathToJsonTestFile = "./../../../testDb.json";

        [Fact]
        public void LoadData_ReadDbFileToAppDataObject_CorrectAppDataObject() {
            PrepareTestData();
            var context = new DbContext(pathToJsonTestFile);
            Assert.NotNull(context.AppData);
            Assert.Equal(2, context.AppData.Rooms.Count);
            Assert.Equal(1, context.AppData.Groups.Count);
            Assert.Equal(2, context.AppData.Classes.Count);
            Assert.Equal(2, context.AppData.Teachers.Count);
            Assert.Equal(2, context.AppData.Activities.Count);
        }
        [Theory]
        [InlineData("132")]
        [InlineData("142")]
        [InlineData("152")]
        public void SaveChanges_AddNewValues_CorrectAppDataObjectWithNewValues(string room) {
            PrepareTestData();
            var context = new DbContext(pathToJsonTestFile);
            context.AppData.Rooms.Add(room);
            context.SaveChanges();
            Assert.True(context.AppData.Rooms.Contains(room));
        }
        private void PrepareTestData() {
            FileInfo fi = new FileInfo(pathToJsonTestFile);
            using (TextWriter txtWriter = new StreamWriter(fi.Open(FileMode.Truncate))) {
                string data = @"
{
    ""rooms"": [
        ""110"",
        ""111""
    ],
    ""groups"": [
        ""1a""
    ],
    ""classes"": [
        ""mat"",
        ""phys""
    ],
    ""teachers"": [
        ""kowalski"",
        ""nowak""
    ],
    ""activities"": [
        {
            ""room"": ""110"",
            ""group"": ""1a"",
            ""class"": ""mat"",
            ""slot"": 1,
            ""teacher"": ""kowalski""
        },
        {
            ""room"": ""121"",
            ""group"": ""1c"",
            ""class"": ""eng"",
            ""slot"": 2,
            ""teacher"": ""nowak""
        }
    ]
}";
                txtWriter.Write(data);
            }
        }
    }
}
