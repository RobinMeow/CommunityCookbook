import {
  ChangeDetectionStrategy,
  Component,
  Signal,
  inject
} from '@angular/core'
import { CommonModule } from '@angular/common'
import { RouterLink } from '@angular/router'
import { MatButtonModule } from '@angular/material/button'
import { AuthService } from '../../utils/auth.service'

@Component({
  selector: 'auth-corner',
  standalone: true,
  imports: [CommonModule, MatButtonModule, RouterLink],
  templateUrl: './auth-corner.html',
  styleUrl: './auth-corner.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthCorner {
  private readonly authService = inject(AuthService)

  protected readonly isAuthorizedSignal: Signal<boolean> =
    this.authService.isAuthorized()

  protected logout(): void {
    this.authService.logout()
  }
}
