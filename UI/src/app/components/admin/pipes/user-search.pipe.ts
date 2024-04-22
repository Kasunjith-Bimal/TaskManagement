import { Pipe, PipeTransform } from '@angular/core';
import { User } from 'src/app/model/User';

@Pipe({
  name: 'userSearch'
})
export class UserSearchPipe implements PipeTransform {

  transform(users: User[] | null, searchText: string): User[] | null {
    if (!users || !searchText) {
      return users;
    }

    searchText = searchText.toLowerCase();

    return users.filter(user => {
      const isActiveString = user.isActive ? 'true' : 'false';
      // Check and concatenate all fields to include in the search, handling null or undefined
      const fieldsToSearch = [
        user.id,
        user.email,
        user.fullName,
        isActiveString
      ].map(field => field ? field.toLowerCase() : ''); // Convert all fields to lowercase, ensuring empty string for nulls

      // Check if any field includes the searchText
      return fieldsToSearch.some(field => field.includes(searchText));
    });
  }
}
