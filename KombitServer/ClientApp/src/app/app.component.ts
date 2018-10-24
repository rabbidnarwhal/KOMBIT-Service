import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';
import { LoginResponse } from './models/login-response';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: [ './app.component.scss' ]
})
export class AppComponent {
  authChecked = true;
  constructor() {}
}
