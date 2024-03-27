import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {UserData} from "../Data/UserData";




const initialState: UserData | null = null;

const UserDataSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        setUserData(state,action){
            return action.payload;
        }
    }
});

export const {setUserData} = UserDataSlice.actions;
export default UserDataSlice.reducer;
export type UserDataState = ReturnType<typeof UserDataSlice.reducer>;

