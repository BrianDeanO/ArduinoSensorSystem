import './App.css';
import LordNewell from './images/gabe_newell_meme.jpg';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={LordNewell} className="App-logo" alt="logo" />
        <p>
          CPTS 421 - Arduino Sensor System
        </p>
        <div>
          Test App
        </div>
      </header>
    </div>
  );
}

export default App;
