import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import React, { useState } from "react";
import 'bootstrap/dist/css/bootstrap.min.css'
import { Link } from 'react-router-dom'


const options = ["a", "b", "c"];
function EditActivity() {
    return (
        <div>
            <Form>
                <Form.Group controlId="formBasicEmail">
                    <Form.Label>Email address</Form.Label>
                    <br></br>
                    <select className="selectpicker">
                        {options.map((option) => (
                            <option value={option}>{option}</option>
                        ))}
                    </select>
                </Form.Group>

                <Form.Group controlId="formBasicPassword">
                    <Form.Label>Password</Form.Label>
                    <Form.Control type="password" placeholder="Password" />
                </Form.Group>
                <Button variant="primary" type="submit">
                    Submit
                </Button>
            </Form>
        </div>
    );
}
export default EditActivity;
