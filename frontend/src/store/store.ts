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

const rootReducer = combineReducers({
    login: loginReducer,
    userData: userDataReducer,
    [genresApi.reducerPath]: genresApi.reducer,
    [directorsApi.reducerPath]: directorsApi.reducer,
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
        }).concat(genresApi.middleware, directorsApi.middleware),
})

export const persistor = persistStore(store)

export default store