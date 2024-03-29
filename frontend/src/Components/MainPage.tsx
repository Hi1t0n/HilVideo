import Header from "./Header/Header";
import Footer from "./Footer/Footer";
import {Outlet} from "react-router-dom";

function MainPage(){
    return(
        <>
            <div>
                <Header></Header>
            </div>
            <div>
                <Outlet/>
            </div>
            <div>
                <Footer></Footer>
            </div>
        </>
    );
}

export default MainPage;