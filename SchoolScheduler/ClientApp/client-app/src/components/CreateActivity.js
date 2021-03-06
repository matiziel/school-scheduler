import React, { useState, useEffect } from "react";
import 'bootstrap/dist/css/bootstrap.min.css'
import { useForm } from "react-hook-form";
import {
    useParams,
    useHistory,
    Link
} from "react-router-dom";
import Utils from './Utils.js';
import ApiClient from './ApiClient.js';


function CreateActivity() {
    let { id, slot, type, typeName } = useParams();
    const { register, handleSubmit } = useForm();
    const [selectLists, setSelectLists] = useState({ teachers: [], subjects: [], rooms: [], groups: [] });
    const history = useHistory();

    useEffect(() => {
        const fetchData = async () => {
            const result = await ApiClient.getDictionaries(slot);
            setSelectLists(result);
        };
        fetchData();
    }, [id, slot, type, typeName]);

    const onSubmit = async (data) => {
        data.Slot = parseInt(data.Slot);
        const result = await ApiClient.createActivity(data);
        if (result.status === 200)
            history.push("/" + type + "/" + typeName);
        else
            history.push("/error/" + result.data.message);
    }
    return (
        <div class="col-md-4">
            <label>{Utils.getTermBySlot(slot)}</label>
            <form onSubmit={handleSubmit(onSubmit)}>
                {
                    type !== 'classGroups'
                        ? <div className="form-group">
                            <label>
                                Group:
                        <select className="form-control" name="ClassGroup" ref={register}>
                                    {selectLists.groups.map(item =>
                                        <option value={item}>{item}</option>
                                    )}
                                </select>
                            </label>
                        </div>
                        : <input type="hidden" name="ClassGroup" ref={register} value={typeName} />
                }
                {
                    type !== 'teachers'
                        ? <div className="form-group">
                            <label>
                                Teacher:
                        <select className="form-control" name="Teacher" ref={register}>
                                    {selectLists.teachers.map(item =>
                                        <option value={item}>{item}</option>
                                    )}
                                </select>
                            </label>
                        </div>
                        : <input type="hidden" name="Teacher" ref={register} value={typeName} />
                }
                <div className="form-group">
                    <label>
                        Subject:
                    <select className="form-control" name="Subject" ref={register}>
                            {selectLists.subjects.map(item =>
                                <option value={item}>{item}</option>
                            )}
                        </select>
                    </label>
                </div>
                {
                    type !== 'rooms'
                        ? <div className="form-group">
                            <label>
                                Room:
                    <select className="form-control" name="Room" ref={register}>
                                    {selectLists.rooms.map(item =>
                                        <option value={item}>{item}</option>
                                    )}</select>
                            </label>
                        </div>
                        : <input type="hidden" name="Room" ref={register} value={typeName} />
                }

                <input type="hidden" name="Slot" ref={register} value={slot} />
                <input className="btn btn-primary" type="submit" />
            </form>
            <Link to={"/" + type + "/" + typeName} className="App-link">Back to list</Link>
        </div>
    );

}
export default CreateActivity;


