import logo from  "../logo.svg"
import "./Header.css"
import {useNavigate} from "react-router-dom";
function Header(){
    const navigate = useNavigate();
    return(
        <header className={"header"}>
            <nav className={"navigationMenu"}>
                <img src={logo} className={"logo"} alt={"Company logo"}/>
                <button className={"navigationButton"} onClick={()=>navigate('login', {replace: false})} >Войти</button>
                <button className={"navigationButton"} onClick={()=>navigate('registration',{replace: false})}>Зарегистрироваться</button>
            </nav>
        </header>
    );
}

export default Header;