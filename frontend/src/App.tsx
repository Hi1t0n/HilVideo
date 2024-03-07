import React from 'react';
import MainPage from "./Components/MainPage";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import Events from "./Components/Test/Events";

function App() {
  return (
    <div>
        <BrowserRouter>
            <Routes>
                <Route path={"/"} element={<MainPage/>}/>
                <Route path={"/login"} element={<Events/>}/>
                <Route path={"/registration"}/>
            </Routes>
        </BrowserRouter>
    </div>
  );
}

export default App;
