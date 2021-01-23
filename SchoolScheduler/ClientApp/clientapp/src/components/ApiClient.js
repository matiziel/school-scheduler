import axios from 'axios';
import Utils from './Utils.js';

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

        let x = {
            teachers: t.data,
            groups: g.data,
            rooms: r.data,
            subjects: s.data
        };
        console.log(x);
        return x;
    },
    getDictionary: async function (type) {
        const result = await axios({
            method: 'get',
            url: this.apiUrl('/Dictionaries/all/' + type),
            headers: { 'Content-Type': 'application/json' }
        });
        return result.data;
    },
    getDictionaryBySlot: async function (query) {
        const result = await axios({
            method: 'get',
            url: this.apiUrl('/Dictionaries/' + query),
            headers: { 'Content-Type': 'application/json' }
        });
        return result.data;
    },
    getSchedule: async function (type, name) {
        const result = await axios({
            method: 'get',
            url: this.apiUrl('/Schedule/' + type + '/' + name),
            headers: { 'Content-Type': 'application/json' }
        });
        return {
            slots: result.data.slots,
            name: result.data.name,
            type: result.data.type
        }
    }
};

export default ApiClient;