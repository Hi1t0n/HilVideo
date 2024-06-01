import SearchIcon from '@mui/icons-material/Search';
import './SearchBar.css'
import React from "react";

interface SearchBarProps {
    onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    onClick: () => void;
    value: string;
}

function SearchBar({onChange, onClick, value}: SearchBarProps) {
    return(
      <div className={"SearchBar-wrapper"}>
          <input placeholder={"Поиск"} onChange={onChange} autoComplete={'off'} value={value} className="search-input"/>
          <SearchIcon className={"Icon"} onClick={onClick}/>
      </div>
    );
}

export default SearchBar;