import { ComponentFixture, TestBed } from '@angular/core/testing'
import { provideNoopAnimations } from '@angular/platform-browser/animations'

import { CreateRecipe } from './create_recipe'
import { provideHttpClientTesting } from '@angular/common/http/testing'
import { provideHttpClient } from '@angular/common/http'
import { RecipeApi } from '../util/recipe_api'
import { provideApiBaseUrlTesting } from '@api'
import { byTestAttr, setValue } from '@common/testing'
import { MockProvider } from 'ng-mocks'
import { ActivatedRoute } from '@angular/router'
import { Validation, ValidationField } from '@common/constraints'
import { fakeSnapshot } from 'src/app/common/testing/fake_snapshot'
import { withResolvedData } from 'src/app/common/testing/with_resolved_data'
import {
  withConstraint,
  fakeValidationField
} from '@common/constraints/testing'

describe('CreateRecipe should', () => {
  let component: CreateRecipe
  let recipeApi: RecipeApi
  let fixture: ComponentFixture<CreateRecipe>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateRecipe],
      providers: [
        provideNoopAnimations(),
        provideHttpClient(),
        provideHttpClientTesting(),
        provideApiBaseUrlTesting(),
        MockProvider(RecipeApi),
        MockProvider(ActivatedRoute, {
          snapshot: fakeSnapshot(
            withResolvedData<ValidationField[]>('validationFields', [
              fakeValidationField('title', 'string', [
                withConstraint(Validation.Min, 1),
                withConstraint(Validation.Max, 2),
                withConstraint(Validation.Required)
              ])
            ])
          )
        })
      ]
    }).compileComponents()

    fixture = TestBed.createComponent(CreateRecipe)
    component = fixture.componentInstance
    fixture.detectChanges()
    recipeApi = TestBed.inject(RecipeApi)
  })

  it('create', () => {
    expect(component).toBeTruthy()
  })

  it('render title', () => {
    expect(byTestAttr(fixture, 'title')).toBeTruthy()
  })

  it('render form', () => {
    expect(byTestAttr(fixture, 'create-recipe-form')).toBeTruthy()
  })

  it('render recipe-title-input', () => {
    expect(byTestAttr(fixture, 'recipe-title-input')).toBeTruthy()
  })

  it('render submit button', () => {
    expect(byTestAttr(fixture, 'create-recipe-submit-button')).toBeTruthy()
  })

  it('have disabled submit button by default', () => {
    const btn = byTestAttr<HTMLButtonElement>(
      fixture,
      'create-recipe-submit-button'
    )
    expect(btn.disabled).toBeTrue()
  })

  it('enable submit button after valid inputs', () => {
    const btn = byTestAttr<HTMLButtonElement>(
      fixture,
      'create-recipe-submit-button'
    )
    expect(btn).toBeTruthy()
    expect(btn.disabled).toBeTrue()

    const input = byTestAttr<HTMLInputElement>(fixture, 'recipe-title-input')
    setValue(input, 'A')
    fixture.detectChanges()

    expect(btn.disabled).toBeFalse()
  })

  it('not send http request when form is submitted invalid', async () => {
    const form = byTestAttr<HTMLFormElement>(fixture, 'create-recipe-form')
    expect(form.querySelector('[invalid]')).toBeDefined()
    expect(component['form'].invalid).toBeTrue()

    const addAsyncSpy = spyOn(recipeApi, 'newAsync')

    const submitButton = byTestAttr<HTMLButtonElement>(
      fixture,
      'create-recipe-submit-button'
    )

    expect(submitButton.disabled).toBe(true)

    submitButton.click()
    expect(addAsyncSpy).not.toHaveBeenCalled()
  })
})
