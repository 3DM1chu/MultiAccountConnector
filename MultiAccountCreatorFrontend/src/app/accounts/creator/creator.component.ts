import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-creator',
  imports: [FormsModule],
  templateUrl: './creator.component.html',
  styleUrl: './creator.component.css'
})
export class CreatorComponent {
  accountDetails = {
    name: "",
    email: "",
    serviceName: ""
  }
  onSubmit() {
    console.log(this.accountDetails);
    this.accountDetails = {
      name: "",
      email: "",
      serviceName: "Uber"
    }
  }
}
