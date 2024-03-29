import { ChangeDetectionStrategy, Component, inject } from '@angular/core'
import { MatButtonModule } from '@angular/material/button'
import { Router } from '@angular/router'

@Component({
  selector: 'recipe-create-menu-button',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './create-recipe.menu-button.html',
  styleUrl: './create-recipe.menu-button.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CreateRecipeMenuButton {
  private readonly router = inject(Router)

  protected onClick() {
    this.router.navigate(['/create-recipe'])
  }
}