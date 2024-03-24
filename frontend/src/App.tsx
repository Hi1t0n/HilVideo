import React from 'react';
import MainPage from "./Components/MainPage";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import RegistrationForm from "./Components/RegistrationForm/RegistrationForm";
import LoginForm from "./Components/LoginForm/LoginForm";

function App() {
  return (
    <div>
        <BrowserRouter>
            <Routes>
                <Route path={"/"} element={<MainPage/>}/>
                <Route path={"/login"} element={<LoginForm/>}/>
                <Route path={"/registration"} element={<RegistrationForm/>}/>
            </Routes>
        </BrowserRouter>
    </div>
  );
}

export default App;
