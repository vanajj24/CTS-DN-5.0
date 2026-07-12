import { createAction, props } from '@ngrx/store';

export const loadCourses = createAction('[Course List] Load Courses');
export const loadCoursesSuccess = createAction('[Course List] Load Courses Success', props<{ courses: any[] }>());
export const loadCoursesFailure = createAction('[Course List] Load Courses Failure', props<{ error: string }>());
