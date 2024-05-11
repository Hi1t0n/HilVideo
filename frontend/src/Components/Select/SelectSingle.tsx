import React, { useState } from 'react';
import OutlinedInput from '@mui/material/OutlinedInput';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import ListItemText from '@mui/material/ListItemText';
import Select, { SelectChangeEvent } from '@mui/material/Select';
import Checkbox from '@mui/material/Checkbox';
import {Director} from "../../types/DirectorsTypes";
import {Genre} from "../../types/GenresTypes";
import {MovieType} from "../../types/MovieTypeTypes";

interface Props {
    data: Genre[] | Director[] | MovieType[];
    handleChange: (event: SelectChangeEvent<string>) => void;
    multiple: boolean;
    placeholder: string;
    selectValue: string
}

const SelectSingle: React.FC<Props> = ({ data, handleChange, multiple, placeholder, selectValue }) => {


    return (
        <FormControl required sx={{ m: 1, minWidth: 120 }}>
            <InputLabel id={`select_${placeholder}`}>{placeholder}</InputLabel>
            <Select
                labelId={`select_${placeholder}`}
                id={`select_${placeholder}`}
                value={selectValue}
                label={placeholder}
                onChange={handleChange}
            >
                {data.map((item : Genre | Director | MovieType, index) => (
                    <MenuItem key={item.id} value={item.id}>{item.name}</MenuItem>
                ))}
            </Select>
        </FormControl>

    );
};

export default SelectSingle;