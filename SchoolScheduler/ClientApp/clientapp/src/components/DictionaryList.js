import React, { useEffect, useState } from "react";
import DropdownButton from 'react-bootstrap/DropdownButton'
import Dropdown from 'react-bootstrap/Dropdown'
import 'bootstrap/dist/css/bootstrap.min.css'
import axios from 'axios';
import {
    Link,
    useParams,
    useHistory
} from "react-router-dom";
import './../App.css';

const apiUrl = 'https://localhost:5001/api/Dictionaries/all/'

function DictionaryList() {
    let { type } = useParams();
    const [dictionaryList, setdictionaryList] = useState({ elements: [] });
    const [value, setValue] = useState(type);
    const history = useHistory();
    const handleSelect = (e) => {
        setValue(e);
        history.push("/dictionaries/" + e);
    }
    useEffect(() => {
        const fetchData = async () => {
            const result = await axios({
                method: 'get',
                url: apiUrl + type,
                headers: { 'Content-Type': 'application/json' }
            });
            setdictionaryList({ elements: result.data });
        };
        fetchData();
    });
    return (
        <div>
            <DropdownButton
                alignRight
                title={value}
                id="dropdown-menu-align-right"
                onSelect={handleSelect}>
                <Dropdown.Item eventKey="classGroups">classGroups</Dropdown.Item>
                <Dropdown.Item eventKey="teachers">teachers</Dropdown.Item>
                <Dropdown.Item eventKey="rooms">rooms</Dropdown.Item>
                <Dropdown.Item eventKey="subjects">subjects</Dropdown.Item>
            </DropdownButton>
            <table className="table">
                <thead>
                    <tr>
                        <label>Name</label>
                    </tr>
                </thead>
                <tbody>
                    {dictionaryList.elements.map(item => (
                        <tr>
                            <td>
                                <label>{item.name}</label>
                            </td>
                            <td>
                                <Link to="/edit" className="App-link">Edit</Link>
                            </td>
                            <td>
                                <Link to="/edit" className="App-link">Delete</Link>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>

    );
}



export default DictionaryList;