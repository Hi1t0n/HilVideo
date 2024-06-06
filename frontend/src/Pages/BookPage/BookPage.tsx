import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { apiUrl, url } from "../../utils/constants";
import axios, { AxiosError } from "axios";
import {useSelector} from "react-redux";
import './BookPage.css';
import { Book } from "../../types/BooksTypes";
import {toast} from "react-toastify";
import StarIcon from "@mui/icons-material/Star";
import StarBorderIcon from "@mui/icons-material/StarBorder";

function BookPage(){
    const [bookData, setBookData] = useState<Book | null>(null);
    const [errorMessage , setErrorMessage] = useState<string | null>(null);
    const [releaseDate , setReleaseDate] = useState<Date | null>(null);
    const [file, setFile] = useState<File | null>(null);
    const [isFavorite, setIsFavorite] = useState<boolean>(false);

    // @ts-ignore
    const isLogin = useSelector((state: RootState) => state.login.isLogin);
    // @ts-ignore
    const userData = useSelector((state: UserDataState)=> state.userData);

    const {id} = useParams();

    const navigate = useNavigate();

    const favoriteData = {
        userId: userData.userId,
        bookId: id
    }

    const config = {
        withCredentials: true,
    };

    const checkFavorite = async () => {
        await axios.get<boolean>(`${apiUrl}books/check-favorite`, {
            withCredentials: true,
            params: {
                userId: userData.userId,
                bookId: id
            }
        }).then(response=> {
            // @ts-ignore
            setIsFavorite(response.data.isFavorite);
        }).catch(response => setIsFavorite(false));
    }

    const getBookById = async () => {
        try {
            const response = await axios.get<Book>(`${apiUrl}books/${id}`);

            if(response.status === 200){
                setBookData(response.data);
                setReleaseDate(new Date(response.data.releaseDate));
            }
        }
        catch (error){
            const axiosError = error as AxiosError;
            if(axiosError.response){
                if(axiosError.response.status === 404){
                    // @ts-ignore
                    const errorMessage: string = axiosError.response.data.error;
                    if(!errorMessage){
                        setErrorMessage("Книга не найдена");
                        navigate('/not-found', {state: {errorMessage: errorMessage}});
                    }
                    setErrorMessage(errorMessage);
                    navigate('/not-found', {state: {errorMessage: errorMessage}});
                }
            }
        }
    }

    const onClickStarIcon = async () => {
        await axios.delete(`${apiUrl}books/deletebookfromfavorites`, {
            withCredentials: true,
            data: favoriteData
        }).then(response => {
            if (response.status === 200) {
                setIsFavorite(false);
                toast.success("Книга удалена из избранного");
            }
        }).catch(response => {
            toast.error("Что-то пошло не так");
        });
    }

    const onClickStarBorderIcon = async () => {
        await axios.post(`${apiUrl}books/addbooktofavorites`, favoriteData, config)
            .then(response => {
                if (response.status === 200) {
                    setIsFavorite(true);
                    toast.success("Книга добавлена в избранное");
                }
            }).catch(response => {
                toast.error("Что-то пошло не так");
            })
    }

    useEffect(() => {
        getBookById();
        if(isLogin){
            checkFavorite();
        }
    },[]);



    return (
        <>
            {bookData && (
                <div className={"book-page-wrapper"}>
                    <div className={"book-name"}>
                        <h1>{bookData?.bookName} ({releaseDate?.getFullYear()})</h1>
                    </div>
                    <div className={"bookpage-content-wrapper"}>
                        <div>
                            <div className={"poster-wrapper"}>
                                <img className={"poster"} src={`${url}${bookData?.posterFilePath}`}
                                     alt={"Постер фильма"}/>
                            </div>
                            <div className={"button-wrapper"}>
                                {isLogin &&
                                    <div>
                                        {isFavorite ? <StarIcon className={'favorites-button'} onClick={onClickStarIcon}/> : <StarBorderIcon className={'favorites-button'} onClick={onClickStarBorderIcon} />}
                                    </div>
                                }
                                <div className={"readbook-wrapper"}>
                                    <button className={'readbook-button'}
                                            onClick={() => window.open(`https://localhost:7099/${bookData?.bookFilePath}`, '_blank')}>Читать
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div className={"about-wrapper"}>
                            <div className={"description-wrapper"}>
                                <h2>О книге</h2>
                                <div className={"book-description"}>
                                    <h3>Описание</h3>
                                    <p>{bookData?.bookDescription}</p>
                                </div>
                                <div className={"authors-wrapper"}>
                                <h3>Авторы</h3>
                                    <div className={"directors"}>
                                        {bookData?.authors.map(item => <p key={item}>{item}</p>)}
                                    </div>
                                </div>
                                <div className={"bookgenres-wrapper"}>
                                    <h3>Жанры</h3>
                                    <div>
                                        {bookData?.genres.map(item => <p key={item}>{item}</p>)}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </>
    );
}

export default BookPage;