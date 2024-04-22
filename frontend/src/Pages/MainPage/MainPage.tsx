import Header from "../../Components/Header/Header";
import Footer from "../../Components/Footer/Footer";
import {Outlet} from "react-router-dom";

function MainPage(){
    return(
        <body>
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
        </body>
    );
}

export default MainPage;