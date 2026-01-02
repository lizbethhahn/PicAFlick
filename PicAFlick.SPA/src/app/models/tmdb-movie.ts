export interface TmdbMovieDto {
    adult: boolean;
    backdropPath?: string;
    genreIds?: number[];
    tmdbMovieId: number;
    originalLanguage?: string;
    originalTitle?: string;
    overview?: string;
    popularity?: number;
    posterPath?: string;
    releaseDate?: string;
    title?: string;
    video: boolean;
    voteAverage?: number;
    voteCount?: number;
}