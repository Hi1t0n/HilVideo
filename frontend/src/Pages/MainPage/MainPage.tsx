import Header from "../../Components/Header/Header";
import Footer from "../../Components/Footer/Footer";
import {Outlet} from "react-router-dom";
import '../MainPage/MainPage.css'

function MainPage(){
    return(
        <div className="MainPage-wrapper">
            <Header />
            <div className="MainPage-content">
                <Outlet />
            </div>
            <Footer />
        </div>
    );
}

export default MainPage;