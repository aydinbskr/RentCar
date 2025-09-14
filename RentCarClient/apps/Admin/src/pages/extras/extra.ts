import { FlexiGridModule } from 'flexi-grid';
import Grid from '../../components/grid/grid';
import { ExtraModel, initialExtraModel } from '../../models/extra.model';
import { ChangeDetectionStrategy, Component, inject, signal, ViewEncapsulation } from '@angular/core';
import { BreadCrumbModel } from '../../services/breadcrumb';
import { Common } from '../../services/common';

@Component({
  imports: [
    Grid,
    FlexiGridModule,
  ],
  templateUrl: './extra.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Extra {
  readonly bredcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Ekstralar',
      icon: 'bi-plus-square',
      url: '/extra',
      isActive: true
    }
  ]);

  readonly #common = inject(Common);

  checkPermission(permission: string){
    return this.#common.checkPermission(permission);
  }
}
