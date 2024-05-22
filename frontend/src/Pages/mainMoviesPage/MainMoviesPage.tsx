import {useEffect, useState} from "react";
import axios, {AxiosError} from "axios";
import {Movies} from "../../types/MovieData";
import {apiUrl} from "../../utils/constants";
import MovieCard from "../../Components/Card/MovieCard/MovieCard";
import "../mainMoviesPage/MainMoviesPage.css"

function MainMoviesPage() {
    const [movies, setMovies] = useState<Movies[] | null>(null);

    async function getMovies() : Promise<void> {
        try{
            const response  = await axios.get<Movies[]>(`${apiUrl}movie/`);

            if(response.status === 200){
                setMovies(response.data);
            }
        }
        catch (error)
        {
            console.error(error);
        }
    }

    useEffect(() => {
        getMovies();
    }, []);
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