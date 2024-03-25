import TextInput from "../Input/TextInput/TextInput";
import React, {useState} from "react";
import Button from "../Button/Button";
import axios, {AxiosError} from "axios";
import {useNavigate} from "react-router-dom";
import {useDispatch} from "react-redux";
import {setLoginState} from "../../store/LoginSlice";
import {saveUserData} from "../../store/UserDataSlice";
function LoginForm(){
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");

    const navigate = useNavigate();
    const dispatch = useDispatch();

    const handleLoginChange = (e : React.ChangeEvent<HTMLInputElement>) => {
        setLogin(e.target.value);
    }

    const handlePasswordChange = (e : React.ChangeEvent<HTMLInputElement>) => {
        setPassword(e.target.value);
    }

    const handleLogin = async () => {
        const userLoginRequest = {
            Login: login,
            Password: password
        };

        try{
            const response = await axios('https://localhost:7099/api/auth/login', {
                method: "POST",
                data: userLoginRequest,
                withCredentials: true
            });
            if(response.status === 200){
                dispatch(saveUserData(response.data));
                console.log(response.data);
                dispatch(setLoginState(true));
                navigate('/', {replace: true});
            }
        } catch (error){
            const axiosError = error as AxiosError;
            if(axiosError.response){
                // @ts-ignore
                const errorMessage: string = axiosError.response.data.error;
                setErrorMessage(errorMessage);
            }
        }
    }

    return(
        <div className={"container"}>
            <div className={"items-div"}>
                <TextInput id={"login-input"} type={"text"} placeholder={"Логин"} value={login} required={true} onChange={handleLoginChange}/>
            </div>
            <div className={"items-div"}>
                <TextInput id={"password-input"} type={"password"} placeholder={"Пароль"} value={password} required={true} onChange={handlePasswordChange}/>
            </div>
            <div>
                <p className={"error-message"}>{errorMessage}</p>
                <Button onClick={handleLogin} children={"Войти"}></Button>
            </div>
        </div>
    );
}

export default LoginForm;