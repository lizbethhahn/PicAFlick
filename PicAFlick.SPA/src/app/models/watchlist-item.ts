import { UserMedia } from "./user-media";
import { MediaType } from "./media-type";

export interface WatchlistItem {
    id: number;
    userId?: string;
    userMediaId: number;
    userMedia: UserMedia;

    tmdbId: number;
    title: string;
    mediaType: MediaType;

    notes?: string;
    watched: boolean;

    posterPath?: string;
    overview?: string;
    releaseDate?: string;
}