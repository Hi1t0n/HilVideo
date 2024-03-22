import "./Button.css"
interface Props{
    children: string
    onClick: () => void;
}
function Button({children, onClick}:Props){
    return(
        <button className={"button"} onClick={onClick}>{children}</button>
    );
}

export default Button;