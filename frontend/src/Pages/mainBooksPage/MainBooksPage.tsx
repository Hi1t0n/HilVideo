import {useCallback, useEffect, useState} from "react";
import axios, {AxiosError} from "axios";
import {apiUrl} from "../../utils/constants";
import "../mainMoviesPage/MainMoviesPage.css"
import {useLocation, useNavigate, useParams} from "react-router-dom";
import {Books} from "../../types/BooksTypes";
import BookCard from "../../Components/Card/BookCard/BookCard";
import {Movies} from "../../types/MovieData";
import {toast} from "react-toastify";
import {useSelector} from "react-redux";

function MainBooksPage() {
    const { bookName } = useParams();
    const navigate = useNavigate();
    const location = useLocation();

    const [books, setBooks] = useState<Books[] | null>(null);


    // @ts-ignore
    const userData = useSelector((state) => state.userData);

    const getBooks = useCallback(async () => {
        try {
            const response = await axios.get<Books[]>(`${apiUrl}books/`);
            if (response.status === 200) {
                setBooks(response.data);
            }
        } catch (error) {
            console.error(error);
        }
    }, []);

    const getSearchBooks = useCallback(async () => {
        try {
            const response = await axios.get<Books[]>(`${apiUrl}books/search/${bookName}`);
            if (response.status === 200) {
                setBooks(response.data);
            }
        } catch (error) {
            if (axios.isAxiosError(error) && error.response && error.response.status === 404) {
                console.log(error.response.data.error);
                navigate('/not-found', { state: { errorMessage: error.response.data.error }, replace: false });
            }
        }
    }, [bookName, navigate]);

    const getFavoritesBooks = useCallback(async () => {
        try {
            const response = await axios.get<Books[]>(`${apiUrl}books/getfavoritebook/${userData.userId}`, {
                withCredentials: true
            });

            if (response.status === 200) {
                setBooks(response.data);
                return;
            }
        }
        catch (error){
            toast.error("Что-то пошло не так");
        }
    }, [])

    useEffect(() => {
        if (location.pathname === "/favorites/books"){
            getFavoritesBooks();
        }
        else if(bookName){
            getSearchBooks();
        }
        else{
            getBooks();
        }
    }, [getBooks, getSearchBooks, bookName, userData]);
    return(
        <div className="MainBooksPage-wrapper">
            <div className="MainBooksPage-content">
                {books?.map((book) => (<BookCard
                    key={book.id}
                    id={book.id}
                    bookName={book.bookName}
                    bookDescription={book.bookDescription}
                    posterFilePath={book.posterFilePath}
                    releaseDate={book.releaseDate}
                    authors={book.authors}
                    genres={book.genres}/>))}
            </div>
        </div>
    )
}
export default MainBooksPage;