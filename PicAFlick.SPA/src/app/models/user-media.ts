import { MediaType } from './media-type';

export interface UserMedia {
  id: number;
  tmdbId: number;
  title: string;
  posterPath?: string;
  overview?: string;
  mediaType: MediaType; 
}
