import { provideNoopAnimations } from '@angular/platform-browser/animations'
import { PasswordInput } from './password.input'
import { FormControl } from '@angular/forms'
import { signal } from '@angular/core'

function getPasswordInput() {
  return cy.getByAttr('password-input')
}

function getEyeToggleButton() {
  return cy.getByAttr('password-eye-toggle')
}

describe('password should', () => {
  beforeEach('mount', () => {
    cy.mount(PasswordInput, {
      providers: [provideNoopAnimations()],
      componentProperties: {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        passwordControl: signal(new FormControl('')) as any
      }
    })
  })

  it('render', () => {
    getPasswordInput().should('be.visible')
    getEyeToggleButton().should('be.visible')
  })

  it('toggle text visibility on eye-toggle', () => {
    getPasswordInput()
      .as('password-input')
      .should('have.attr', 'type')
      .should('equal', 'password')

    // this is only for the screenshot, and not required for this test.
    cy.get('@password-input').type('iLoveJesus<3!')

    getEyeToggleButton().click()

    cy.get('@password-input')
      .should('have.attr', 'type')
      .should('equal', 'text')
  })
})