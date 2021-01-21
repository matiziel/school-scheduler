import './App.css';
import ScheduleGrid from './components/ScheduleGrid.js';


function App() {
  return (
    <div key="app" className="App">
      <header className="App-header">
        <ScheduleGrid key="scheduleGrid" />
      </header>
    </div>
  );
}

export default App;
