import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { WatchlistItem } from '../models/watchlist-item';

@Injectable({
    providedIn: 'root'
})

export class WatchlistService {
    private apiUrl = 'https://localhost:7043/api/Watchlist'
    constructor(private http: HttpClient) { }

    // getAll()
    getAll() {
        return this.http.get<WatchlistItem[]>(this.apiUrl);
    }

    // getById(id: number)
}