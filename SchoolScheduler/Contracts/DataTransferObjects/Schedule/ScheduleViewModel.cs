using System;
using System.Collections.Generic;

namespace Contracts.DataTransferObjects.Schedule {
    public class ScheduleViewModel {
        public ActivityIndexViewModel[] Slots { get; set; }
        public ScheduleViewModel() {
            Slots = new ActivityIndexViewModel[45];
            for (int i = 0; i < Slots.Length; i++) {
                Slots[i] = new ActivityIndexViewModel();
                Slots[i].Title = "-";
            }
        }
        public IEnumerable<string> Names { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}