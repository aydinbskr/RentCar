import { FlexiGridFilterDataModel, FlexiGridModule } from 'flexi-grid';
import Grid from '../../components/grid/grid';
import { ChangeDetectionStrategy, Component, computed, inject, signal, ViewEncapsulation } from '@angular/core';
import { BreadCrumbModel, BreadcrumbService } from '../../services/breadcrumb';
import { Common } from '../../services/common';
import { NgxMaskPipe } from 'ngx-mask';

@Component({
    imports: [
        Grid,
        FlexiGridModule,
        NgxMaskPipe
    ],
  templateUrl: './customers.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class CustomersPage {
  readonly bredcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Müşteriler',
      icon: 'bi-people',
      url: '/customers',
      isActive: true
    }
  ]);

  constructor() {
    inject(BreadcrumbService).reset(this.bredcrumbs());
  }
}