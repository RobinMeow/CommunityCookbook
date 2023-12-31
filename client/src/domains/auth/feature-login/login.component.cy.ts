import { provideHttpClient } from '@angular/common/http';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { LoginComponent } from './login.component';
import { AuthService } from '../utils/auth.service';

describe('login should', () => {
  beforeEach('mount', () => {
    cy.mount(LoginComponent, {
      providers: [provideNoopAnimations(), provideHttpClient(), AuthService],
    });
  });

  it('have visible title', () => {
    cy.getByAttr('title').should('be.visible');
  });

  it('have disabled submit button', () => {
    cy.getByAttr('login-submit-button').should('be.disabled');
  });

  it('have enabled submit button when form is filled out', () => {
    cy.getByAttr('login-name-input').type('Weinberg des Herrn');
    cy.getByAttr('password-input').type('iLoveJesus<3');
    cy.getByAttr('login-submit-button').should('be.enabled');
  });
});
