import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { EnrollmentService } from '../../services/enrollment';

@Component({
  selector: 'app-student-profile',
  imports: [CommonModule, RouterLink],
  templateUrl: './student-profile.html',
  styleUrl: './student-profile.css',
})
export class StudentProfile implements OnInit {
  studentInfo = {
    name: 'Jane Doe',
    email: 'jane.doe@example.com',
    major: 'Computer Science',
    gpa: 3.8
  };

  enrollments: any[] = [];
  isLoading = true;

  constructor(private enrollmentService: EnrollmentService) {}

  ngOnInit() {
    this.enrollmentService.getEnrollments().subscribe({
      next: (data) => {
        this.enrollments = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load enrollments', err);
        this.isLoading = false;
      }
    });
  }
}
