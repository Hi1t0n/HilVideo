import { useState } from "react";
import './RemoveAdminPage.css';
import axios from "axios";
import {apiUrl} from "../../utils/constants";
import {toast, ToastContainer} from "react-toastify";

function RemoveAdminPage() {
    const [login, setLogin] = useState<string>('');

    const onClickButton = () => {
        if(!login){
            toast.error("Введите логин");
        }

        axios.put(`${apiUrl}users/admin/remove/${login}`, {}, {
            withCredentials: true
        }).then((response) => {
            if (response.data.status === 200) {
                toast.success("Пользователь успешно снят с поста администратора");
            }
        }).catch(response => {
            toast.error("Что-то пошло не так");
            return;
        })
    }

    return(
        <div className="removeAdmin-container">
            <ToastContainer/>
            <h1>Снятие администратора</h1>
            <div className="removeAdmin-input-container">
                <input
                    className='removeAdmin-input'
                    value={login}
                    onChange={(e) => setLogin(e.target.value)}
                    required
                    placeholder={'Логин'}
                />
            </div>
            <div className="removeAdmin-button-container">
                <button className={'removeAdmin-button'}  onClick={onClickButton}>Снять</button>
            </div>
        </div>
    )
}

export default RemoveAdminPage;