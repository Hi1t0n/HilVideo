import {useCallback, useEffect, useState} from "react";
import axios, {AxiosError} from "axios";
import {Movies} from "../../types/MovieData";
import {apiUrl} from "../../utils/constants";
import MovieCard from "../../Components/Card/MovieCard/MovieCard";
import "../mainMoviesPage/MainMoviesPage.css"
import {useNavigate, useParams} from "react-router-dom";

function MainMoviesPage() {
    const { movieName } = useParams();
    const navigate = useNavigate(); 
    
    const [movies, setMovies] = useState<Movies[] | null>(null);

    const getMovies = useCallback(async () => {
        try {
            const response = await axios.get<Movies[]>(`${apiUrl}movie/`);
            if (response.status === 200) {
                setMovies(response.data);
            }
        } catch (error) {
            console.error(error);
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
                console.log(error.response.data.error);
                navigate('/not-found', { state: { errorMessage: error.response.data.error }, replace: false });
            }
        }
    }, [movieName, navigate]);

    useEffect(() => {
        if(movieName){
            getSearchMovies();
        }
        else{
            getMovies();
        }
    }, [getMovies, getSearchMovies, movieName]);
    return(
        <div className="MainMoviesPage-wrapper">
            <div className="MainMoviesPage-content">
                {movies?.map((movie) => (<MovieCard
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