import Button from 'react-bootstrap/Button'
import DropdownButton from 'react-bootstrap/DropdownButton'
import Dropdown from 'react-bootstrap/Dropdown'
import 'bootstrap/dist/css/bootstrap.min.css'
import React, { useEffect, useState } from "react";
import axios from 'axios';
import {
    Link, useParams, useHistory, Router
} from "react-router-dom";
const terms = {
    days: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'],
    hours: ['8:15 - 9:00', '9:15 - 10:00', '10:15 - 11:00', '11:15 - 12:00', '12:15 - 13:00', '13:15 - 14:00', '14:15 - 15:00', '15:15 - 16:00', '16:15 - 17:00']
}
const range = (min, max) => Array.from({ length: max - min + 1 }, (_, i) => min + i);


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

function ScheduleButtons(props) {
    const apiUrl = 'https://localhost:5001/api/Schedule/';
    const [scheduleData, setScheduleData] = useState({ slots: [], name: '', type: '' });
    useEffect(() => {
        const fetchData = async () => {
            const result = await axios({
                method: 'get',
                url: apiUrl + props.type + '/' + props.name,
                headers: { 'Content-Type': 'application/json' }
            });
            setScheduleData({ slots: result.data.slots, name: result.data.name, type: result.data.type });
        };
        fetchData();
    });
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


function ScheduleGrid(props) {
    let { searchName } = useParams();
    const [value, setValue] = useState(searchName);
    const history = useHistory();
    const handleSelect = (e) => {
        setValue(e);
        history.push("/" + props.type + "/" + e);
    }
    const apiUrl = 'https://localhost:5001/api/Dictionaries/all/'
    const [dictionaryList, setDictionaryList] = useState({ nameList: [] });
    useEffect(() => {
        const fetchData = async () => {
            const result = await axios({
                method: 'get',
                url: apiUrl + props.type,
                headers: { 'Content-Type': 'application/json' }
            });
            setDictionaryList({ nameList: result.data });
        };
        fetchData();
    }, []);
    return (
        <div>

            <DropdownButton
                alignRight
                title={value}
                id="dropdown-menu-align-right"
                onSelect={handleSelect}>
                {dictionaryList.nameList.map(item => (
                    <Dropdown.Item eventKey={item.name}>{item.name}
                        {/* <Link className="App-link" to={"/" + props.type + "/" + item.name}>{item.name}</Link> */}
                    </Dropdown.Item>
                ))}
            </DropdownButton>

            <br></br>
            <table className="table">
                <tbody>

                    <Days></Days>
                    <ScheduleButtons type={props.type} name={searchName}></ScheduleButtons>
                </tbody>
            </table>
        </div>

    );
}



export default ScheduleGrid;
