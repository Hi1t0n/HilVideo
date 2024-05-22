import {Fragment, useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import {apiUrl, url} from "../../utils/constants";
import axios, {AxiosError} from "axios";
import {Movie} from "../../types/MovieData";
import ReactPlayer from "react-player/file";

function MoviePage(){
    const [movieData, setMovieData] = useState<Movie | null>(null);
    const [errorMessage , setErrorMessage] = useState<string | null>(null);
    const [releaseDate , setReleaseDate] = useState<Date | null>(null);

    const {id} = useParams();

    const getMovieById = async () => {
        try {
            const response = await axios.get<Movie>(`${apiUrl}movie/${id}`);


            if(response.status === 200){
                setMovieData(response.data);
                setReleaseDate(new Date(response.data.releaseDate));
            }
        }
        catch (error){
            const axiosError = error as AxiosError;
            if(axiosError.response){
                // @ts-ignore
                const errorMessage: string = axiosError.response.data.error;
                setErrorMessage(errorMessage);
            }
        }
    }

    useEffect(() => {
        getMovieById();
    },[]);




    return (
    <>
        <div>
            {errorMessage ? (<div className={"errorMessage"}>{errorMessage}</div>) : (movieData && <>
                    <div className={"movie-name"}>
                        <h1>{movieData?.movieName} ({releaseDate?.getFullYear()})</h1>
                    </div>
                    <div className={"poster-wrapper"}>
                        <img className={"poster"} src={`${url}${movieData?.posterFilePath}`} alt={"Постер фильма"}/>
                    </div>
                    <div className={"description-wrapper"}>
                        <h2>О фильме</h2>
                        <div className={"movie-description"}>
                            <h3>Описание</h3>
                            <p>{movieData?.movieDescription}</p>
                        </div>
                        <div className={"directors-wrapper"}>
                            <h3>Продюсеры</h3>
                            <div className={"directors"}>
                                {movieData?.directors.map(item => <p>{item}</p>)}
                            </div>
                        </div>
                        <div className={"genres-wrapper"}>
                            <h3>Жанры</h3>
                            <div>
                                {movieData.genres.map(item => <p>{item}</p>)}
                            </div>
                        </div>
                    </div>
                    <div className={"player-wrapper"}>
                        <Fragment>
                            <ReactPlayer
                                url={`${url}${movieData.moviesFile[0].filePath}`}
                                light={false}
                                controls
                                playing={false}
                                className={"react-player"}
                            />
                        </Fragment>
                    </div>
                </>


            )}
        </div>
    </>)
}

export default MoviePage;