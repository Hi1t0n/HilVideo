export interface Director {
    id: string,
    name: string,
}

export interface AddDirectorRequest {
    FirstName : string,
    SecondName : string,
    Patronymic? : string
}

export interface UpdateDirectorRequest {
    id : string,
    FirstName : string,
    SecondName : string,
    Patronymic? : string
}