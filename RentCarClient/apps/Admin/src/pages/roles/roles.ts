import { ChangeDetectionStrategy, Component, inject, signal, ViewEncapsulation } from '@angular/core';
import Grid from '../../components/grid/grid';
import { FlexiGridModule } from 'flexi-grid';
import { BreadCrumbModel } from '../../services/breadcrumb';
import { RouterLink } from '@angular/router';
import { Common } from '../../services/common';

@Component({
  imports: [
    Grid,
    FlexiGridModule,
     RouterLink
  ],
  templateUrl: './roles.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Roles {
  readonly bredcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Roller',
      icon: 'bi-clipboard2-check',
      url: '/roles',
      isActive: true
    }
  ]);

  readonly #common = inject(Common);

  checkPermission(permission: string){
    return this.#common.checkPermission(permission);
  }
}
