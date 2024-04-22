import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskLoaderComponent } from './task-loader.component';

describe('TaskLoaderComponent', () => {
  let component: TaskLoaderComponent;
  let fixture: ComponentFixture<TaskLoaderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TaskLoaderComponent]
    });
    fixture = TestBed.createComponent(TaskLoaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
