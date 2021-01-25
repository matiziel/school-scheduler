import React, { useState, useEffect } from "react";
import 'bootstrap/dist/css/bootstrap.min.css'
import { useForm } from "react-hook-form";
import {
    useParams,
    useHistory,
    Link
} from "react-router-dom";
import ApiClient from './ApiClient.js';


function EditDictionaryElement() {
    let { type, id } = useParams();
    const { register, handleSubmit } = useForm();
    const [element, setElement] = useState({
        id: "",
        name: "",
        comment: "",
        timestamp: "",
    });
    const history = useHistory();

    useEffect(() => {
        const fetchData = async () => {
            const result = await ApiClient.getDictionaryElement(type, id);
            setElement(result);
        };
        fetchData();
    }, []);

    const onSubmit = async (data) => {
        data.Id = parseInt(data.Id);
        const result = await ApiClient.editDictionaryElement(type, data);
        if (result.status === 200)
            history.push("/dictionaries/" + type);
    }
    const onChangeName = (e) => {
        setElement({
            id: element.id,
            name: e.target.value,
            comment: element.comment,
            timestamp: element.timestamp,
        });

    }
    const onChangeComment = (e) => {
        setElement({
            id: element.id,
            name: element.name,
            comment: e.target.value,
            timestamp: element.timestamp,
        });
    }
    return (
        <div class="col-md-4">
            <label>{"Edit " + type}</label>
            <form onSubmit={handleSubmit(onSubmit)}>
                <div className="form-group">
                    <label>
                        Name:
                    <input className="form-control" name="Name" onChange={onChangeName} ref={register({ required: true })} value={element.name} />
                    </label>
                </div>
                <div className="form-group">
                    <label>
                        Comment:
                    <input className="form-control" type="textarea" name="Comment" onChange={onChangeComment} ref={register} value={element.comment === null ? "" : element.comment} />
                    </label>
                </div>
                <input type="hidden" name="Id" ref={register} value={element.id} />
                <input type="hidden" name="Timestamp" ref={register} value={element.timestamp} />
                <input className="btn btn-primary" type="submit" />
            </form>
            <Link to={"/dictionaries/" + type} className="App-link">Back to list</Link>
        </div>
    );

}
export default EditDictionaryElement;