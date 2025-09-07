import { ChangeDetectionStrategy, Component, signal, ViewEncapsulation } from '@angular/core';
import { FlexiGridModule } from 'flexi-grid';
import { NgxMaskPipe } from 'ngx-mask';
import { BreadCrumbModel } from '../../services/breadcrumb';
import Grid from '../../components/grid/grid';

@Component({
  imports: [
    Grid,
    FlexiGridModule,
    NgxMaskPipe
  ],
  templateUrl: './branches.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Branches {
  readonly bredcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Şubeler',
      icon: 'bi-buildings',
      url: '/branches',
      isActive: true
    }
  ]);

  
}
