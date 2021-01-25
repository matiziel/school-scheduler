import React, { useEffect, useState } from "react";
import DropdownButton from 'react-bootstrap/DropdownButton'
import Dropdown from 'react-bootstrap/Dropdown'
import 'bootstrap/dist/css/bootstrap.min.css'
import {
    Link,
    useParams,
    useHistory
} from "react-router-dom";
import ApiClient from './ApiClient.js';
import './../App.css';




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
            const result = await ApiClient.getDictionary(type);
            setdictionaryList({ elements: result });
        };
        fetchData();
    });
    return (
        <div>
            <Link to={"/dictionariesCreate/" + type} className="App-link">Create</Link>
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
                                <Link to={"/dictionariesEdit/" + type + "/" + item.id} className="App-link">Edit</Link>
                            </td>
                            <td>
                                <Link to={"/dictionariesDelete/" + type + "/" + item.id} className="App-link">Delete</Link>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>

    );
}



export default DictionaryList;