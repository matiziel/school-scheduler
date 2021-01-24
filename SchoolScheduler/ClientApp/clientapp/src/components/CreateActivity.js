import React, { useState, useEffect } from "react";
import 'bootstrap/dist/css/bootstrap.min.css'
import { useForm } from "react-hook-form";
import {
    useParams
} from "react-router-dom";
import Utils from './Utils.js';
import ApiClient from './ApiClient.js';

function CreateActivity() {
    let { slot } = useParams();
    const { register, handleSubmit } = useForm();
    const [selectLists, setSelectLists] = useState({ teachers: [], subjects: [], rooms: [], groups: [] });
    useEffect(() => {
        const fetchData = async () => {
            const result = await ApiClient.getDictionaries(slot);
            setSelectLists(result);
        };
        fetchData();
    }, [slot]);

    const onSubmit = async (data) => {
        data.Slot = parseInt(data.Slot);
        ApiClient.createActivity(data).then(result => console.log(result));
    }
    return (
        <>
            <label>{Utils.getTermBySlot(slot)}</label>
            <form onSubmit={handleSubmit(onSubmit)}>
                <div className="form-group">
                    <label>
                        Group:
                    <select name="ClassGroup" ref={register}>
                            {selectLists.groups.map(item =>
                                <option value={item}>{item}</option>
                            )}
                        </select>
                    </label>
                </div>
                <div className="form-group">
                    <label>
                        Teacher:
                <select name="Teacher" ref={register}>
                            {selectLists.teachers.map(item =>
                                <option value={item}>{item}</option>
                            )}
                        </select>
                    </label>
                </div>
                <div className="form-group">
                    <label>
                        Subject:
                <select name="Subject" ref={register}>
                            {selectLists.subjects.map(item =>
                                <option value={item}>{item}</option>
                            )}
                        </select>
                    </label>
                </div>
                <div className="form-group">
                    <label>
                        Room:
                <select name="Room" ref={register}>
                            {selectLists.rooms.map(item =>
                                <option value={item}>{item}</option>
                            )}
                        </select>
                    </label>
                </div>
                <input type="hidden" name="Slot" ref={register} value={slot} />
                <input type="submit" />
            </form>
        </>
    );

}
export default CreateActivity;


