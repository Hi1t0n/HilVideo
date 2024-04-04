import React from 'react';
import MainPage from "./Components/MainPage/MainPage";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import RegistrationForm from "./Components/RegistrationForm/RegistrationForm";
import LoginForm from "./Components/LoginForm/LoginForm";
import AdminPanel from "./Components/AdminPanel/AdminPanel";
import Profile from "./Components/Profile/Profile";
import RequiredAuth from "./hoc/RequiredAuth";
import RequiredAuthAndOwnerOrAdminRole from "./hoc/RequiredAuthAndOwnerOrAdminRole";


function App() {
  return (
    <div>
        <BrowserRouter>
            <Routes>
                <Route path={"/"} element={<MainPage/>}>
                    <Route path={"/profile"} element={
                        <RequiredAuth>
                            <Profile/>
                        </RequiredAuth>
                    }/>
                    <Route path={"/adminpanel"} element={
                        <RequiredAuthAndOwnerOrAdminRole>
                            <AdminPanel/>
                        </RequiredAuthAndOwnerOrAdminRole>
                    }/>
                </Route>
                <Route path={"/login"} element={<LoginForm/>}/>
                <Route path={"/registration"} element={<RegistrationForm/>}/>
            </Routes>
        </BrowserRouter>
    </div>
  );
}

export default App;
