import { Component, inject } from '@angular/core';
import { Location } from '@angular/common';  // Import from @angular/common, not the global Location

@Component({
  selector: 'app-not-found',
  imports: [],
  templateUrl: './not-found.html',
  styleUrl: './not-found.css',
})
export class NotFound {
  private location = inject(Location);  // Now this is Angular's Location service

  goBack(){
    this.location.back();  // This will work now
  }
}