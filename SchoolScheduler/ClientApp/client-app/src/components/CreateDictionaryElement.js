import React, { useState, useEffect } from "react";
import 'bootstrap/dist/css/bootstrap.min.css'
import { useForm } from "react-hook-form";
import {
    useParams,
    useHistory, Link
} from "react-router-dom";
import Utils from './Utils.js';
import ApiClient from './ApiClient.js';


function CreateDictionaryElement() {
    let { type } = useParams();
    const { register, handleSubmit } = useForm();
    const history = useHistory();


    const onSubmit = async (data) => {
        const result = await ApiClient.createDictionaryElement(type, data);
        if(result.status === 200)
            history.push("/dictionaries/" + type)
    }
    return (
        <div class="col-md-4">
            <label>{"Create " + type}</label>
            <form onSubmit={handleSubmit(onSubmit)}>
                <div className="form-group">
                    <label>
                        Name:
                    <input className="form-control" name="Name" ref={register({ required: true })} />
                    </label>
                </div>
                <div className="form-group">
                    <label>
                        Comment:
                    <input className="form-control" type="textarea" name="Comment" ref={register} />
                    </label>
                </div>
                <input className="btn btn-primary" type="submit" />
            </form>
            <Link to={"/dictionaries/" + type} className="App-link">Back to list</Link>
        </div>
    );

}
export default CreateDictionaryElement;


