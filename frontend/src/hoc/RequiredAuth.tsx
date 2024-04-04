import React from "react";
import {useSelector} from "react-redux";
import {RootState} from "../store/LoginSlice";
import {Navigate, useLocation} from "react-router-dom";


/* Для router с обязательной авторизацией */
// @ts-ignore
function RequiredAuth({children}){

    const location = useLocation();
    // @ts-ignore
    const isLogin = useSelector((state : RootState) => state.login.isLogin);

    if(!isLogin){
        return <Navigate to={'/login'} state={{from: location}}/>
    }

    return children;
}

export default RequiredAuth;