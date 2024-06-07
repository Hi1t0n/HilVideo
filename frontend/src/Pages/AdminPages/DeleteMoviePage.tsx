import SelectSingle from "../../Components/Select/SelectSingle";
import {useEffect, useState} from "react";
import axios from "axios";
import {apiUrl} from "../../utils/constants";
import {MovieDataForDelete} from "../../types/MovieData";
import {toast, ToastContainer} from "react-toastify";
import {SelectChangeEvent} from "@mui/material/Select";
import './DeleteMoviePage.css';

function DeleteMoviePage (){

    const [movies, setMovies] = useState<MovieDataForDelete[]>([]);
    const [selectedMovie, setSelectedMovie] = useState<string>("");
    const [confirmMsg, setConfirmMsg] = useState("");

    const getMovieData = async () => {
        axios.get(`${apiUrl}movie/get-movie-id-with-name`, {
            withCredentials: true
        }).then(result => {
            if(result.status === 200){
                setMovies(result.data);
            }
        }).catch(err=>{
            toast.error("Что-то пошло не так");
        })
    }

    const handleChangeMovie = (event: SelectChangeEvent<string>) => {
        setSelectedMovie(event.target.value);
    }

    const onClickDeleteButton = async () => {
        if(!selectedMovie){
            toast.error('Выберите фильм');
            return;
        }
        if(confirmMsg !== "УДАЛИТЬ"){
            toast.error('Подтверждающее сообщение должно быть "УДАЛИТЬ"');
            return;
        }

        axios.delete(`${apiUrl}movie/${selectedMovie}`, {
            withCredentials: true
        }).then(response => {
            if(response.status === 200){
                toast.success("Фильм успешно удален");
                window.location.reload();
            }
        }).catch(err=>{
            toast.error("Что-то пошло не так");
        })
    }

    useEffect(() => {
        getMovieData();
    }, []);

    return(
      <div className={"delete-movie-wrapper"}>
          <ToastContainer/>
          <div className={"select-wrapper"}>
              <SelectSingle data={movies} handleChange={handleChangeMovie} multiple={false} placeholder={"Фильм"} selectValue={selectedMovie}/>
          </div>
          <div className={"input-wrapper"} >
              <input className={'delete-movie-input'} placeholder={'Введите "УДАЛИТЬ"'} required value={confirmMsg} onChange={(e) => setConfirmMsg(e.target.value)}/>
          </div>
          <div className={"button-wrapper"}>
              <button className={'delete-movie-button'} onClick={onClickDeleteButton}>Удалить</button>
          </div>
      </div>
    );
}

export default DeleteMoviePage;