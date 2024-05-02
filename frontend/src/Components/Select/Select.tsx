import {Genre} from "../../types/GenresTypes";
import {Director} from "../../types/DirectorsTypes";

interface Props {
    data: Genre[] | Director [];
    multiple : boolean;

}

function Select({data, multiple}: Props){
    return(
        <>
            <select multiple={multiple}>
                {data.map((item : Genre | Director) =>
                <option key={item.id} value={item.id}>{item.name}</option>)}
            </select>
        </>
    )
}

export default Select;