import { ChangeDetectionStrategy, Component, inject, OnInit, resource, ViewEncapsulation } from '@angular/core';
import { BreadcrumbService } from '../../services/breadcrumb';
import Blank from '../../components/blank/blank';
import { HttpService } from '../../services/http';
import { lastValueFrom } from 'rxjs';

@Component({
  imports: [Blank],
  templateUrl: './dashboard.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Dashboard implements OnInit {
  readonly #breadcrumb = inject(BreadcrumbService);

  readonly #http = inject(HttpService);

  ngOnInit(): void {
    this.#breadcrumb.setDashboard();
  }

}
