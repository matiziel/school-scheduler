import './App.css';
import ScheduleGrid from './components/ScheduleGrid.js';
import DropDownList from './components/DropDownList.js';
import NavbarHeader from './components/NavbarHeader.js';
import DictionaryList from './components/DictionaryList.js'
import EditActivity from './components/EditActivity.js'
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Link
} from "react-router-dom";


function App() {
  return (
    <Router>
      <div key="app" className="App">
        <header className="App-header">
          <label>School scheduler</label>
          <NavbarHeader></NavbarHeader>
        </header>
        <div className="App-body">
          <Switch>
            <Route path="/dictionaries">
              <DictionaryList></DictionaryList>
            </Route>
            <Route path="/edit">
              <EditActivity></EditActivity>
            </Route>
            <Route path="/">
              <ScheduleGrid key="scheduleGridGroup" type="room" name="111" />
            </Route >
            <Route path="/group">
              <ScheduleGrid key="scheduleGridGroup" type="group" name="1a" />
            </Route>
            <Route path="/room">
              <ScheduleGrid key="scheduleGridRoom" type="room" name="111" />
            </Route>
            <Route path="/teacher">
              <ScheduleGrid key="scheduleGridTeacher" type="teacher" name="clarkson" />
            </Route>
          </Switch>
        </div>
      </div>
    </Router>
  );
}

export default App;
