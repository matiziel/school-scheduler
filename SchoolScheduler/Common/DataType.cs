using System.ComponentModel;


namespace Common {
    public enum DataType {
        [Description("Klasy")]
        Group,
        [Description("Nauczyciele")]
        Teacher,
        [Description("Przedmioty")]
        Class,
        [Description("Sale")]
        Room
    }
}
