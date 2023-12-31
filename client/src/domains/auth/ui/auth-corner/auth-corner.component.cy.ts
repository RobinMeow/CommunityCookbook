import { provideHttpClient } from '@angular/common/http';
import { AuthCornerComponent } from './auth-corner.component';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthService } from '../../utils/auth.service';

describe('auth-corner should', () => {
  beforeEach('mount', () => {
    cy.mount(AuthCornerComponent, {
      imports: [RouterTestingModule],
      providers: [provideNoopAnimations(), provideHttpClient(), AuthService],
    });
  });

  it('be visible', () => {
    cy.getByAttr('auth-corner').should('be.visible');
  });

  it('contain register button with routerLink', () => {
    cy.getByAttr('register-button')
      .should('be.visible')
      .should('have.attr', 'routerLink');
  });

  it('contain login button with routerLink', () => {
    cy.getByAttr('login-button')
      .should('be.visible')
      .should('have.attr', 'routerLink');
  });
});
