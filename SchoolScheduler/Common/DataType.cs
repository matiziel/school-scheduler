using System.ComponentModel;


namespace Common {
    public enum DataType {
        [Description("Klasy")]
        ClassGroup,
        [Description("Nauczyciele")]
        Teacher,
        [Description("Przedmioty")]
        Subject,
        [Description("Sale")]
        Room
    }
}
