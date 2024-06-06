import { useState } from "react";
import './MakeAdminPage.css';
import axios from "axios";
import {apiUrl} from "../../utils/constants";
import {toast, ToastContainer} from "react-toastify";

function MakeAdminPage() {
    const [login, setLogin] = useState<string>('');

    const onClickButton = () => {
        if(!login){
            toast.error("Введите логин");
        }

        axios.put(`${apiUrl}users/admin/add/${login}`, {}, {
            withCredentials: true
        }).then((response) => {
            if (response.data.status === 200) {
                toast.success(`Пользователь с логином: ${login} успешно назначен администратором`);
                window.location.reload();
            }
        }).catch(response => {
            toast.error("Что-то пошло не так");
            return;
        })
    }

    return(
        <div className="makeAdmin-container">
            <ToastContainer/>
            <h1>Назначение администратора</h1>
            <div className="makeAdmin-input-container">
                <input
                    className='makeAdmin-input'
                    value={login}
                    onChange={(e) => setLogin(e.target.value)}
                    required
                    placeholder={'Логин'}
                />
            </div>
            <div className="makeAdmin-button-container">
                <button className={'makeAdmin-button'} onClick={onClickButton}>Назначить</button>
            </div>
        </div>
    )
}

export default MakeAdminPage;