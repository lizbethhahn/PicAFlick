import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { WatchlistItem } from '../models/watchlist-item';

@Injectable({
    providedIn: 'root'
})

export class WatchlistService {
  private apiUrl = 'https://localhost:7043/api/Watchlist'
  static itemId: any;
  constructor(private http: HttpClient) { }

  // getAll()
  getAll() {
      return this.http.get<WatchlistItem[]>(this.apiUrl);
  }

  add(item: any) {
      return this.http.post<WatchlistItem>(this.apiUrl, item);
  }

  remove(id: number) {
      return this.http.delete(`${this.apiUrl}/${id}`);
  }
  // getById(id: number)
}