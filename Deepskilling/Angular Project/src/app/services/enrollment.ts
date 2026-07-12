import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EnrollmentService {
  private apiUrl = 'http://localhost:3000/enrollments';

  constructor(private http: HttpClient) { }

  enrollStudent(data: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, data);
  }

  getEnrollments(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
