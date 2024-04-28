import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import {Genre, UpdateGenreRequest} from "../../types/GenresTypes";
import {apiUrl} from "../../utils/constants";

export const genresApi = createApi({
    reducerPath: 'genresApi',
    tagTypes: ['Genre'],
    baseQuery: fetchBaseQuery({ baseUrl: apiUrl }),
    endpoints: (build) => ({
        getGenres: build.query<Genre[], void>({
            query: () => 'genres/',
            providesTags: (result) =>
                result
                    ? [
                        ...result.map(({ id }) => ({ type: 'Genre' as const, id })),
                        { type: 'Genre', id: 'LIST' },
                    ]
                    : [],
        }),
        addGenres: build.mutation({
            query: (genreName: string) => ({
                url: 'genres/',
                params: { genreName },
                method: 'POST',
            }),
            invalidatesTags: [{ type: 'Genre', id: 'LIST' }],
        }),
        updateGenres: build.mutation({
            query: (request: UpdateGenreRequest) => ({
                url: 'genres/',
                body: request,
                method: 'PUT'
            }),
            invalidatesTags: [{ type: 'Genre', id: 'LIST' }],
        }),
        deleteGenres: build.mutation({
            query: (id: string) => ({
                url: `genres/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: [{ type: 'Genre', id: 'LIST' }],
        })
    }),
});

export const { useGetGenresQuery, useAddGenresMutation, useUpdateGenresMutation, useDeleteGenresMutation } = genresApi;