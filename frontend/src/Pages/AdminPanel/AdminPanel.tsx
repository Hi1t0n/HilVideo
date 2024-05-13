import Sidebar from "../../Components/AdminPanel/Sidebar/Sidebar";
import {Outlet} from "react-router-dom";
import './AdminPanel.css'

function AdminPanel(){
    return(
        <div className={"wrapper-admin"}>
            <div className={"block"}>
                <Sidebar/>
            </div>
            <div className={"block"}>
                <Outlet/>
            </div>
        </div>
    )
}

export default AdminPanel;