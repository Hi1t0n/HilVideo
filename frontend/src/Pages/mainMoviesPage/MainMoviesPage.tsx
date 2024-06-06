import {useCallback, useEffect, useState} from "react";
import axios, {AxiosError} from "axios";
import {Movies} from "../../types/MovieData";
import {apiUrl} from "../../utils/constants";
import MovieCard from "../../Components/Card/MovieCard/MovieCard";
import "../mainMoviesPage/MainMoviesPage.css"
import {useLocation, useNavigate, useParams} from "react-router-dom";
import {useSelector} from "react-redux";
import {toast, ToastContainer} from "react-toastify";
import {UserDataState} from "../../store/UserDataSlice";
import {UserData} from "../../Data/UserData";

function MainMoviesPage() {
    const { movieName } = useParams();
    const navigate = useNavigate();
    const location = useLocation();

    const [movies, setMovies] = useState<Movies[] | null>(null);

    // @ts-ignore
    const userData = useSelector((state) => state.userData);

    const getMovies = useCallback(async () => {
        try {
            const response = await axios.get<Movies[]>(`${apiUrl}movie/`);
            if (response.status === 200) {
                setMovies(response.data);
            }
        } catch (error) {
            toast.error("Что-то пошло не так");
        }
    }, []);

    const getSearchMovies = useCallback(async () => {
        try {
            const response = await axios.get<Movies[]>(`${apiUrl}movie/search/${movieName}`);
            if (response.status === 200) {
                setMovies(response.data);
            }
        } catch (error) {
            if (axios.isAxiosError(error) && error.response && error.response.status === 404) {
                navigate('/not-found', { state: { errorMessage: error.response.data.error }, replace: false });
            }
        }
    }, [movieName, navigate]);

    const getFavoritesMovies = useCallback(async () => {
        try {
            const response = await axios.get<Movies[]>(`${apiUrl}movie/getfavoritemovies/${userData.userId}`, {
                withCredentials: true
            });

            if (response.status === 200) {
                setMovies(response.data);
                return;
            }
        }
        catch (error){
            toast.error("Что-то пошло не так");
        }
    }, [userData.userId])

    useEffect(() => {
        if (location.pathname === "/favorites/movies") {
            getFavoritesMovies();
        } else if (movieName) {
            getSearchMovies();
        } else {
            getMovies();
        }
    }, [getMovies, getSearchMovies, getFavoritesMovies, location.pathname, movieName, userData]);

    return(
        <div className="MainMoviesPage-wrapper">
            <ToastContainer/>
            <div className="MainMoviesPage-content">
                {movies?.map((movie) => (<MovieCard
                    key={movie.id}
                    id={movie.id}
                    movieName={movie.movieName}
                    movieDescription={movie.movieDescription}
                    posterFilePath={movie.posterFilePath}
                    movieType={movie.movieType}
                    releaseDate={new Date(movie.releaseDate)}
                    directors={movie.directors}
                    genres={movie.genres}/>))}
            </div>
        </div>
    )
}
export default MainMoviesPage;