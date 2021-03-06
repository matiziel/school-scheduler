﻿using System;

namespace Model {
    public class Activity {
        public int Id { get; set; }
        public Room Room { get; set; }
        public ClassGroup ClassGroup { get; set; }
        public Subject Subject { get; set; }
        public Slot Slot { get; set; }
        public Teacher Teacher { get; set; }
        public byte[] Timestamp { get; set; }

        public Activity() { }
        public Activity(Room room, ClassGroup classGroup, Subject subject, Slot slot, Teacher teacher) {
            ClassGroup = classGroup;
            Subject = subject;
            Teacher = teacher;
            Slot = slot;
            Room = room;
        }
        public string GetTitleForTeachers() {
            return Room.Name + " " + Subject.Name + " " + ClassGroup.Name;
        }
        public string GetTitleForGroups() {
            return Room.Name + " " + Subject.Name;
        }
        public string GetTitleForRooms() {
            return ClassGroup.Name;
        }
    }
}
