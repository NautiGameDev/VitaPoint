import './App.css';
import { Routes, Route } from 'react-router-dom';
import Login from "./Pages/Login/Login";
import Register from "./Pages/Register/Register"

function App() {
    
    return (
        <div className="app-container">
            <Routes>
                <Route index element={<Login />} />
                <Route path="/Register" element={<Register />} />
            </Routes>
        </div>
    );
}

export default App;