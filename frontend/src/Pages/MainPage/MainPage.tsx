import Header from "../../Components/Header/Header";
import Footer from "../../Components/Footer/Footer";
import {Outlet} from "react-router-dom";

function MainPage(){
    return(
        <div>
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