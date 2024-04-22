import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Task } from '../model/Task';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private http: HttpClient) { }

  getTaskById(id : number) {
    const url = `${environment.baseUrl}api/task/${id}`;
    return this.http.get(url)
  }

  updateTask(task:Task) {
    const url = `${environment.baseUrl}api/task/${task.id}`;
    return this.http.put(url,task);
  }

  deleteTask(id:number) {
    const url = `${environment.baseUrl}api/task/${id}`;
    return this.http.delete(url);
  }

  addTask(task:Task) {
    const url = `${environment.baseUrl}api/task`;
    return this.http.post(url,task);
  }
}
