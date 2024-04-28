import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import {apiUrl} from "../../utils/constants";
import {AddDirectorRequest, Director, UpdateDirectorRequest} from "../../types/DirectorsTypes";

export const directorsApi = createApi({
    reducerPath: 'directorsApi',
    tagTypes: ['Director'],
    baseQuery: fetchBaseQuery({baseUrl: apiUrl}),
    endpoints: (build) => ({
        getDirectors: (build.query<Director[], void>) ({
            query: () => 'directors/',
            providesTags: (result) =>
                result
                    ? [
                        ...result.map(({ id }) => ({ type: 'Director' as const, id })),
                        { type: 'Director', id: 'LIST' },
                    ]
                    : [],
        }),
        addDirectors: build.mutation({
            query: (request : AddDirectorRequest) => ({
                url: 'directors/',
                body: request,
                method: 'POST',
            }),
            invalidatesTags: [{ type: 'Director', id: 'LIST' }],
        }),
        updateDirectors: build.mutation({
            query: (request : UpdateDirectorRequest) => ({
                url: 'directors/',
                body: request,
                method: 'PUT',
            }),
            invalidatesTags: [{ type: 'Director', id: 'LIST' }],
        }),
        deleteDirectors: build.mutation({
            query: (id : string) => ({
                url: `directors/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: [{ type: 'Director', id: 'LIST' }],
        })
    })
})

export const {useAddDirectorsMutation,useGetDirectorsQuery,useUpdateDirectorsMutation,useDeleteDirectorsMutation} = directorsApi;