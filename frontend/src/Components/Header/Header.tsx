import logo from "../../logo.svg"
import "./Header.css"
import { useNavigate, useLocation } from "react-router-dom";
import NavigationButton from "../NavigationButton/NavigationButton";
import {useDispatch, useSelector} from "react-redux";
import {RootState, setLoginState} from "../../store/LoginSlice";
import axios, {AxiosError} from "axios";
import {Avatar, Stack} from "@mui/material";
import Alert from '@mui/material/Alert';
import {UserDataState} from "../../store/UserDataSlice";
import color from "../../function/randomColor";
import {persistor} from "../../store/store";
import SearchBar from "../SearchBar/SearchBar";
import React, {useState} from "react";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import {apiUrl} from "../../utils/constants";
import moviePage from "../../Pages/MoviePage/MoviePage";

type TypeMsg = 'error' | 'warning' | 'info' | 'success';

function Header(){
    const navigate = useNavigate();
    const location = useLocation();
    // @ts-ignore
    const isLogin = useSelector((state: RootState)=>state.login.isLogin);
    // @ts-ignore
    const userData = useSelector((state: UserDataState)=> state.userData);
    const [searchText, setSearchText] = useState<string>("");

    const onChangeSearchText = (e: React.ChangeEvent<HTMLInputElement>) => {
        setSearchText(e.target.value);
    }

    const onClickSearch = async () => {
        if (searchText.length === 0) {
            notify('Введите параметры поиска', 'error');
            return;
        }

        if (location.pathname.startsWith('/book')) {
            navigate(`/book/search/${searchText}`, { replace: false });
        } else {
            navigate(`/movie/search/${searchText}`, { replace: false });
        }
    };

    const onClickLogo = () => {
        navigate(('/'), {replace: false});
    }

    const notify = (msg: string, typeMsg: TypeMsg) => {
        if(typeMsg === 'error'){
            toast.error(msg);
            return
        }
        if(typeMsg === 'warning'){
            toast.warning(msg);
            return;
        }
        if(typeMsg === 'info'){
            toast.info(msg);
            return;
        }
        if(typeMsg === 'success'){
            toast.success(msg);
            return;
        }
    };

    return(
        <header className={"header"}>
            <nav className={"navigationMenu"}>
                <div>
                    <img src={logo} className={"logo"} alt={"Company logo"} onClick={onClickLogo} />
                </div>
                <div>
                    <SearchBar onChange={onChangeSearchText} onClick={onClickSearch} value={searchText}/>
                    <ToastContainer/>
                </div>
                <div className={"navigationButton"}>
                    <NavigationButton children={"Книги"} onClick={()=> navigate('/book', {replace: false})} />
                    <NavigationButton children={"Фильмы"} onClick={()=> navigate('/', {replace: false})} />
                {isLogin ? (
                        <>
                                <Avatar className={"profile"} style={{backgroundColor: color}} onClick={()=> navigate('profile', {replace: false})}>{userData.login.toUpperCase().slice(0,2)}</Avatar>
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