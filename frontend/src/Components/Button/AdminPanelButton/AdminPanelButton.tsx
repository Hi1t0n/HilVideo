import {Button} from "@mui/material";
import {useNavigate} from "react-router-dom";
import {useSelector} from "react-redux";
import {RootState} from "../../../store/LoginSlice";
import {UserDataState} from "../../../store/UserDataSlice";

interface Props{
    text: string
}

function AdminPanelButton({text}: Props){
    const navigate = useNavigate();

    const onClick = () => {
        navigate('/adminpanel', {replace: true});
    }

    return(
      <Button sx={{color: ''}} variant={'outlined'} onClick={onClick}>{text}</Button>
    );
}

export default AdminPanelButton;