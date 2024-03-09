import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecipeViewComponent } from './recipe-view.component';

describe('RecipeViewComponent should', () => {
  let component: RecipeViewComponent;
  let fixture: ComponentFixture<RecipeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecipeViewComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(RecipeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('create', () => {
    expect(component).toBeTruthy();
  });
});