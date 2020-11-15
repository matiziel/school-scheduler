using System;
using System.Collections.Generic;

namespace WebClient.Utils {
    public enum Days {
        Monday, Tuesday, Wednesday, Thursday, Friday
    }
    public class Hours {
        public static string[] Values { get; } = {
            "8:15 - 9:00",
            "9:15 - 10:00",
            "10:15 - 11:00",
            "11:15 - 12:00",
            "12:15 - 13:00",
            "13:15 - 14:00",
            "14:15 - 15:00",
            "15:15 - 16:00",
            "16:15 - 17:00",
        };

    }
    public class SlotToTermMapper {
        public static ValueTuple<Days, string> GetTerm(int slot) {
            Days day = toDaysMapper[slot % 5];
            string hour = Hours.Values[slot / 5];
            return (day, hour);
        }
        private static Dictionary<int, Days> toDaysMapper = new Dictionary<int, Days>() {
            { 0, Days.Monday },
            { 1, Days.Tuesday },
            { 2, Days.Wednesday },
            { 3, Days.Thursday },
            { 4, Days.Friday }
        };
    }

    

}