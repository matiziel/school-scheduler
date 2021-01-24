import React, { useState, useEffect } from "react";
import 'bootstrap/dist/css/bootstrap.min.css'
import { useForm } from "react-hook-form";
import { useParams } from "react-router-dom";
import Utils from './Utils.js';
import ApiClient from './ApiClient.js';

function EditActivity() {
    let { id, slot, type } = useParams();
    const { register, handleSubmit } = useForm();
    const { register: deleteRegister, handleSubmit: deleteHandleSubmit } = useForm();
    const [selectLists, setSelectLists] = useState({ teachers: [], subjects: [], rooms: [], groups: [] });
    const [activity, setActivity] = useState({
        id: "",
        classGroup: "",
        teacher: "",
        subject: "",
        room: "",
        slot: "",
        timestamp: "",
    });
    useEffect(() => {
        const fetchData = async () => {
            const resultActivity = await ApiClient.getActivity(id);
            setActivity(resultActivity);
            const resultList = await ApiClient.getDictionaries(parseInt(slot), id);
            setSelectLists(resultList);
        };
        fetchData();
    }, [id, slot, type]);
    const onSubmit = async (data) => {
        data.Slot = parseInt(data.Slot);
        data.Id = parseInt(data.Id);
        const result = await ApiClient.updateActivity(data);
        console.log(result);
    }
    const onSubmitDelete = async (data) => {
        data.Id = parseInt(data.Id);
        const result = await ApiClient.deleteActivity(data);
        console.log(result);
    }
    return (
        <div class="col-md-4">
            <label>{Utils.getTermBySlot(slot)}</label>
            <form onSubmit={handleSubmit(onSubmit)}>
                {
                    type !== 'Group'
                        ? <div className="form-group">
                            <label>
                                Group:
                        <select className="form-control" name="ClassGroup" ref={register} visible={(type === 'Group').toString()} >
                                    {selectLists.groups.map(item => {
                                        if (item === activity.classGroup)
                                            return (<option value={item} selected>{item}</option>);
                                        else
                                            return (<option value={item}>{item}</option>);
                                    }
                                    )}
                                </select>
                            </label>
                        </div>
                        : <input type="hidden" name="ClassGroup" ref={register} value={activity.classGroup} />
                }
                {
                    type !== 'Teacher'
                        ? < div className="form-group" >
                            <label>
                                Teacher:
                        <select className="form-control" name="Teacher" ref={register} >
                                    {selectLists.teachers.map(item => {
                                        if (item === activity.teacher)
                                            return (<option value={item} selected>{item}</option>);
                                        else
                                            return (<option value={item}>{item}</option>);
                                    }
                                    )}
                                </select>
                            </label>
                        </div>
                        : <input type="hidden" name="Teacher" ref={register} value={activity.teacher} />
                }
                <div className="form-group">
                    <label>
                        Subject:
                    <select className="form-control" name="Subject" ref={register} >
                            {selectLists.subjects.map(item => {
                                if (item === activity.subject)
                                    return (<option value={item} selected>{item}</option>);
                                else
                                    return (<option value={item}>{item}</option>);
                            }
                            )}
                        </select>
                    </label>
                </div>
                {
                    type !== 'Room'
                        ? <div className="form-group">
                            <label>
                                Room:
                        <select className="form-control" name="Room" ref={register} visible={(type === 'Room').toString()}>
                                    {selectLists.rooms.map(item => {
                                        if (item === activity.room)
                                            return (<option value={item} selected>{item}</option>);
                                        else
                                            return (<option value={item}>{item}</option>);
                                    }
                                    )}</select>
                            </label>
                        </div>
                        : <input type="hidden" name="Room" ref={register} value={activity.room} />
                }
                <input type="hidden" name="Slot" ref={register} value={activity.slot} />
                <input type="hidden" name="Id" ref={register} value={activity.id} />
                <input type="hidden" name="Timestamp" ref={register} value={activity.timestamp} />
                <input className="btn btn-primary" value="Save" type="submit" />
            </form>

            <form onSubmit={deleteHandleSubmit(onSubmitDelete)}>
                <input type="hidden" name="Id" ref={deleteRegister} value={activity.id} />
                <input type="hidden" name="Timestamp" ref={deleteRegister} value={activity.timestamp} />
                <input className="btn btn-danger" value="Delete" type="submit" />
            </form>
        </div>
    );
}
export default EditActivity;