interface Props{
    children : string;
    onClick : () => void;
}

function ButtonFormAdminPanel({children, onClick}: Props){
    return(
        <button onClick={onClick}>{children}</button>
    )
}

export default ButtonFormAdminPanel;