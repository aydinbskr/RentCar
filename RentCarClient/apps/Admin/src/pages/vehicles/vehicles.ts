import { FlexiGridModule } from 'flexi-grid';
import Grid from '../../components/grid/grid';
import { ChangeDetectionStrategy, Component, inject, signal, ViewEncapsulation } from '@angular/core';
import { BreadCrumbModel } from '../../services/breadcrumb';
import { Common } from '../../services/common';

@Component({
  imports: [
    Grid,
    FlexiGridModule,
  ],
  templateUrl: './vehicles.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Vehicles {
  readonly bredcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Araçlar',
      icon: 'bi-car-front',
      url: '/vehicles',
      isActive: true
    }
  ]);

  readonly #common = inject(Common);

  checkPermission(permission: string){
    return this.#common.checkPermission(permission);
  }
}