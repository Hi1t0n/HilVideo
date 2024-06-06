import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import {apiUrl} from "../../utils/constants";
import {AddAuthorRequest, Author, UpdateAuthorRequest} from "../../types/AuthorsTypes";

export const authorsApi = createApi({
    reducerPath: 'authorsApi',
    tagTypes: ['Author'],
    baseQuery: fetchBaseQuery({baseUrl: apiUrl}),
    endpoints: (build) => ({
        getAuthors: (build.query<Author[], void>) ({
            query: () => ({
                url: 'author/',
                method: 'GET',
            }),
            providesTags: (result) =>
                result
                    ? [
                        ...result.map(({ id }) => ({ type: 'Author' as const, id })),
                        { type: 'Author', id: 'LIST' },
                    ]
                    : [],
        }),

        addAuthors: build.mutation({
            query: (request : AddAuthorRequest) => ({
                url: 'author/',
                body: request,
                method: 'POST',
            }),
            invalidatesTags: [{ type: 'Author', id: 'LIST' }],
        }),
        updateAuthors: build.mutation({
            query: (request : UpdateAuthorRequest) => ({
                url: 'author/',
                body: request,
                method: 'PUT',
            }),
            invalidatesTags: [{ type: 'Author', id: 'LIST' }],
        }),
        deleteAuthors: build.mutation({
            query: (id : string) => ({
                url: `author/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: [{ type: 'Author', id: 'LIST' }],
        })
    })
})

export const {useGetAuthorsQuery, useAddAuthorsMutation,useDeleteAuthorsMutation,useUpdateAuthorsMutation} = authorsApi;