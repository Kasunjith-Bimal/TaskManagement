import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskinlineComponent } from './task-inline.component';

describe('TaskinlineComponent', () => {
  let component: TaskinlineComponent;
  let fixture: ComponentFixture<TaskinlineComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TaskinlineComponent]
    });
    fixture = TestBed.createComponent(TaskinlineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
