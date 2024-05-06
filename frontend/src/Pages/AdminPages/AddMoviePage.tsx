import React, {useState} from "react";
import {useGetGenresQuery} from "../../store/Api/genresApi";
import {useGetDirectorsQuery} from "../../store/Api/directorsApi";
import SelectCheckBox from "../../Components/Select/Select";
import {SelectChangeEvent} from "@mui/material/Select";
import './AddMoviePage.css'

function AddMoviePage(){
    const [selectedGenres,setSelectedGenres] = useState<string[]>([]);
    const [selectedDirectors,setSelectedDirectors] = useState<string[]>([]);
    const [movieName , setMovieName] = useState("");
    const [description , setDescription] = useState("");

    const {data: genresData=[]} = useGetGenresQuery();
    const {data: directorsData = [], } = useGetDirectorsQuery();


    const handleChangeGenresData = (event: SelectChangeEvent<string[]>) => {
        const {
            target: { value },
        } = event;
        setSelectedGenres(value as string[]);
    };

    const handleChangeDirectorsData = (event: SelectChangeEvent<string[]>) => {
        const {
            target: { value },
        } = event;
        setSelectedDirectors(value as string[]);
    };

    const handleClick = async () => {

    }


    return(
        <form>
            <div className={"wrapper"}>
                <div className={"content"}>
                    <h1 className={"form-name"}>Добавление фильма</h1>
                    <div>
                        <input className={"movie-input"} placeholder={"Название*"} type={'text'} value={movieName}
                               required={true}
                               onChange={(e: React.ChangeEvent<HTMLInputElement>) => setMovieName(e.target.value)}/>
                    </div>
                    <div>
                        <textarea className={"movie-textarea"} placeholder={"Описание*"} value={description} required={true}
                          onChange={(e: React.ChangeEvent<HTMLTextAreaElement>) => setDescription(e.target.value)}/>
                    </div>
                    <div>
                        <label>Постер:</label>
                        <input className={'file-input'} type={'file'} name={"poster"} accept="image/png, image/jpeg" required={true}/>
                    </div>
                    <div>
                        <label>Фильм:</label>
                        <input className={'file-input'} type={'file'} name={"movie"} accept="video/*" required={true}/>
                    </div>
                    <div>
                        <SelectCheckBox data={genresData} handleChange={handleChangeGenresData} multiple={true}
                                        placeholder={"Жанры"} selectedData={selectedGenres}/>
                    </div>
                    <div>
                        <SelectCheckBox data={directorsData} handleChange={handleChangeDirectorsData} multiple={true}
                                        placeholder={"Режиссеры"} selectedData={selectedDirectors}/>
                    </div>
                    <button onClick={handleClick} className={"add-movie-button"}>{"Добавить фильм"}</button>
                </div>
            </div>
        </form>
    )
}

export default AddMoviePage;