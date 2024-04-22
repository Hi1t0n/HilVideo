import React from "react";
import './Sidebar.css';
import {SidebarData} from "./SidebarData";
import {useNavigate} from "react-router-dom";

function Sidebar(){

    const navigate = useNavigate();

    return(
        <div className={"Sidebar"}>
            <ul className={"SidebarList"}>
                {SidebarData.map((item, key) => {
                    return (
                        <li
                            key={key}
                            onClick={()=> navigate(item.link, {replace: true})}
                            className={"row"}
                        >
                            <div id={"icon"}>
                                {item.icon}
                            </div>
                            <div id={"title"}>
                                {item.title}
                            </div>
                        </li>
                    )
                })}
            </ul>
        </div>
    )
}

export default Sidebar;