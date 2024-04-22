import { Pipe, PipeTransform } from '@angular/core';
import { Task } from 'src/app/model/Task';

@Pipe({
  name: 'taskSearch'
})
export class TaskSearchPipe implements PipeTransform {



  transform(tasks: Task[] | null, searchText: string): Task[] | null {
    if (!tasks || !searchText) {
      return tasks;
    }

    searchText = searchText.toLowerCase();

    return tasks.filter(task => {
      let  isComplete = task.isComplete ? 'true' :'false' 
      // Check and concatenate all fields to include in the search, handling null or undefined
      const fieldsToSearch = [
        task.id.toString(),
        task.dueDate ? this.formatDate(task.dueDate ) : '', // Custom formatDate function used for readability
        task.description,
        isComplete ,
      ].map(field => field ? field.toLowerCase() : ''); // Convert all fields to lowercase, ensuring empty string for nulls

      // Check if any field includes the searchText
      return fieldsToSearch.some(field => field.includes(searchText));
    });
  }

  // Helper function to format Date objects to a string, could be adjusted based on desired format
  private formatDate(date: Date): string {
    // Example format: YYYY-MM-DD
    const d = new Date(date);
    let month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) 
        month = '0' + month;
    if (day.length < 2) 
        day = '0' + day;

    return [year, month, day].join('-');
  }

}
