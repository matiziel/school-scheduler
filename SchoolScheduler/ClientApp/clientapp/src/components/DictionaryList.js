import 'bootstrap/dist/css/bootstrap.min.css'
import React, { useEffect, useState } from "react";
import axios from 'axios';
import {
    Link
} from "react-router-dom";
import './../App.css';

const apiUrl = 'https://localhost:5001/api/Dictionaries/all/Teacher'

function DictionaryList() {
    const [dictionaryList, setdictionaryList] = useState({ elements: [] });
    useEffect(() => {
        const fetchData = async () => {
            const result = await axios({
                method: 'get',
                url: apiUrl,
                headers: { 'Content-Type': 'application/json' }
            });
            setdictionaryList({ elements: result.data });
        };
        fetchData();
    }, []);
    return (
        <div>
            <table className="table">
                <thead>
                    <tr>
                        <th>
                            <label>Name</label>
                        </th>
                        <th></th>
                        <th></th>
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