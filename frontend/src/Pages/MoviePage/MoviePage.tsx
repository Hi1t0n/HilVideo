import {Fragment, useEffect, useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import {apiUrl, url} from "../../utils/constants";
import axios, {AxiosError} from "axios";
import {Movie} from "../../types/MovieData";
import ReactPlayer from "react-player/file";
import StarBorderIcon from '@mui/icons-material/StarBorder';
import StarIcon from '@mui/icons-material/Star';
import './MoviePage.css'
import {useSelector} from "react-redux";
import {RootState} from "../../store/LoginSlice";
import {UserDataState} from "../../store/UserDataSlice";
import {toast, ToastContainer} from "react-toastify";

function MoviePage() {
    const [movieData, setMovieData] = useState<Movie | null>(null);
    const [errorMessage, setErrorMessage] = useState<string | null>(null);
    const [releaseDate, setReleaseDate] = useState<Date | null>(null);
    const [isFavorite, setIsFavorite] = useState<boolean>(false);

    // @ts-ignore
    const isLogin = useSelector((state: RootState) => state.login.isLogin);
    // @ts-ignore
    const userData = useSelector((state: UserDataState)=> state.userData);

    const {id} = useParams();

    const favoriteData = {
        userId: userData.userId,
        movieId: id
    }

    const config = {
        withCredentials: true,
    };

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

    const checkFavorite = async () => {
        await axios.get<boolean>(`${apiUrl}movie/check-favorite`, {
            withCredentials: true,
            params: {
                userId: userData.userId,
                movieId: id
            }
        }).then(async (response) => {
            // @ts-ignore
            setIsFavorite(response.data.isFavorite);
        }).catch(response => setIsFavorite(false));
    }

    const onClickStarIcon = async () => {
        await axios.delete(`${apiUrl}movie/deletemoviefromfavorites`, {
                withCredentials: true,
                data: favoriteData
            }).then(response => {
                if (response.status === 200) {
                    setIsFavorite(false);
                    toast.success("Фильм удален из избранного");
                }
            }).catch(response => {
                toast.error("Что-то пошло не так");
            });
    }

    const onClickStarBorderIcon = async () => {
        await axios.post(`${apiUrl}movie/addmovietofavorites`, favoriteData, config)
            .then(response => {
            if (response.status === 200) {
                setIsFavorite(true);
                toast.success("Фильм добавлен в избранное");
            }
        }).catch(response => {
            toast.error("Что-то пошло не так");
        })
    }

    useEffect(() => {
        getMovieById();
        if(isLogin){
            checkFavorite();
        }
    }, []);



    return (
        <div className={'moviepage-wrapper'}>
            <ToastContainer/>
            {movieData && (
                <>
                    <div className={"movie-name"}>
                        <h1>{movieData?.movieName} ({releaseDate?.getFullYear()})</h1>
                    </div>
                    <div className={"movie-content-wrapper"}>
                        <div className={"poster-wrapper"}>
                            <div className={"poster-wrapper"}>
                                <img className={"poster"} src={`${url}${movieData?.posterFilePath}`} alt={"Постер фильма"}/>
                            </div>
                            {isLogin &&
                                <div>
                                    {isFavorite ? <StarIcon className={'favorites-button'} onClick={onClickStarIcon}/> : <StarBorderIcon className={'favorites-button'} onClick={onClickStarBorderIcon} />}
                                </div>
                            }
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