import {configureStore, combineReducers} from "@reduxjs/toolkit";
import {
    persistStore,
    persistReducer,
    FLUSH,
    REHYDRATE,
    PAUSE,
    PERSIST,
    PURGE,
    REGISTER,
} from 'redux-persist';
import {genresApi} from "./Api/genresApi";
import storage from 'redux-persist/lib/storage';
import loginReducer from "./LoginSlice";
import userDataReducer from "./UserDataSlice";
import {directorsApi} from "./Api/directorsApi";
import {movieTypeApi} from "./Api/movieTypeApi";

const rootReducer = combineReducers({
    login: loginReducer,
    userData: userDataReducer,
    [genresApi.reducerPath]: genresApi.reducer,
    [directorsApi.reducerPath]: directorsApi.reducer,
    [movieTypeApi.reducerPath]: movieTypeApi.reducer,
})

const persistConfig = {
    key: 'root',
    storage: storage,
}

const persistedReducer = persistReducer(persistConfig, rootReducer);

const store = configureStore({
    reducer: persistedReducer,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware({
            serializableCheck: {
                ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER],
            },
        }).concat(genresApi.middleware, directorsApi.middleware, movieTypeApi.middleware),
})

export const persistor = persistStore(store)

export default store