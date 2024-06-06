import React, {useRef, useState} from "react";
import SelectCheckBox from "../../Components/Select/SelectCheckBox";
import {SelectChangeEvent} from "@mui/material/Select";
import {useGetGenresQuery} from "../../store/Api/genresApi";
import {useGetAuthorsQuery} from "../../store/Api/authorApi";
import './AddBookPage.css'
import {toast, ToastContainer} from "react-toastify";
import {retry} from "@reduxjs/toolkit/query";
import axios, {AxiosError} from "axios";
import {apiUrl} from "../../utils/constants";

type TypeMsg = 'error' | 'warning' | 'info' | 'success';

function AddBookPage () {
    const [selectedBookFile , setSelectedBookFile] = useState<File | null>(null);
    const [selectedPosterFile, setSelectedPosterFile] = useState<File | null>(null);
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [selectedGenres,setSelectedGenres] = useState<string[]>([]);
    const [selectedAuthors, setSelectedAuthors] = useState<string[]>([]);
    const [releaseDate, setReleaseDate] = useState(new Date().toISOString());

    const {data: genresData= [], refetch: genresRefetch} = useGetGenresQuery(undefined, {pollingInterval: 600000});
    const {data: authorsData = [], refetch: authorsRefetch } = useGetAuthorsQuery(undefined, {pollingInterval: 600000});

    const onChangeTitle = async (e : React.ChangeEvent<HTMLInputElement>) => {
        setTitle(e.target.value);
    }

    const onChangeDescription = async (e : React.ChangeEvent<HTMLTextAreaElement>) => {
        setDescription(e.target.value);
    }

    const handleChangeBookFile = async (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files && e.target.files[0];
        if(file){
            setSelectedBookFile(file);
        }

    }
    const handleChangeReleaseDate = async (e: React.ChangeEvent<HTMLInputElement>) => {
        setReleaseDate(e.target.value);
    }

    const handleChangePosterFile = async (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files && e.target.files[0];
        if(file){
            setSelectedPosterFile(file);
        }
    }

    const handleChangeGenresData = async (event: SelectChangeEvent<string[]>) => {
        const {
            target: { value },
        } = event;
        setSelectedGenres(value as string[]);
    };

    const handleChangeAuthorsData = async (event: SelectChangeEvent<string[]>) => {
        const {
            target: {value},
        } = event;
        setSelectedAuthors(value as string[]);
    }

    const handlePickPoster = async () =>{
        PosterFileRef.current?.click();
    }

    const handlePickBook = async () => {
        BookFileRef.current?.click();
    }

    const PosterFileRef = useRef<HTMLInputElement | null>(null);
    const BookFileRef = useRef<HTMLInputElement | null>(null);

    const handleAddBook = () => {
        if(!title){
            toast.error('Введите название книги');
            return;
        }
        if(!description){
            toast.error('Введите описание книги');
            return;
        }
        if(!selectedBookFile){
            toast.error('Выберите файл книги');
            return;
        }
        if(!selectedPosterFile){
            toast.error('Выберите файл для постера');
            return;
        }
        if(selectedGenres.length === 0){
            toast.error('Выберите как минимум 1 жанр');
            return;
        }
        if(selectedAuthors.length === 0){
            toast.error('Выберите как минимум 1-ого автора');
            return;
        }
        if(!releaseDate){
            toast.error('Укажите дату выхода');
            return;
        }

        const formData = new FormData();
        formData.append('BookName', title);
        formData.append('BookDescription', description);
        formData.append('ReleaseDate', releaseDate);
        formData.append('Authors', JSON.stringify(selectedAuthors));
        formData.append('Genres', JSON.stringify(selectedGenres));
        formData.append('Poster', selectedPosterFile);
        formData.append('Book', selectedBookFile);

        axios.post(`${apiUrl}books/`, formData, {
            withCredentials: true,
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        }).then(response => {
            if (response.status === 201) {
                toast.success("Книга успешно добавлена");
                window.location.reload();
            }
        }).catch(error => {
            const axiosError = error as AxiosError;
            // @ts-ignore
            if (axiosError.response?.data?.error) {
                // @ts-ignore
                toast.error(axiosError.response.data.error);
            } else {
                toast.error("Произошла ошибка при добавлении книги");
            }
        });


    }

    return (
      <div className={"addbook-wrapper"}>
          <div>
              <h1>Добавление книги</h1>
          </div>
          <ToastContainer/>
          <div className={"AddBookPage-content"}>
              <div className={"input-title"}>
                  <input type={"text"} onChange={onChangeTitle} className={'title-input'} value={title} autoComplete={"off"}
                         placeholder={"Название книги*"} required={true}/>
              </div>
              <div className={'textarea-description-wrapper'}>
                  <textarea className={"textarea-description"} onChange={onChangeDescription} value={description} autoComplete={"off"}
                            placeholder={"Описание*"} required={true}/>
              </div>
              <div className={'select-file-wrapper'}>
                  <div>
                      <button className={'add-book-button'} onClick={handlePickPoster}>Выберите постер</button>
                      <input className={'file-input'} ref={PosterFileRef} type={'file'} name={"poster"}
                             accept="image/png, image/jpeg"
                             required={true} onChange={handleChangePosterFile}/>
                  </div>
                  <div>
                      <button onClick={handlePickBook} className={'add-book-button'} >Выберите книгу</button>
                      <input className={'file-input'} ref={BookFileRef} type={'file'} name={"movie"}
                             accept="application/pdf"
                             required={true} onChange={handleChangeBookFile}/>
                  </div>
              </div>
              <div>
                  <input className={'data-input'} value={releaseDate} type={'date'} onChange={handleChangeReleaseDate}
                         required/>
              </div>
              <div className={"select-group"}>
                  <SelectCheckBox data={genresData} handleChange={handleChangeGenresData} multiple={true}
                                  placeholder={"Жанры"} selectedData={selectedGenres} required={true}/>
                  <SelectCheckBox data={authorsData} handleChange={handleChangeAuthorsData} multiple={true}
                                  required={true} placeholder={"Авторы"} selectedData={selectedAuthors}/>
              </div>
              <div>
                  <button className={"add-book-button"} onClick={handleAddBook}>Добавить книгу</button>
              </div>
          </div>

      </div>
    );
}

export default AddBookPage;