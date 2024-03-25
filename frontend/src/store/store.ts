import {configureStore} from "@reduxjs/toolkit";
import loginReducer from "./LoginSlice";
import userDataReducer from "./UserDataSlice";

export const store = configureStore({
    reducer: {
        login: loginReducer,
        userData: userDataReducer,
    }
})