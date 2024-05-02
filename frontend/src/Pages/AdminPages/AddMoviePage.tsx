import {useState} from "react";
import {Genre} from "../../types/GenresTypes";
import {useGetGenresQuery} from "../../store/Api/genresApi";
import Select from "../../Components/Select/Select";
import {useGetDirectorsQuery} from "../../store/Api/directorsApi";

function AddMoviePage(){
    const [selectedGenres,setSelectedGenres] = useState<Genre[]>([]);
    const [selectedDirectors,setSelectedDirectors] = useState<Genre[]>([]);

    const {data: genresData=[]} = useGetGenresQuery();
    const {data: directorsData = []} = useGetDirectorsQuery();

    return(
        <>
            <div>
                <Select multiple={true} data={genresData}></Select>
                <Select multiple={true} data={directorsData}></Select>
            </div>
        </>
    )
}

export default AddMoviePage;