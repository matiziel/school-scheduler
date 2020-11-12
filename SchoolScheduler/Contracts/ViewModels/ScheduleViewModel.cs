using System;

namespace Contracts.ViewModels {
    public class ScheduleViewModel {
        public ActivityViewModel[] Slots { get; set; }
        public ScheduleViewModel() {
            Slots = new ActivityViewModel[45];
            for (int i = 0; i < Slots.Length; i++) {
                Slots[i] = new ActivityViewModel();
                Slots[i].Title = "-";
            }
        }
    }
}