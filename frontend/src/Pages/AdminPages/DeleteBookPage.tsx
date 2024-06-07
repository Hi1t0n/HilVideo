import SelectSingle from "../../Components/Select/SelectSingle";
import {useEffect, useState} from "react";
import axios from "axios";
import {apiUrl} from "../../utils/constants";
import {MovieDataForDelete} from "../../types/MovieData";
import {toast, ToastContainer} from "react-toastify";
import {SelectChangeEvent} from "@mui/material/Select";
import './DeleteMoviePage.css';
import {BookDataForDelete} from "../../types/BooksTypes";
import './DeleteBookPage.css';

function DeleteBookPage (){

    const [books, setBooks] = useState<BookDataForDelete[]>([]);
    const [selectedBook, setSelectedBook] = useState<string>("");
    const [confirmMsg, setConfirmMsg] = useState("");

    const getBookData = async () => {
        axios.get(`${apiUrl}books/get-book-id-with-name`, {
            withCredentials: true
        }).then(result => {
            if(result.status === 200){
                setBooks(result.data);
            }
        }).catch(err=>{
            toast.error("Что-то пошло не так");
        })
    }

    const handleChangeBook = (event: SelectChangeEvent<string>) => {
        setSelectedBook(event.target.value);
    }

    const onClickDeleteButton = async () => {
        if(!setSelectedBook){
            toast.error('Выберите фильм');
            return;
        }
        if(confirmMsg !== "УДАЛИТЬ"){
            toast.error('Подтверждающее сообщение должно быть "УДАЛИТЬ"');
            return;
        }

        axios.delete(`${apiUrl}books/${selectedBook}`, {
            withCredentials: true
        }).then(response => {
            if(response.status === 200){
                toast.success("Книга успешно удалена");
                window.location.reload();
            }
        }).catch(err=>{
            toast.error("Что-то пошло не так");
        })
    }

    useEffect(() => {
        getBookData();
    }, []);

    return(
        <div className={"delete-book-wrapper"}>
            <ToastContainer/>
            <div className={"select-wrapper"}>
                <SelectSingle data={books} handleChange={handleChangeBook} multiple={false} placeholder={"Книга"} selectValue={selectedBook}/>
            </div>
            <div className={"input-wrapper"} >
                <input className={'delete-book-input'} placeholder={'Введите "УДАЛИТЬ"'} required value={confirmMsg} onChange={(e) => setConfirmMsg(e.target.value)}/>
            </div>
            <div className={"button-wrapper"}>
                <button className={'delete-book-button'} onClick={onClickDeleteButton}>Удалить</button>
            </div>
        </div>
    );
}

export default DeleteBookPage;