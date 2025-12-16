import { UserMedia } from "./user-media";

export interface WatchlistItem {
    id: number;
    userId?: string;
    userMediaId: number;
    userMedia: UserMedia;
    notes?: string;
    watched: false;
}