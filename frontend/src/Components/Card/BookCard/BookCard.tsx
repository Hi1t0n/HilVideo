import {url} from "../../../utils/constants";
import {useNavigate} from "react-router-dom";
import './BookCard.css';

interface Props{
    id: string;
    bookName: string;
    bookDescription: string;
    posterFilePath: string;
    releaseDate: Date;
    authors: string[];
    genres: string[];
}

function BookCard({id,bookName,bookDescription,posterFilePath, releaseDate,authors, genres} : Props){

    const navigate = useNavigate();

    const date = new Date(releaseDate);

    function handleClick(){
        navigate(`/book/${id}`, {replace: false});
    }

    return(
        <div className="BookCard-wrapper" id={id} onClick={handleClick}>
            <div className="BookCard-content">
                <div className="BookCard-poster">
                    <img src={`${url}${posterFilePath}`} alt={"постер"}/>
                </div>
                <div className="BookCard-bookName">
                    <h1>{bookName}({date.getFullYear()})</h1>
                </div>
                <div className="BookCard-bookDescription">
                    <p>{bookDescription.length > 70 ? bookDescription.substring(0, 70) + '...' : bookDescription}</p>
                </div>
                <div className="BookCard-authors">
                    {authors.length > 1 ? <p>{`${authors[0]} + ${authors.length - 1}`}</p> : <p>{authors[0]}</p>}
                </div>
                <div className="BookCard-genres">
                    {genres.length > 1 ? <p>{`${genres[0]} + ${genres.length - 1}`}</p> : <p>{genres[0]}</p>}
                </div>
            </div>
        </div>
    )
}

export default BookCard;