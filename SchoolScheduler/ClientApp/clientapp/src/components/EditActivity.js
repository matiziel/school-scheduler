import React, { useState, useEffect } from "react";
import 'bootstrap/dist/css/bootstrap.min.css'
import { useForm } from "react-hook-form";
import {
    useParams
} from "react-router-dom";
import Utils from './Utils.js';
import ApiClient from './ApiClient.js';

function EditActivity() {
    // let { id } = useParams();
    // const { register, handleSubmit } = useForm();
    // const onSubmit = data => console.log(data);
    // const [selectLists, setSelectLists] = useState({ teachers: [], subjects: [], rooms: [], groups: [] });
    // useEffect(() => {
    //     const fetchData = async () => {
    //         const data = await ApiClient.getDictionaries(slot);
    //         setSelectLists({
    //             teachers: data.teachers,
    //             groups: data.groups,
    //             rooms: data.rooms,
    //             subjects: data.subjects
    //         });
    //     };
    //     fetchData();
    // }, [id]);
    return (
        <div></div>
        // <>
        //     <label>{ApiClient.getTermBySlot(slot)}</label>
        //     <form onSubmit={handleSubmit(onSubmit)}>
        //         <div className="form-group">
        //             <label>
        //                 Group:
        //             <select name="ClassGroup" ref={register}>
        //                     {selectLists.groups.map(item =>
        //                         <option value={item}>{item}</option>
        //                     )}
        //                 </select>
        //             </label>
        //         </div>
        //         <div className="form-group">
        //             <label>
        //                 Teacher:
        //         <select name="Teacher" ref={register}>
        //                     {selectLists.teachers.map(item =>
        //                         <option value={item}>{item}</option>
        //                     )}
        //                 </select>
        //             </label>
        //         </div>
        //         <div className="form-group">
        //             <label>
        //                 Subject:
        //         <select name="Subject" ref={register}>
        //                     {selectLists.subjects.map(item =>
        //                         <option value={item}>{item}</option>
        //                     )}
        //                 </select>
        //             </label>
        //         </div>
        //         <div className="form-group">
        //             <label>
        //                 Room:
        //         <select name="Room" ref={register}>
        //                     {selectLists.rooms.map(item =>
        //                         <option value={item}>{item}</option>
        //                     )}
        //                 </select>
        //             </label>
        //         </div>
        //         <input type="hidden" name="Slot" ref={register} value={slot} />
        //         <input type="submit" />
        //     </form>
        // </>
    );

}
export default EditActivity;