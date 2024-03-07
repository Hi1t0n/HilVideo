import React, {useState} from "react";

function Events(){
    const [data, setData  ] = useState('');

    function handleChangeData(event : React.ChangeEvent<HTMLInputElement>){
        setData(event.target.value);
    }

    function handleClickButton(){
        console.log(data);
        alert(data);
    }

    return (
        <div>
            <input value={data} onChange={handleChangeData}/>
            <button onClick={handleClickButton}>Click me noob</button>
        </div>

    );
}

export default Events;