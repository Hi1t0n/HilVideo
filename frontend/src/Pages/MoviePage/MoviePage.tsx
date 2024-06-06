import {Fragment, useEffect, useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import {apiUrl, url} from "../../utils/constants";
import axios, {AxiosError} from "axios";
import {Movie} from "../../types/MovieData";
import ReactPlayer from "react-player/file";
import './MoviePage.css'

function MoviePage() {
    const [movieData, setMovieData] = useState<Movie | null>(null);
    const [errorMessage, setErrorMessage] = useState<string | null>(null);
    const [releaseDate, setReleaseDate] = useState<Date | null>(null);

    const {id} = useParams();

    const navigate = useNavigate();

    const getMovieById = async () => {
        try {
            const response = await axios.get<Movie>(`${apiUrl}movie/${id}`);


            if (response.status === 200) {
                setMovieData(response.data);
                setReleaseDate(new Date(response.data.releaseDate));
            }
        } catch (error) {
            const axiosError = error as AxiosError;
            if (axiosError.response) {
                if (axiosError.response.status === 404) {
                    // @ts-ignore
                    const errorMessage: string = axiosError.response.data.error;
                    if (!errorMessage) {
                        setErrorMessage("Книга не найдена");
                        navigate('/not-found', {state: {errorMessage: errorMessage}});
                    }
                    setErrorMessage(errorMessage);
                    navigate('/not-found', {state: {errorMessage: errorMessage}});
                }
            }
        }
    }

    useEffect(() => {
        getMovieById();
    }, []);


    return (
        <div className={'moviepage-wrapper'}>
            {movieData && (
                <>
                    <div className={"movie-name"}>
                        <h1>{movieData?.movieName} ({releaseDate?.getFullYear()})</h1>
                    </div>
                    <div className={"movie-content-wrapper"}>
                        <div className={"poster-wrapper"}>
                            <img className={"poster"} src={`${url}${movieData?.posterFilePath}`} alt={"Постер фильма"}/>
                        </div>
                        <div className={"description-wrapper"}>
                            <h2>О фильме</h2>
                            <div className={"movie-description"}>
                                <h3>Описание</h3>
                                <p>{movieData?.movieDescription}</p>
                            </div>
                            <div className={"genres-directors-wrapper"}>
                                <div className={"directors-wrapper"}>
                                    <h3>Продюсеры</h3>
                                    <div className={"directors"}>
                                        {movieData?.directors.map(item => <p key={item}>{item}</p>)}
                                    </div>
                                </div>
                                <div className={"genres-wrapper"}>
                                    <h3>Жанры</h3>
                                    <div>
                                        {movieData?.genres.map(item => <p key={item}>{item}</p>)}
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className={"player-wrapper"}>
                            <Fragment>
                                <ReactPlayer
                                    url={`${url}${movieData?.moviesFile[0].filePath}`}
                                    light={false}
                                    controls
                                    playing={false}
                                    className={"react-player"}
                                />
                            </Fragment>
                        </div>
                    </div>
                </>
            )}
        </div>
    );
}

export default MoviePage;