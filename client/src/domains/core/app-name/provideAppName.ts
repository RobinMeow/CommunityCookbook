import { EnvironmentProviders, makeEnvironmentProviders } from '@angular/core';
import { APP_NAME } from './APP_NAME';

export function provideAppName(): EnvironmentProviders {
  return makeEnvironmentProviders([
    {
      provide: APP_NAME,
      useValue: 'Geschmack nach Christlicher Art',
    },
  ]);
}
