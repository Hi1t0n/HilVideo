import React from 'react';
import MainPage from "./Components/MainPage";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import Events from "./Components/Test/Events";
import RegistrationForm from "./Components/RegistrationForm/RegistrationForm";

function App() {
  return (
    <div>
        <BrowserRouter>
            <Routes>
                <Route path={"/"} element={<MainPage/>}/>
                <Route path={"/login"} element={<Events/>}/>
                <Route path={"/registration"} element={<RegistrationForm/>}/>
            </Routes>
        </BrowserRouter>
    </div>
  );
}

export default App;
