import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { EnrollmentService } from '../../services/enrollment';
import { NotificationService } from '../../services/notification';

@Component({
  selector: 'app-enrollment-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './enrollment-form.html',
  styleUrl: './enrollment-form.css'
})
export class EnrollmentForm {
  studentName = '';
  studentEmail = '';
  courseId: number | null = null;
  preferredSemester = 'Odd';
  agreeToTerms = false;
  submitted = false;

  constructor(
    private enrollmentService: EnrollmentService,
    private notificationService: NotificationService
  ) {}

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.enrollmentService.enrollStudent(form.value).subscribe({
        next: (res) => {
          this.submitted = true;
          this.notificationService.show('Enrollment successful!');
        },
        error: (err) => {
          this.notificationService.show('Error during enrollment.');
          console.error(err);
        }
      });
    }
  }

  resetForm() {
    this.submitted = false;
    this.studentName = '';
    this.studentEmail = '';
    this.courseId = null;
    this.preferredSemester = 'Odd';
    this.agreeToTerms = false;
  }
}
