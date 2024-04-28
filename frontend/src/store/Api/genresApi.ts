import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { Genre } from "../../types/Genre";

export const genresApi = createApi({
    reducerPath: 'genresApi',
    tagTypes: ['Genre'],
    baseQuery: fetchBaseQuery({ baseUrl: 'https://localhost:7099/api/' }),
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
        })
    }),
});

export const { useGetGenresQuery, useAddGenresMutation } = genresApi;