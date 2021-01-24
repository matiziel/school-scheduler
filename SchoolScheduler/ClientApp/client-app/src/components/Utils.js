const Utils = {
    terms: {
        days: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'],
        hours: ['8:15 - 9:00', '9:15 - 10:00', '10:15 - 11:00', '11:15 - 12:00', '12:15 - 13:00', '13:15 - 14:00', '14:15 - 15:00', '15:15 - 16:00', '16:15 - 17:00']
    },
    range: (min, max) => Array.from({ length: max - min + 1 }, (_, i) => min + i),
    getTermBySlot: function (slot) {
        return this.terms.days[slot % 5] + ' ' + this.terms.hours[Math.floor(slot / 5)]
    }
};



export default Utils;