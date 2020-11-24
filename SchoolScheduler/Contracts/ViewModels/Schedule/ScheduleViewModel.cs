using System;

namespace Contracts.ViewModels.Schedule {
    public class ScheduleViewModel {
        public ActivityIndexViewModel[] Slots { get; set; }
        public ScheduleViewModel() {
            Slots = new ActivityIndexViewModel[45];
            for (int i = 0; i < Slots.Length; i++) {
                Slots[i] = new ActivityIndexViewModel();
                Slots[i].Title = "-";
            }
        }
    }
}