import { ChangeDetectionStrategy, Component, inject, signal, ViewEncapsulation } from '@angular/core';
import Grid from '../../components/grid/grid';
import { FlexiGridModule } from 'flexi-grid';
import { RouterLink } from '@angular/router';
import { BreadCrumbModel } from '../../services/breadcrumb';
import { Common } from '../../services/common';

@Component({
  imports: [
    Grid,
    FlexiGridModule
  ],
  templateUrl: './users.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Users {
readonly bredcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Kullanıcılar',
      icon: 'bi-people',
      url: '/users',
      isActive: true
    }
  ]);

  readonly #common = inject(Common);

  checkPermission(permission: string){
    return this.#common.checkPermission(permission);
  }
}
