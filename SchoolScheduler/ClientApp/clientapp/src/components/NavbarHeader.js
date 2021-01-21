import Navbar from 'react-bootstrap/Navbar'
import Nav from 'react-bootstrap/Nav'
import 'bootstrap/dist/css/bootstrap.min.css'
import { Link } from 'react-router-dom'


function DropDownList() {
    return (
        <div>
            <Navbar bg="light" variant="light">
                <Nav className="mr-auto">
                    <Nav.Link as={Link} to="/group">Groups</Nav.Link>
                    <Nav.Link as={Link} to="/room">Rooms</Nav.Link>
                    <Nav.Link as={Link} to="/teacher">Teachers</Nav.Link>
                    <Nav.Link as={Link} to="/dictionaries">Dictionaries</Nav.Link>
                </Nav>
            </Navbar>
        </div>
    );
}

export default DropDownList;
