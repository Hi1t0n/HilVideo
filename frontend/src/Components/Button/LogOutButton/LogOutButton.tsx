import LogoutIcon from "@mui/icons-material/Logout";
import {Button} from "@mui/material";
import React from "react";
import axios, {AxiosError} from "axios";
import {useNavigate} from "react-router-dom";
import {useDispatch, useSelector} from "react-redux";
import {RootState, setLoginState} from "../../../store/LoginSlice";
import {UserDataState} from "../../../store/UserDataSlice";
import {persistor} from "../../../store/store";

interface Props{
    text: string
}



function LogOutButton({ text }: Props){

    const navigate = useNavigate();

    const dispatch = useDispatch();

    /* Выход из профиля */
    const handleLogout = async () => {
        try {
            const response = await axios('https://localhost:7099/api/auth/logout', {
                method: "GET",
                withCredentials: true,
            });
            if(response.status === 200){
                dispatch(setLoginState(false));
                await persistor.flush();
                navigate('/');
            }
        } catch (error){
            const axiosError = error as AxiosError;
            if(axiosError.response && axiosError.response.status === 401){
                navigate('/login', {replace: false});
            }
        }

    }

    return(
        <Button sx={{color: '#800000', bgcolor: 'none', borderColor: '#800000', marginTop: '10px',
            ":hover": {
                borderColor: '#DC143C',
            }
        }} variant={"outlined"} endIcon={<LogoutIcon/>} onClick={handleLogout}>{text}</Button>
    );
}

export default LogOutButton