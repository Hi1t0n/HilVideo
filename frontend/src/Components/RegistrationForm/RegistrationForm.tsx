import './RegistrationForm.css'
import React, {useState} from "react";
import TextInput from "../Input/TextInput/TextInput";
import Button from "../Button/Button";
import axios, {AxiosError} from "axios";
import {useNavigate} from "react-router-dom";

function RegistrationForm(){
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [email, setEmail] = useState("");
    const [phoneNumber, setPhoneNumber] = useState("");
    const [errorMessage, setErrorMessage] = useState("");

    const navigate = useNavigate();

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

    const handleErrorMessage = (e: string) => {
        setErrorMessage(e)
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

        /* Проверка логина */
      if(!LOGIN_REGEX.test(login)){
          handleErrorMessage("Логин должен состоять букв aA-zZ, цифр 0-9 и символов _-");
          return;
      }

        /* Проверка пароля */
      if(!PASSWORD_REGEX.test(password)){
          handleErrorMessage("Введен не верный пароль");
          return;
      }
        /* Проверка на соответствие паролей */
      if(password !== confirmPassword){
          handleErrorMessage("Пароли не совпадают");
          return;
      }
        /* Проверка номера телефона */
      if(!PHONENUMBER_REGEX.test(phoneNumber)){
          handleErrorMessage("Введен не верный номер телефона");
          return;
      }
        /* Проверка электронной почты */
      if(!EMAIL_REGEX.test(email)){
          handleErrorMessage("введен не верный email");
          return;
      }

      const userData = {
          Login: login,
          Password: password,
          Email: email,
          PhoneNumber: phoneNumber
        };

      try{
          const response = await axios.post('https://localhost:7099/api/auth/register', userData)
          if(response.status === 200){
              navigate('/login', {replace: true});
          }
      }
      catch (error){
          const axiosError = error as AxiosError;
          if(axiosError.response){
              // @ts-ignore
              const errorMessage: string = axiosError.response.data.error;
              handleErrorMessage(errorMessage);
          }
      }


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
                            <p className={"errorMessage"}>{errorMessage}</p>
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