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
    createDictionaryElement: async function (element) {

    },
    editDictionaryElement: async function (element) {

    },
    deleteDictionaryElement: async function (element) {

    },
    getSchedule: async function (type, name) {
        const result = await axios.get(this.apiUrl('/Schedule/' + type + '/' + name));
        return result.data;
    },
    getActivity: async function (id) {
        const result = await axios.get(this.apiUrl('/Activities/' + id.toString()));
        return result.data
    },
    createActivity: async function (activity) {
        const result = await axios.post(this.apiUrl('/Activities'), activity);
        return { data: result.data, status: result.status };
    },
    updateActivity: async function (activity) {
        const result = await axios.put(this.apiUrl('/Activities/' + activity.Id), activity);
        return { data: result.data, status: result.status };
    },
    deleteActivity: async function (activity) {
        console.log(activity);
        const result = axios.delete(this.apiUrl('/Activities/' + activity.Id + '/' + activity.Timestamp));
        return { data: result.data, status: result.status };
    }
};

export default ApiClient;