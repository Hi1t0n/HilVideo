import {
    Accordion,
    AccordionDetails,
    AccordionSummary, Alert,
    Avatar, Button,
    Card,
    CardContent,
    CardHeader, Input,
    Typography
} from "@mui/material";
import color from "../../function/randomColor";
import {useSelector} from "react-redux";
import {UserDataState} from "../../store/UserDataSlice";
import ArrowDropDownIcon from '@mui/icons-material/ArrowDropDown';
import React, {useState} from "react";
import {ChangeUserPasswordRequest} from "../../Data/ChangeUserPasswordRequest";
import axios, {AxiosError} from "axios";
import {PASSWORD_REGEX} from "../../Data/REGEX";
import LogOutButton from "../../Components/Button/LogOutButton/LogOutButton";
import AdminPanelButton from "../../Components/AdminPanel/AdminPanelButton/AdminPanelButton";
import {useNavigate} from "react-router-dom";
import './Profile.css'

type Severity = 'error' | 'warning' | 'info' | 'success';

function Profile() {
    const [currentPassword, setCurrentPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [showAlert, setShowAlert] = useState(false);
    const [alertMessage, setAlertMessage] = useState('');
    const [severity , setSeverity] = useState<Severity>('success');

    // @ts-ignore
    const userData = useSelector((state: UserDataState)=> state.userData);
    const possibleRoles = ['Owner', 'Admin'];
    const navigate = useNavigate();

    const handleCurrentPasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setCurrentPassword(e.target.value);
    }

    const handleNewPasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setNewPassword(e.target.value);
    }

    const handleCloseAlert = () => {
        setShowAlert(false);
    };


    /* Смена пароля пользователя */
    const handleChangePassword = async () => {

        if(currentPassword === newPassword){
            setSeverity('error');
            setAlertMessage('Текущий и новый пароль не могут быть одинаковыми!');
            setShowAlert(true);
            return;
        }

        if(!PASSWORD_REGEX.test(newPassword)){
            setSeverity('error');
            setAlertMessage('В пароле присутствуют запрещенные символы!');
            setShowAlert(true);
            return;
        }

        const data: ChangeUserPasswordRequest = {
            id: userData.id,
            currentPassword: currentPassword,
            newPassword: newPassword,
        };

        try {
            const response = await axios("https://localhost:7099/api/users/changepassword",{
                method: "PUT",
                data: data,
                withCredentials: true
            });
            if(response.status === 200){
                setSeverity('success');
                setAlertMessage('Пароль успешно изменен!');
                setShowAlert(true);
            }
        }
        catch (error){
            const axiosError = error as AxiosError;
            if (axiosError.response){
                setSeverity('error');
                // @ts-ignore
                setAlertMessage(axiosError.response.data.error);
                setShowAlert(true);
            }
        }
    }



    return(

        <>
            <Card sx={{ bgcolor: '#282c34', minHeight: '100vh', borderRadius: '0px' }}>
                <CardHeader avatar={<Avatar sx={{bgcolor: color, minHeight: '40px', minWidth:'40px'}}>{userData.login.toUpperCase().slice(0,2)}</Avatar>} title={`Имя пользователя: ${userData.login.toUpperCase()}`} titleTypographyProps={{sx: {color: '#D3D3D3'}}} />
                <CardContent>
                    <Accordion sx={{bgcolor: '#282c34'}}>
                        <AccordionSummary expandIcon={<ArrowDropDownIcon sx={{color: '#D3D3D3'}}/>}>
                            <Typography sx={{color: '#D3D3D3'}}>Смена пароля</Typography>
                        </AccordionSummary>
                        <AccordionDetails>
                            <div>
                                <Input sx={{marginTop: '5px'}} type={'password'} onChange={handleCurrentPasswordChange}
                                       placeholder={'Текущий пароль'}></Input>
                            </div>
                            <div>
                                <Input sx={{marginTop: '5px'}} type={'password'} onChange={handleNewPasswordChange}
                                       placeholder={'Новый пароль'}></Input>
                            </div>
                            <div>
                                <Button sx={{marginTop: '5px'}} variant={'outlined'} onClick={handleChangePassword}>Сменить
                                    пароль</Button>
                            </div>
                        </AccordionDetails>
                    </Accordion>
                    <div className={'button-wrapper'}>
                        <div>
                            {possibleRoles.includes(userData.roleName) && (
                                <AdminPanelButton text={"Админ панель"}/>
                            )}
                        </div>
                        <div>
                            <Button sx={{color: ''}} variant={'outlined'}
                                    onClick={() => navigate('/favorites/movies', {replace: false})}>{"Избранные фильмы"}</Button>
                        </div>
                        <div>
                            <Button sx={{color: ''}} variant={'outlined'}
                                    onClick={() => navigate('/favorites/books', {replace: false})}>{"Избранные книги"}</Button>
                        </div>
                    </div>
                    <div>
                        <LogOutButton text={'Выход'}/>
                    </div>

                </CardContent>
            </Card>

            {showAlert && (
                <Alert severity={severity} onClose={handleCloseAlert}
                       style={{position: 'absolute', bottom: '150px', left: '10px'}}>
                    {alertMessage}
                </Alert>
            )}
        </>
    )
}

export default Profile;