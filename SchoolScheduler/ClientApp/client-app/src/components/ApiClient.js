import axios from 'axios';

const ApiClient = {
    apiUrl: (path) => ('https://localhost:5001/api' + path),
    getDictionaries: async function (slot, id = null) {
        let query = slot.toString();
        if (id !== null)
            query += '?id=' + id.toString();
        const g = await this.getDictionaryBySlot('classGroups/slot/' + query)
        const r = await this.getDictionaryBySlot('rooms/slot/' + query);
        const t = await this.getDictionaryBySlot('teachers/slot/' + query);
        const s = await this.getDictionaryBySlot('subjects/slot/' + query);
        return {
            teachers: t,
            groups: g,
            rooms: r,
            subjects: s
        };
    },
    getDictionary: async function (type) {
        const result = await axios.get(this.apiUrl('/' + type));
        return result.data;
    },
    getDictionaryBySlot: async function (query) {
        const result = await axios.get(this.apiUrl('/' + query));
        return result.data;
    },
    getDictionaryElement: async function (type, id) {
        const result = await axios.get(this.apiUrl('/' + type + '/' + id));
        return result.data;
    },
    createDictionaryElement: async function (type, element) {
        const result = await axios.post(this.apiUrl('/' + type), element);
        return { data: result.data, status: result.status };
    },
    editDictionaryElement: async function (type, element) {
        const result = await axios.put(this.apiUrl('/' + type + '/' + element.Id), element);
        return { data: result.data, status: result.status };
    },
    deleteDictionaryElement: async function (type, element) {
        const result = await axios.delete(this.apiUrl('/' + type + '/' + element.Id + '/' + element.Timestamp));
        return { data: result.data, status: result.status };
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
        const result = await axios.delete(this.apiUrl('/Activities/' + activity.Id + '/' + activity.Timestamp));
        return { data: result.data, status: result.status };
    }
};

export default ApiClient;