import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CourseCard } from '../../components/course-card/course-card';
import { Store } from '@ngrx/store';
import { loadCourses } from '../../store/course.actions';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-course-list',
  imports: [CommonModule, CourseCard],
  templateUrl: './course-list.html',
  styleUrl: './course-list.css'
})
export class CourseList implements OnInit {
  courses$: Observable<any[]>;
  isLoading$: Observable<boolean>;
  selectedCourseId: number | null = null;

  constructor(private store: Store<{ courses: any }>) {
    this.courses$ = this.store.select('courses').pipe(map(s => s.courses));
    this.isLoading$ = this.store.select('courses').pipe(map(s => s.status === 'loading' || s.status === 'pending'));
  }

  ngOnInit() {
    this.store.dispatch(loadCourses());
  }

  trackByCourseId(index: number, course: any): number {
    return course.id;
  }

  onEnroll(courseId: number) {
    console.log('Enrolling in course: ' + courseId);
    this.selectedCourseId = courseId;
  }
}
