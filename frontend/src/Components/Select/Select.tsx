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

interface Props {
    data: Genre[] | Director[];
    handleChange: (event: SelectChangeEvent<string[]>) => void;
    multiple: boolean;
    placeholder: string;
    selectedData: string[]
}

const SelectCheckBox: React.FC<Props> = ({ data, handleChange, multiple, placeholder, selectedData }) => {


    return (
            <FormControl sx={{ m: 1, minWidth: 200 }}>
                <InputLabel id={`select_${placeholder}`}>{placeholder}</InputLabel>
                <Select
                    labelId={`select_${placeholder}`}
                    id={`select_${placeholder}`}
                    multiple
                    value={selectedData}
                    onChange={handleChange}
                    input={<OutlinedInput label={placeholder} />}
                >
                    {data.map((item : Genre | Director, index) => (
                        <MenuItem key={item.id} value={item.id}>
                            <Checkbox checked={selectedData.includes(item.id)} />
                            <ListItemText primary={item.name} />
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>

    );
};

export default SelectCheckBox;