import 'bootstrap/dist/css/bootstrap.min.css'
import React from "react";
import { useParams } from "react-router-dom";



function Error() {
    let { message } = useParams();
    return (
        <div>
            <h2>{message}</h2>
        </div>
    );
}

export default Error;
