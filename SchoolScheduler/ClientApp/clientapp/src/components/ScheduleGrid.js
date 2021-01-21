import Button from 'react-bootstrap/Button'
import 'bootstrap/dist/css/bootstrap.min.css'
import React, { useEffect, useState } from "react";
import axios from 'axios';
import {
    Link
} from "react-router-dom";

const terms = {
    days: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'],
    hours: ['8:15 - 9:00', '9:15 - 10:00', '10:15 - 11:00', '11:15 - 12:00', '12:15 - 13:00', '13:15 - 14:00', '14:15 - 15:00', '15:15 - 16:00', '16:15 - 17:00']
}
const range = (min, max) => Array.from({ length: max - min + 1 }, (_, i) => min + i);

const apiUrl = 'https://localhost:5001/api/Schedule/';

function Days() {
    return (
        <tr>
            <td>
                <Button key="time" className="btn btn-light btn-block">time</Button>
            </td>
            {terms.days.map(dayId => (
                <td>
                    <Button key={dayId} className="btn btn-light btn-block">{dayId}</Button>
                </td>
            ))}
        </tr>
    );
}

function ScheduleButtons(prop) {

    const [scheduleData, setScheduleData] = useState({ slots: [], name: '', type: '' });
    useEffect(() => {
        const fetchData = async () => {
            const result = await axios({
                method: 'get',
                url: apiUrl + prop.type + '/' + prop.name,
                headers: { 'Content-Type': 'application/json' }
            });
            setScheduleData({ slots: result.data.slots, name: result.data.name, type: result.data.type });
        };
        fetchData();
    }, []);
    return (
        <>
            {range(0, 8).map(i => (
                <tr>
                    <td>
                        <Button key={'term' + i.toString()} className="btn btn-light btn-block">{terms.hours[i]}</Button>
                    </td>
                    {scheduleData.slots.slice(i * 5, i * 5 + 5).map((slot, index) => (
                        <td>
                            <Link to="/edit">
                                <Button key={i * 5 + index} className="btn btn-secondary btn-block"> {slot.title} </Button>
                            </Link>
                        </td>
                    ))}
                </tr>
            ))}
        </>
    );
}

function ScheduleGrid(prop) {
    return (
        <div>
            <table className="table">
                <tbody>
                    <Days></Days>
                    <ScheduleButtons type={prop.type} name={prop.name}></ScheduleButtons>
                </tbody>
            </table>
        </div>

    );
}



export default ScheduleGrid;
