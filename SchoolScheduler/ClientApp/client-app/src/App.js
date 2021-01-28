import './App.css';
import React, { useState, useEffect } from "react";
import ScheduleGrid from './components/ScheduleGrid.js';
import DictionaryList from './components/DictionaryList.js';
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Link
} from "react-router-dom";
import CreateActivity from './components/CreateActivity.js';
import EditActivity from './components/EditActivity.js';
import CreateDictionaryElement from './components/CreateDictionaryElement.js';
import EditDictionaryElement from './components/EditDictionaryElement.js';
import DeleteDictionaryElement from './components/DeleteDictionaryElement.js';
import ApiClient from './components/ApiClient.js';
import Error from './components/Error.js';



function App() {
  const [elements, setElements] = useState({
    teacher: "",
    room: "",
    group: ""
  });
  useEffect(() => {
    const fetchData = async () => {
      const result = await ApiClient.getDictionariesFirstElements();
      setElements(result);
    };
    fetchData();
  }, []);

  return (
    <Router>
      <div key="app" className="App">
        <header className="App-header">
          <label>School scheduler</label>
        </header>
        <header className="App-header">
          <Link className="App-link" to={"/classGroups/" + elements.group}>Group</Link>
          <Link className="App-link" to={"/teachers/" + elements.teacher}>Teacher</Link>
          <Link className="App-link" to={"/rooms/" + elements.room}>Room</Link>
          <Link className="App-link" to="/dictionaries/classGroups">Dictionaries</Link>
        </header>
        <hr className="Line"></hr>
        <div className="App-body">
          <Switch>
            <Route path="/dictionaries/:type">
              <DictionaryList></DictionaryList>
            </Route>
            <Route path="/dictionariesCreate/:type">
              <CreateDictionaryElement></CreateDictionaryElement>
            </Route>
            <Route path="/dictionariesEdit/:type/:id">
              <EditDictionaryElement></EditDictionaryElement>
            </Route>
            <Route path="/dictionariesDelete/:type/:id">
              <DeleteDictionaryElement></DeleteDictionaryElement>
            </Route>
            <Route path="/edit/:type/:typeName/:id/:slot">
              <EditActivity></EditActivity>
            </Route>
            <Route path="/create/:type/:typeName/:slot">
              <CreateActivity></CreateActivity>
            </Route>
            <Route path="/classGroups/:searchName">
              <ScheduleGrid key="scheduleGridGroup" type="classGroups" />
            </Route >
            <Route path="/teachers/:searchName">
              <ScheduleGrid key="scheduleGridTeacher" type="teachers" />
            </Route >
            <Route path="/rooms/:searchName">
              <ScheduleGrid key="scheduleGridRoom" type="rooms" />
            </Route >
            <Route path="/error/:message">
              <Error key="error" />
            </Route >
          </Switch>
        </div>
      </div>
    </Router>
  );
}

export default App;
