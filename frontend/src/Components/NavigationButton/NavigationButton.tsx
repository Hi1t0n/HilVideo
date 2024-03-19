import "./NavigationButton.css"
interface Props{
    children : string;
    onClick: ()=> void;
}
function NavigationButton({children, onClick}: Props){
    return(
        <button className={"navigationButton"} onClick={onClick}>{children}</button>
    );
}

export default NavigationButton;