import Header from "../../Components/Header/Header";
import Footer from "../../Components/Footer/Footer";
import {Outlet} from "react-router-dom";
import '../MainPage/MainPage.css'

function MainPage(){
    return(
        <div className="MainPage-wrapper">
            <div>
                <Header></Header>
            </div>
            <div>
                <Outlet/>
            </div>
            <div>
                <Footer></Footer>
            </div>
        </div>
    );
}

export default MainPage;