using System;

namespace Contracts.ViewModels {
    public class ScheduleViewModel {
        public string[] Slots { get; set; }
        public ScheduleViewModel() {
            Slots = new string[45];
            for (int i = 0; i < Slots.Length; i++) {
                Slots[i] = "-";
            }
        }
    }
}