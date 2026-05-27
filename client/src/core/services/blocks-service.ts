import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PaginatedResult } from '../../types/pagination';
import { Member } from '../../types/member';

@Injectable({
  providedIn: 'root',
})
export class BlocksService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  blockIds = signal<string[]>([]);
  

  block(targetMemberId: string, reason: string) {
    return this.http.post(`${this.baseUrl}blocks/${targetMemberId}`, { reason: reason }).subscribe({
      next: () => {
        this.blockIds.update((currentIds) => [...currentIds, targetMemberId]);
      },
      error: (error) => {
        console.error('Failed to block user:', error);
      },
    });
  }

  getBlockedMembers(pageNumber: number, pageSize: number) {
    const params = new HttpParams().append('pageNumber', pageNumber).append('pageSize', pageSize);

    return this.http.get<PaginatedResult<Member>>(`${this.baseUrl}blocks`, { params });
  }

  unblock(targetMemberId: string) {
   return this.http.delete<void>(`${this.baseUrl}blocks/${targetMemberId}`);
  }

  editReason(targetMemberId: string, reason: string){
    return this.http.put<void>(`${this.baseUrl}blocks/${targetMemberId}`, {reason: reason});
  }
}
