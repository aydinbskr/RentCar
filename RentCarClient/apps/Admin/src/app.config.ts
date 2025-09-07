import {
  ApplicationConfig,
  LOCALE_ID,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { appRoutes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { httpInterceptor } from './interceptors/http-interceptor';
import { authInterceptor } from './interceptors/auth-interceptor';
import { provideNgxMask } from 'ngx-mask';
import localeTr from '@angular/common/locales/tr'
import { registerLocaleData } from '@angular/common';

registerLocaleData(localeTr);

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(appRoutes),
    provideNgxMask(),
    provideHttpClient(withInterceptors([httpInterceptor,authInterceptor])),
    { provide: LOCALE_ID, useValue: 'tr-TR'}
  ],
};
