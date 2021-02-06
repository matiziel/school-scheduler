using System;
using System.Collections.Generic;
using Contracts.DataTransferObjects.Activities;

namespace Contracts.DataTransferObjects.Schedule {
    public class ScheduleDTO {
        public ActivityReadDTO[] Slots { get; set; }
        public ScheduleDTO() {
            Slots = new ActivityReadDTO[45];
            for (int i = 0; i < Slots.Length; i++) {
                Slots[i] = new ActivityReadDTO {
                    Title = "-"
                };
            }
        }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}