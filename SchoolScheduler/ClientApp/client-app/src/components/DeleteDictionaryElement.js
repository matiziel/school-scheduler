import React, { useState, useEffect } from "react";
import 'bootstrap/dist/css/bootstrap.min.css'
import { useForm } from "react-hook-form";
import {
    useParams,
    useHistory,
    Link
} from "react-router-dom";
import ApiClient from './ApiClient.js';


function DeleteDictionaryElement() {
    let { type, id } = useParams();
    const { register, handleSubmit } = useForm();
    const [element, setElement] = useState({
        id: "",
        name: "",
        comment: "",
        timestamp: "",
    });
    // const [selectLists, setSelectLists] = useState({ teachers: [], subjects: [], rooms: [], groups: [] });
    const history = useHistory();

    useEffect(() => {
        const fetchData = async () => {
            const result = await ApiClient.getDictionaryElement(type, id);
            setElement(result);
        };
        fetchData();
    }, [id, type]);

    const onSubmit = async (data) => {
        const result = await ApiClient.deleteDictionaryElement(type, data);
        if (result.status === 200)
            history.push("/dictionaries/" + type);
        else
            history.push("/error/" + result.data.message);
    }
    return (
        <div class="col-md-4">
            <h3>Are you sure you want to delete this?</h3>
            <h3>All activities related to this element will be deleted.</h3>
            <br></br>
            <label>{"Name: " + element.name}</label>
            <br></br>
            <label>{"Comment: " + (element.comment === null ? "" : element.comment)}</label>

            <form onSubmit={handleSubmit(onSubmit)}>
                <input type="hidden" name="Id" ref={register} value={element.id} />
                <input type="hidden" name="Timestamp" ref={register} value={element.timestamp} />
                <input className="btn btn-danger" type="submit" value="Delete" />
            </form>
            <Link to={"/dictionaries/" + type} className="App-link">Back to list</Link>
        </div>
    );

}
export default DeleteDictionaryElement;