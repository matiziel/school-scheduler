import Button from 'react-bootstrap/Button'
import DropdownButton from 'react-bootstrap/DropdownButton'
import Dropdown from 'react-bootstrap/Dropdown'
import 'bootstrap/dist/css/bootstrap.min.css'
import React, { useEffect, useState } from "react";
import {
    Link, useParams, useHistory
} from "react-router-dom";
import Utils from './Utils.js';
import ApiClient from './ApiClient.js';


function Days() {
    return (
        <tr>
            <td>
                <Button key="time" className="btn btn-light btn-block">time</Button>
            </td>
            {Utils.terms.days.map(dayId => (
                <td>
                    <Button key={dayId} className="btn btn-light btn-block">{dayId}</Button>
                </td>
            ))}
        </tr>
    );
}

function ScheduleButtons(props) {
    const [scheduleData, setScheduleData] = useState({ slots: [], name: '', type: '' });
    useEffect(() => {
        const fetchData = async () => {
            const result = await ApiClient.getSchedule(props.type, props.name);
            setScheduleData(result);
        };
        fetchData();
    }, [props.name, props.type]);
    return (
        <>
            {Utils.range(0, 8).map(i => (
                <tr>
                    <td>
                        <Button key={'term' + i.toString()} className="btn btn-light btn-block">{Utils.terms.hours[i]}</Button>
                    </td>
                    {scheduleData.slots.slice(i * 5, i * 5 + 5).map((slot, index) => {
                        if (slot.id === null) {
                            return (
                                <td>
                                    <Link to={"/create/" + scheduleData.type + "/" + scheduleData.name + "/" + (i * 5 + index).toString()}>
                                        <Button key={i * 5 + index} className="btn btn-secondary btn-block"> {slot.title} </Button>
                                    </Link>
                                </td>
                            )
                        }
                        else {
                            return (
                                <td>
                                    <Link to={"/edit/" + scheduleData.type + "/" + slot.id + "/" + (i * 5 + index).toString()}>
                                        <Button key={i * 5 + index} className="btn btn-secondary btn-block"> {slot.title} </Button>
                                    </Link>
                                </td>
                            )

                        }
                    })}
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
    const [dictionaryList, setDictionaryList] = useState({ nameList: [] });
    useEffect(() => {
        const fetchData = async () => {
            const result = await ApiClient.getDictionary(props.type);
            setDictionaryList({ nameList: result });
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
                    <Dropdown.Item eventKey={item.name}>{item.name}</Dropdown.Item>
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
