import React from "react";
import "./ImageButton.css"

interface Props {
    src: string;
    url: string;
}
function ImageButton({url, src} : Props){

    const handleClick = () => {
        window.open(url)
    }

    return(
        <img className={"image-button"} src={src} alt={"Image"} onClick={handleClick}></img>
    )
}

export default ImageButton;