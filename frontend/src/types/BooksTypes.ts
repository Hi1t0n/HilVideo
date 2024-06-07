export interface Books {
    id: string;
    bookName: string;
    bookDescription: string;
    posterFilePath: string;
    releaseDate: Date;
    authors: string[];
    genres: string[];
}

export interface Book {
    bookId: string;
    bookName: string;
    bookDescription: string;
    bookFilePath: string;
    posterFilePath: string;
    releaseDate: Date;
    authors: string[];
    genres: string[];
}

export interface BookDataForDelete {
    id: string;
    name: string;
}