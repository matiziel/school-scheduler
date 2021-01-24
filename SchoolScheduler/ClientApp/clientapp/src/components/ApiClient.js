import axios from 'axios';

const ApiClient = {
    apiUrl: (path) => ('https://localhost:5001/api' + path),
    getDictionaries: async function (slot, id = null) {
        let query = slot.toString();
        if (id !== null)
            query += '?id=' + id.toString();
        const g = await this.getDictionaryBySlot('classGroups/' + query)
        const r = await this.getDictionaryBySlot('rooms/' + query);
        const t = await this.getDictionaryBySlot('teachers/' + query);
        const s = await this.getDictionaryBySlot('subjects');
        return {
            teachers: t,
            groups: g,
            rooms: r,
            subjects: s
        };
    },
    getDictionary: async function (type) {
        const result = await axios.get(this.apiUrl('/Dictionaries/all/' + type));
        return result.data;
    },
    getDictionaryBySlot: async function (query) {
        const result = await axios.get(this.apiUrl('/Dictionaries/' + query));
        return result.data;
    },
    getSchedule: async function (type, name) {
        const result = await axios.get(this.apiUrl('/Schedule/' + type + '/' + name));
        return {
            slots: result.data.slots,
            name: result.data.name,
            type: result.data.type
        }
    },
    createActivity: async function (activity) {
        const result = await axios.post(this.apiUrl('/Activities'), activity);
        
        return result.status;
    }
};

export default ApiClient;