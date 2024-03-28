import { TestBed } from '@angular/core/testing'
import { App } from './app'
import { provideHttpClientTesting } from '@angular/common/http/testing'
import { provideHttpClient } from '@angular/common/http'
import { provideAppName } from 'src/app/core'
import { provideNoopAnimations } from '@angular/platform-browser/animations'
import { provideApiBaseUrlTesting } from './shared/api/provideApiBaseUrl'
import { provideRouter } from '@angular/router'

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [provideRouter([]), App],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideAppName(),
        provideNoopAnimations(),
        provideApiBaseUrlTesting()
      ]
    }).compileComponents()
  })

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App)
    const app = fixture.componentInstance
    expect(app).toBeTruthy()
  })
})
