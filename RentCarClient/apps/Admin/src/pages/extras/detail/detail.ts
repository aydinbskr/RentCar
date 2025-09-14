import Blank from 'apps/Admin/src/components/blank/blank';
import { ExtraModel, initialExtraModel } from '../../../models/extra.model';
import { TrCurrencyPipe } from 'tr-currency';
import { ChangeDetectionStrategy, Component, computed, effect, inject, signal, ViewEncapsulation } from '@angular/core';
import { httpResource } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { BreadCrumbModel, BreadcrumbService } from 'apps/Admin/src/services/breadcrumb';
import { Result } from 'apps/Admin/src/models/result.model';

@Component({
  imports: [
    Blank,
    TrCurrencyPipe
  ],
  templateUrl: './detail.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class ExtraDetail {
  readonly id = signal<string>('');
  readonly bredcrumbs = signal<BreadCrumbModel[]>([]);
  readonly result = httpResource<Result<ExtraModel>>(() => `/rent/extras/${this.id()}`);
  readonly data = computed(() => this.result.value()?.data ?? initialExtraModel);
  readonly loading = computed(() => this.result.isLoading());
  readonly pageTitle = signal<string>("Ekstra Detay");

  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);

  constructor() {
    this.#activated.params.subscribe(res => {
      this.id.set(res['id']);
    });

    effect(() => {
      const breadCrumbs: BreadCrumbModel[] = [
        {
          title: 'Ekstralar',
          icon: 'bi-plus-square',
          url: '/extra'
        }
      ];

      if (this.data()) {
        this.bredcrumbs.set(breadCrumbs);
        this.bredcrumbs.update(prev => [...prev, {
          title: this.data().name,
          icon: 'bi-zoom-in',
          url: `/extra/detail/${this.id()}`,
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
      }
    });
  }
}
