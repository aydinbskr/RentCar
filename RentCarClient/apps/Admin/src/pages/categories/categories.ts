import { ChangeDetectionStrategy, Component, inject, signal, ViewEncapsulation } from '@angular/core';
import Grid from '../../components/grid/grid';
import { FlexiGridModule } from 'flexi-grid';
import { Common } from '../../services/common';
import { BreadCrumbModel } from '../../services/breadcrumb';


@Component({
  imports: [
    Grid,
    FlexiGridModule,
  ],
  templateUrl: './categories.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Categories {
  readonly bredcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Kategoriler',
      icon: 'bi-tags',
      url: '/categories',
      isActive: true
    }
  ]);

  readonly #common = inject(Common);

  checkPermission(permission: string){
    return this.#common.checkPermission(permission);
  }
}
