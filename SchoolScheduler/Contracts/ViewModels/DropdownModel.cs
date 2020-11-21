namespace Contracts.ViewModels {
    public class DropdownModel {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public DropdownModel(int id, string name) {
            Id = id;
            DisplayName = name;
        }
    }
}
