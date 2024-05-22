import React from 'react';
import MainPage from "./Pages/MainPage/MainPage";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import RegistrationForm from "./Pages/RegistrationForm/RegistrationForm";
import LoginForm from "./Pages/LoginForm/LoginForm";
import Profile from "./Pages/Profile/Profile";
import RequiredAuth from "./hoc/RequiredAuth";
import RequiredAuthAndOwnerOrAdminRole from "./hoc/RequiredAuthAndOwnerOrAdminRole";
import AdminPanel from "./Pages/AdminPanel/AdminPanel";
import AddMoviePage from "./Pages/AdminPages/AddMoviePage";
import MoviePage from "./Pages/MoviePage/MoviePage";
import MainMoviesPage from "./Pages/mainMoviesPage/MainMoviesPage";


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
                    <Route path={"/"} element={<MainMoviesPage/>}/>
                    <Route path={"/movie/:id"} element={<MoviePage/>}/>

                </Route>
                <Route path={"/login"} element={<LoginForm/>}/>
                <Route path={"/registration"} element={<RegistrationForm/>}/>
                <Route path={"/adminpanel"} element={
                    <RequiredAuthAndOwnerOrAdminRole>
                        <AdminPanel/>
                    </RequiredAuthAndOwnerOrAdminRole>
                }>
                    <Route path={"movie"}>
                        <Route path={"add"} element={<AddMoviePage/>}/>
                        <Route path={"delete"}/>
                    </Route>
                    <Route path={"admin"}>
                        <Route path={"add"}/>
                        <Route path={"remove"}/>
                    </Route>
                </Route>
            </Routes>
        </BrowserRouter>
    </div>
  );
}

export default App;
