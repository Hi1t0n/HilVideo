import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {UserData} from "../Data/UserData";



interface UserState{
    userData: UserData | null;
}

const initialState: UserState = {
    userData: null,
};

const UserDataSlice = createSlice({
    name: 'userData',
    initialState,
    reducers: {
        saveUserData(state,action: PayloadAction<UserData>){
            state.userData = action.payload;
        }
    }
});

export const {saveUserData} = UserDataSlice.actions;
export default UserDataSlice.reducer;
export type UserDataState = ReturnType<typeof UserDataSlice.reducer>;

