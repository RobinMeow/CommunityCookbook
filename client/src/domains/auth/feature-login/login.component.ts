import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import {
  FormControl,
  NonNullableFormBuilder,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { AuthService } from '../utils/auth.service';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ChefFromControlFactory } from '../utils/ChefFormControlFactory';
import { ChefConstraints } from '../utils/ChefConstraints';
import { PasswordComponent } from '../ui/password/password.component';

@Component({
  selector: 'auth-login',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatIconModule,
    MatButtonModule,
    PasswordComponent,
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss', '../utils/auth.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {
  private readonly _authService = inject(AuthService);
  private readonly _router = inject(Router);
  private readonly _nnfb = inject(NonNullableFormBuilder);

  protected readonly chefConstraints = ChefConstraints;

  private readonly _chefFormControlFactory = new ChefFromControlFactory(
    this._nnfb,
  );

  protected readonly loginForm = this._nnfb.group({
    chefname: this._chefFormControlFactory.Name(),
    password: this._chefFormControlFactory.Password(),
  });

  protected readonly chefnameControl: FormControl<string> =
    this.loginForm.controls.chefname;

  protected readonly passwordControl: FormControl<string> =
    this.loginForm.controls.password;

  protected async onSubmit(): Promise<void> {
    if (this.loginForm.invalid) return;

    await this._authService.loginAsync({
      name: this.chefnameControl.value,
      password: this.passwordControl.value,
    });
    await this._router.navigateByUrl('/');
  }
}
