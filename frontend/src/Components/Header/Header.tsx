import logo from "../../logo.svg"
import "./Header.css"
import {useNavigate} from "react-router-dom";
import NavigationButton from "../NavigationButton/NavigationButton";
import {useDispatch, useSelector} from "react-redux";
import {RootState, setLoginState} from "../../store/LoginSlice";
import axios, {AxiosError} from "axios";
import {Avatar, Stack} from "@mui/material";
import {UserDataState} from "../../store/UserDataSlice";
import color from "../../function/randomColor";
import {persistor} from "../../store/store";

function Header(){
    const navigate = useNavigate();
    // @ts-ignore
    const isLogin = useSelector((state: RootState)=>state.login.isLogin);
    // @ts-ignore
    const userData = useSelector((state: UserDataState)=> state.userData);

    return(
        <header className={"header"}>
            <nav className={"navigationMenu"}>
                <img src={logo} className={"logo"} alt={"Company logo"}/>
                <div className={"navigationButton"}>
                    {isLogin ? (
                        <>
                            <Stack direction={"row"}>
                                <Avatar style={{backgroundColor: color}} onClick={()=> navigate('profile', {replace: false})}>{userData.login.toUpperCase().slice(0,2)}</Avatar>
                            </Stack>
                        </>
                    ) : (
                        <>
                            <NavigationButton children={"Войти"} onClick={()=> navigate('/login',{replace: false})} />
                            <NavigationButton children={"Зарегистрироваться"} onClick={()=> navigate('/registration', {replace: false})}></NavigationButton>
                        </>
                    )}

                </div>
            </nav>
        </header>
    );
}

export default Header;