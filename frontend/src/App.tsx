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
import NotFoundPage from "./Pages/ErrorPages/NotFoundPage/NotFoundPage";
import AddBookPage from "./Pages/AdminPages/AddBookPage";
import MainBooksPage from "./Pages/mainBooksPage/MainBooksPage";
import {Book} from "@mui/icons-material";
import BookPage from "./Pages/BookPage/BookPage";
import MakeAdminPage from "./Pages/AdminPages/MakeAdminPage";
import RemoveAdminPage from "./Pages/AdminPages/RemoveAdminPage";
import DeleteMoviePage from "./Pages/AdminPages/DeleteMoviePage";
import DeleteBookPage from "./Pages/AdminPages/DeleteBookPage";


function App() {
  return (
    <div>
        <BrowserRouter>
            <Routes>
                <Route path={"/"} element={<MainPage/>}>
                    <Route path={"profile"} element={
                        <RequiredAuth>
                            <Profile/>
                        </RequiredAuth>
                    }>

                    </Route>
                    <Route path={"/favorites"}>
                        <Route path={"books"} element={<MainBooksPage/>}/>
                        <Route path={"movies"} element={<MainMoviesPage/>}/>
                    </Route>
                    <Route path={"/book"}  element={<MainBooksPage/>}/>
                    <Route path={"/"} element={<MainMoviesPage/>}/>
                    <Route path={"/movie/:id"} element={<MoviePage/>}/>
                    <Route path={"/movie/search/:movieName"} element={<MainMoviesPage/>}/>
                    <Route path={"/book/search/:bookName"} element={<MainBooksPage/>}/>
                    <Route path={"/book/:id"} element={<BookPage/>}/>
                    <Route path={"/not-found"} element={<NotFoundPage/>}/>
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
                        <Route path={"delete"} element={<DeleteMoviePage/>}/>
                    </Route>
                    <Route path={"book"}>
                        <Route path={"add"} element={<AddBookPage/>}/>
                        <Route path={"delete"} element={<DeleteBookPage/>}/>
                    </Route>
                    <Route path={"admin"}>
                        <Route path={"add"} element={<MakeAdminPage/>}/>
                        <Route path={"remove"} element={<RemoveAdminPage/>}/>
                    </Route>
                </Route>
            </Routes>
        </BrowserRouter>
    </div>
  );
}

export default App;
