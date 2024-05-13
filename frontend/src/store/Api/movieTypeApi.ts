import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import {apiUrl} from "../../utils/constants";
import {MovieType, UpdateMovieType} from "../../types/MovieTypeTypes";

export const  movieTypeApi = createApi({
    reducerPath: 'movieTypeApi',
    tagTypes: ['MovieType'],
    baseQuery: fetchBaseQuery({baseUrl:apiUrl, credentials: 'include'}),
    endpoints: (build) => ({
        getMovieType: build.query<MovieType[], void>({
            query : () => ({
                url:'movietype/',
                method: 'GET',
            }),
            providesTags: (result) =>
                result
                    ? [
                        ...result.map(({ id }) => ({ type: 'MovieType' as const, id })),
                        { type: 'MovieType', id: 'LIST' },
                    ]
                    : [],
        }),
        addMovieType: build.mutation({
            query: (movieTypeName: string) => ({
                url: 'movietype/',
                params: { movieTypeName },
                method: 'POST',
            }),
            invalidatesTags: [{ type: 'MovieType', id: 'LIST' }],
        }),
        updateGenres: build.mutation({
            query: (request: UpdateMovieType) => ({
                url: 'movietype/',
                body: request,
                method: 'PUT'
            }),
            invalidatesTags: [{ type: 'MovieType', id: 'LIST' }],
        }),
    }),
});

export const {useAddMovieTypeMutation, useGetMovieTypeQuery, useUpdateGenresMutation} = movieTypeApi