import Sidebar from "../../Components/AdminPanel/Sidebar/Sidebar";
import {Outlet} from "react-router-dom";

function AdminPanel(){
    return(
        <>
            <Sidebar/>
            <Outlet/>
        </>
    )
}

export default AdminPanel;