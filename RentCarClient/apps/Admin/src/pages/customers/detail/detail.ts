import Blank from 'apps/Admin/src/components/blank/blank';
import { ChangeDetectionStrategy, Component, computed, effect, inject, signal, ViewEncapsulation } from '@angular/core';
import { httpResource } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { BreadCrumbModel, BreadcrumbService } from 'apps/Admin/src/services/breadcrumb';
import { Result } from 'apps/Admin/src/models/result.model';
import { CommonModule, DatePipe } from '@angular/common';
import { CustomerModel, initialCustomerModel } from 'apps/Admin/src/models/customer.model';

@Component({
  imports: [
    Blank,
    DatePipe
  ],
  templateUrl: './detail.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class CustomerDetail {
  readonly id = signal<string>('');
  readonly bredcrumbs = signal<BreadCrumbModel[]>([]);
  readonly result = httpResource<Result<CustomerModel>>(() => `/rent/customers/${this.id()}`);
  readonly data = computed(() => this.result.value()?.data ?? initialCustomerModel);
  readonly loading = computed(() => this.result.isLoading());
  readonly pageTitle = signal<string>("Müşteri Detay");

  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);

  constructor() {
    this.#activated.params.subscribe(res => {
      this.id.set(res['id']);
    });

    effect(() => {
      const breadCrumbs: BreadCrumbModel[] = [
        {
          title: 'Müşteriler',
          icon: 'bi-people',
          url: '/customers'
        }
      ];

      if (this.data()) {
        this.bredcrumbs.set(breadCrumbs);
        this.bredcrumbs.update(prev => [...prev, {
          title: this.data().fullName,
          icon: 'bi-zoom-in',
          url: `/customers/detail/${this.id()}`,
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
      }
    });
  }
}