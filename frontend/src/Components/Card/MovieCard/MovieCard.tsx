import {url} from "../../../utils/constants";
import {useNavigate} from "react-router-dom";
import '../MovieCard/MovieCard.css'

interface Props{
    id: string;
    movieName: string;
    movieDescription: string;
    posterFilePath: string;
    movieType: string;
    releaseDate: Date;
    directors: string[];
    genres: string[];
}

function MovieCard({id,movieName,movieDescription,posterFilePath,movieType, releaseDate,directors, genres} : Props){

    const navigate = useNavigate();

    function handleClick(){
        navigate(`/movie/${id}`, {replace: false});
    }

    return(
        <div className="MovieCard-wrapper" id={id} onClick={handleClick}>
            <div className="MovieCard-content">
                <div className="MovieCard-poster">
                    <img src={`${url}${posterFilePath}`} alt={"постер"}/>
                </div>
                <div className="MovieCard-movieName">
                    <h1>{movieName}({releaseDate.getFullYear()})</h1>
                </div>
                <div className="MovieCard-movieDescription">
                    <p>{movieDescription.length > 70 ? movieDescription.substring(0, 70) + '...' : movieDescription}</p>
                </div>
                <div className="MovieCard-movieType">
                    <p>{movieType}</p>
                </div>
                <div className="MovieCard-directors">
                    {directors.length > 1 ? <p>{`${directors[0]} + ${directors.length - 1}`}</p> : <p>{directors[0]}</p>}
                </div>
                <div className="MovieCard-genres">
                    {genres.length > 1 ? <p>{`${genres[0]} + ${genres.length - 1}`}</p> : <p>{genres[0]}</p>}
                </div>
            </div>
        </div>
    )
}

export default MovieCard;