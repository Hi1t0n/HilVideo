import React, {useRef, useState} from "react";
import {useGetGenresQuery} from "../../store/Api/genresApi";
import {useGetDirectorsQuery} from "../../store/Api/directorsApi";
import SelectCheckBox from "../../Components/Select/SelectCheckBox";
import {SelectChangeEvent} from "@mui/material/Select";
import './AddMoviePage.css';
import {useGetMovieTypeQuery} from "../../store/Api/movieTypeApi";
import SelectSingle from "../../Components/Select/SelectSingle";
import axios, {AxiosError} from "axios";

function AddMoviePage(){
    const [selectedMovieFile , setSelectedMovieFile] = useState<File | null>(null);
    const [selectedPosterFile, setSelectedPosterFile] = useState<File | null>(null);
    const [selectedGenres,setSelectedGenres] = useState<string[]>([]);
    const [selectedDirectors,setSelectedDirectors] = useState<string[]>([]);
    const [selectedMovieType, setSelectedMovieType] = useState<string>("");
    const [movieName , setMovieName] = useState("");
    const [description , setDescription] = useState("");
    const [releaseDate, setReleaseDate] = useState(new Date().toISOString());


    const {data: genresData=[], refetch: genresRefetch} = useGetGenresQuery(undefined, {pollingInterval: 600000});
    const {data: directorsData = [], refetch: directorsRefetch } = useGetDirectorsQuery(undefined, {pollingInterval: 3000});
    const {data: movieTypeData = [], refetch: movieTypeRefetch  } = useGetMovieTypeQuery(undefined, {pollingInterval: 3000});

    const PosterFileRef = useRef<HTMLInputElement | null>(null);
    const MovieFileRef = useRef<HTMLInputElement | null>(null);


    const handleChangeMovieFile = async (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files && e.target.files[0];
        if(file){
            setSelectedMovieFile(file);
        }

    }

    const handleChangePosterFile = async (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files && e.target.files[0];
        if(file){
            setSelectedPosterFile(file);
        }
    }

    const handleUpload = async () => {
        if(!movieName) {
            alert("Пожалуйста введите название фильма");
            return;
        }

        if(!description){
            alert("Пожалуйста введите описание");
            return;
        }

        if(!selectedPosterFile){
            alert("Пожалуйста выберите постер");
            return;
        }

        if(!selectedMovieFile){
            alert("Пожалуйста выберите фильм");
            return;
        }


        if(selectedGenres.length === 0){
            alert("пожалуйста выберите жанры");
            return;
        }

        if(selectedDirectors.length === 0){
            alert("пожалуйста выберите режисееров");
            return;
        }

        if(selectedMovieType === ""){
            alert("пожалуйста выберите тип");
            return;
        }

        const formData = new FormData();
        formData.append('Poster', selectedPosterFile);
        formData.append('Movie', selectedMovieFile);
        formData.append('MovieName', movieName);
        formData.append("MovieType", selectedMovieType);
        formData.append('MovieDescription', description);
        formData.append('ReleaseDate', releaseDate);
        formData.append('Directors', JSON.stringify(selectedDirectors));
        formData.append('Genres', JSON.stringify(selectedGenres));


        try {
            const response = axios.post('https://localhost:7099/api/movie/',formData, {
                withCredentials: true,
                headers: {
                    'Content-Type': 'multipart/form-data'

                }
            })


        }catch (error){
            const axiosError = error as AxiosError;
            if(axiosError.response){
                // @ts-ignore
                const errorMessage: string = axiosError.response.data.error;
                alert(errorMessage);
                return;
            }
        }

    }


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

    const handleChangeMovieType = (event: SelectChangeEvent<string>) => {
        setSelectedMovieType(event.target.value);
    }

    const handleChangeReleaseDate = (event: React.ChangeEvent<HTMLInputElement>) =>{
        setReleaseDate(event.target.value);
    }

    const handlePickPoster = () =>{
        PosterFileRef.current?.click();
    }

    const handlePickMovie = () => {
        MovieFileRef.current?.click();
    }



    return(
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
                        <button onClick={handlePickPoster}>Выберите постер</button>
                        <input className={'file-input'} ref={PosterFileRef} type={'file'} name={"poster"} accept="image/png, image/jpeg"
                               required={true} onChange={handleChangePosterFile}/>
                    </div>
                    <div>
                        <button onClick={handlePickMovie}>Выберите фильм</button>
                        <input className={'file-input'} ref={MovieFileRef} type={'file'} name={"movie"} accept="video/*, .mp4" required={true} onChange={handleChangeMovieFile}/>
                    </div>
                    <div>
                        <input className={'data-input'} type={'date'} onChange={handleChangeReleaseDate} required/>
                    </div>
                    <div>
                        <SelectCheckBox data={genresData} handleChange={handleChangeGenresData} multiple={true}
                                        placeholder={"Жанры"} selectedData={selectedGenres} required={true}/>
                    </div>
                    <div>
                        <SelectCheckBox data={directorsData} handleChange={handleChangeDirectorsData} multiple={true}
                                        placeholder={"Режиссеры"} selectedData={selectedDirectors} required={true}/>
                    </div>
                    <div>
                        <SelectSingle data={movieTypeData} handleChange={handleChangeMovieType} multiple={false} placeholder={"Тип"} selectValue={selectedMovieType}/>
                    </div>
                    <button onClick={handleUpload} className={"add-movie-button"}>{"Добавить фильм"}</button>
                </div>
            </div>
    )
}

export default AddMoviePage;