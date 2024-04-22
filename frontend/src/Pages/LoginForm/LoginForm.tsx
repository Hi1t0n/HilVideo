import TextInput from "../../Components/Input/TextInput/TextInput";
import React, {useState} from "react";
import Button from "../../Components/Button/Button";
import axios, {AxiosError} from "axios";
import {useLocation, useNavigate} from "react-router-dom";
import {useDispatch} from "react-redux";
import {setLoginState} from "../../store/LoginSlice";
import {setUserData} from "../../store/UserDataSlice";
function LoginForm(){
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");

    const navigate = useNavigate();
    const location = useLocation();
    const fromPage = location.state?.from?.pathname || '/';
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
                dispatch(setUserData(response.data));
                console.log(response.data.roleName);
                dispatch(setLoginState(true));
                navigate(fromPage, {replace: true});
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