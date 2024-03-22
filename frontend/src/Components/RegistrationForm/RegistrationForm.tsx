import './RegistrationForm.css'
import React, {useState} from "react";
import TextInput from "../Input/TextInput/TextInput";
import Button from "../Button/Button";

function RegistrationForm(){
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [email, setEmail] = useState("");
    const [phoneNumber, setPhoneNumber] = useState("");

    const handleLoginChange = (e : React.ChangeEvent<HTMLInputElement>) => {
        setLogin(e.target.value);
    }

    const handlePasswordChange = (e : React.ChangeEvent<HTMLInputElement>) => {
        setPassword(e.target.value);
    }

    const handleConfirmPasswordChange = (e : React.ChangeEvent<HTMLInputElement>) => {
        setConfirmPassword(e.target.value);
    }

    const handleEmailChange = (e : React.ChangeEvent<HTMLInputElement>) => {
        setEmail(e.target.value);
    }

    /* Обновление phoneNumber и проверка на то что введена именно цифра */
    const handlePhoneNumberChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        let inputValue = e.target.value;
        let lastChar = inputValue[inputValue.length - 1];

        /* Проверяем, является ли последний символ цифрой */
        if (lastChar < '0' || lastChar > '9') {
            /* Удаляем последний символ, если он не является цифрой */
            setPhoneNumber(inputValue.slice(0, -1));
        } else {
            setPhoneNumber(inputValue);
        }
    }

    /* Действие при нажатии на кнопку регистрации */
    const handleRegistration = async () => {
      
    }

    /* Регулярные выражения для проверок */
    const EMAIL_REGEX = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9-]+.+.[a-zA-Z]{2,4}$/i;
    const LOGIN_REGEX = /^[a-zA-Z0-9_-]{3,30}$/;
    const PASSWORD_REGEX = /^[a-zA-Z0-9@#$&*]{6,30}$/;
    const PHONENUMBER_REGEX = /^[0-9]{11}$/;

    return (
        <>
            <html className={"body"}>
                <body>
                    <div className={"container"}>
                        <div className={"items-div"}>
                            <TextInput id={"login-input"} type={"text"} placeholder={"Логин"} value={login} required={true}
                                       onChange={handleLoginChange} minLength={3} maxLength={30}
                                       pattern={LOGIN_REGEX.toString()}/>
                        </div>
                        <div className={"items-div"}>
                            <TextInput id={"password-input"} type={"password"} placeholder={"Пароль"} value={password}
                                       required={true} onChange={handlePasswordChange} minLength={6} maxLength={30}
                                       pattern={PASSWORD_REGEX.toString()}/>
                        </div>
                        <div className={"items-div"}>
                            <TextInput id={"confirm-password-input"} type={"password"} placeholder={"Повторите пароль"}
                                       value={confirmPassword} required={true} onChange={handleConfirmPasswordChange}
                                       minLength={6} maxLength={30} pattern={PASSWORD_REGEX.toString()}/>
                        </div>
                        <div className={"items-div"}>
                            <TextInput id={"email-input"} type={"email"} placeholder={"Email"} value={email} required={true}
                                       onChange={handleEmailChange} pattern={EMAIL_REGEX.toString()}/>
                        </div>
                        <div className={"items-div"}>
                            <TextInput id={"phone-number-input"} type={"tel"} placeholder={"Номер телефона"} value={phoneNumber}
                                       required={true} minLength={11} maxLength={11} onChange={handlePhoneNumberChange}
                                       pattern={PHONENUMBER_REGEX.toString()}/>
                        </div>
                        <div>
                            <Button onClick={handleRegistration} children={'Зарегистрироваться'}></Button>
                        </div>
                    </div>
                </body>
            </html>
        </>
    );
}

export default RegistrationForm;