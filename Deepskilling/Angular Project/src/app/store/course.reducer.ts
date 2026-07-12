import { createReducer, on } from '@ngrx/store';
import { loadCourses, loadCoursesSuccess, loadCoursesFailure } from './course.actions';

export interface CourseState {
  courses: any[];
  error: string | null;
  status: 'pending' | 'loading' | 'error' | 'success';
}

export const initialState: CourseState = {
  courses: [],
  error: null,
  status: 'pending'
};

export const courseReducer = createReducer(
  initialState,
  on(loadCourses, (state) => ({ ...state, status: 'loading' })),
  on(loadCoursesSuccess, (state, { courses }) => ({ ...state, courses, error: null, status: 'success' })),
  on(loadCoursesFailure, (state, { error }) => ({ ...state, error, status: 'error' }))
);
