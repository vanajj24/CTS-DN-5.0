import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreditLabelPipe } from '../../pipes/credit-label-pipe';
import { Highlight } from '../../directives/highlight';

@Component({
  selector: 'app-course-card',
  imports: [CommonModule, CreditLabelPipe, Highlight],
  templateUrl: './course-card.html',
  styleUrl: './course-card.css'
})
export class CourseCard implements OnChanges {
  @Input() course: any;
  @Output() enrollRequested = new EventEmitter<number>();
  
  isExpanded = false;
  isEnrolled = false;

  ngOnChanges(changes: SimpleChanges) {
    if (changes['course']) {
      console.log('Course changed. Prev:', changes['course'].previousValue, 'Current:', changes['course'].currentValue);
    }
  }

  get cardClasses() {
    return {
      'card--enrolled': this.isEnrolled,
      'card--full': this.course?.credits >= 4,
      'expanded': this.isExpanded
    };
  }

  getBorderColor() {
    switch(this.course?.gradeStatus) {
      case 'passed': return '#10b981';
      case 'failed': return '#ef4444';
      case 'pending': return '#94a3b8';
      default: return 'transparent';
    }
  }
}
