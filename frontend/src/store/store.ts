import {configureStore} from "@reduxjs/toolkit";
import loginReducer from "./LoginSlice";

export const store = configureStore({
    reducer: {
        login: loginReducer,
    }
})