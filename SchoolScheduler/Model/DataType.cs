using System.ComponentModel;

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