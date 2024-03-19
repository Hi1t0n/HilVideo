import logo from "../../logo.svg"
import "./Header.css"
import {useNavigate} from "react-router-dom";
import NavigationButton from "../NavigationButton/NavigationButton";

function Header(){
    const navigate = useNavigate();
    return(
        <header className={"header"}>
            <nav className={"navigationMenu"}>
                <img src={logo} className={"logo"} alt={"Company logo"}/>
                <NavigationButton children={"Войти"} onClick={()=> navigate('login',{replace: false})} />
                <NavigationButton children={"Зарегистрироваться"} onClick={()=> navigate('registration', {replace: false})}></NavigationButton>
            </nav>
        </header>
    );
}

export default Header;