export interface Movie {
    movieId: string;
    movieName: string;
    movieDescription: string;
    moviesFile: MovieFileDTO[];
    posterFilePath: string;
    movieType: string;
    releaseDate: Date;
    directors: string[];
    genres: string[];
}
export interface MovieFileDTO{
    filePath: string;
    episodNumber: number;
}