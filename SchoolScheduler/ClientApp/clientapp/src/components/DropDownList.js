import DropdownButton from 'react-bootstrap/DropdownButton'
import Dropdown from 'react-bootstrap/Dropdown'
import React, { useState } from "react";
import 'bootstrap/dist/css/bootstrap.min.css'
import { Link } from 'react-router-dom'


function DropDownList() {
    const [title, setTitle] = useState('')
    const handleSelect = (e) => {
        setTitle(e);
        console.log(e);
    }
    return (
        <div>
            <DropdownButton id="dropdown-basic-button" title="elo" onSelect={handleSelect}>
                <Dropdown.Item >Home</Dropdown.Item>
                <Dropdown.Item >Edit</Dropdown.Item>
            </DropdownButton>
            <h4>You selected {title}</h4>
        </div>
    );
}
export default DropDownList;
