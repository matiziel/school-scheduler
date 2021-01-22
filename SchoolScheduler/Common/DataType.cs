using System.ComponentModel;


namespace Common {
    public enum DataType {
        [Description("Klasy")]
        classGroups,
        [Description("Nauczyciele")]
        teachers,
        [Description("Przedmioty")]
        subjects,
        [Description("Sale")]
        rooms
    }
}
