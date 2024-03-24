import {createSlice} from "@reduxjs/toolkit";

const LoginSlice = createSlice({
    name: 'isLogin',
    initialState: {
        isLogin: false
    },

    reducers: {
        setLoginState(state, action){
            state.isLogin = action.payload;
        }
    }
});

export const {setLoginState} = LoginSlice.actions;
export default LoginSlice.reducer;
export type RootState = ReturnType<typeof LoginSlice.reducer>;