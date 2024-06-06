export interface Author
{
    id: string;
    name: string;
}

export interface AddAuthorRequest{
    firstName: string,
    secondName: string,
    patronymic: string
}

export interface UpdateAuthorRequest{
    id: string,
    firstName: string,
    secondName: string,
    patronymic: string
}