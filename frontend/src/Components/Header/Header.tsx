import logo from "../../logo.svg"
import "./Header.css"
import {useNavigate} from "react-router-dom";
import NavigationButton from "../NavigationButton/NavigationButton";
import {useDispatch, useSelector} from "react-redux";
import {RootState, setLoginState} from "../../store/LoginSlice";
import axios, {AxiosError} from "axios";

function Header(){
    const navigate = useNavigate();
    // @ts-ignore
    const isLogin = useSelector((state: RootState)=>state.login.isLogin);
    const dispatch = useDispatch();

    const handleLogout = async () => {
        try {
            const response = await axios('https://localhost:7099/api/auth/logout', {
                method: "GET",
                withCredentials: true,

            });
            if(response.status === 200){
                dispatch(setLoginState(false));
            }
        } catch (error){
            const axiosError = error as AxiosError;
            if(axiosError.response && axiosError.response.status === 401){
                navigate('/login', {replace: false});
            }
        }

    }

    return(
        <header className={"header"}>
            <nav className={"navigationMenu"}>
                <img src={logo} className={"logo"} alt={"Company logo"}/>
                <div className={"navigationButton"}>
                    {isLogin ? (
                        <>
                            <NavigationButton children={"Выйти"} onClick={handleLogout}/>
                        </>
                    ) : (
                        <>
                            <NavigationButton children={"Войти"} onClick={()=> navigate('login',{replace: false})} />
                            <NavigationButton children={"Зарегистрироваться"} onClick={()=> navigate('registration', {replace: false})}></NavigationButton>
                        </>
                    )}

                </div>
            </nav>
        </header>
    );
}

export default Header;