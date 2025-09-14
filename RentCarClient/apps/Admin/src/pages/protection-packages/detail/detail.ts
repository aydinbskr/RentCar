import { ChangeDetectionStrategy, Component, computed, effect, inject, resource, signal, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { httpResource } from '@angular/common/http';
import Blank from 'apps/Admin/src/components/blank/blank';
import { ProtectionPackageModel, initialProtectionPackageModel } from 'apps/Admin/src/models/protection-package.model';
import { Result } from 'apps/Admin/src/models/result.model';
import { BreadCrumbModel, BreadcrumbService } from 'apps/Admin/src/services/breadcrumb';


@Component({
  imports: [
    Blank
  ],
  templateUrl: './detail.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class ProtectionPackageDetail {
  readonly id = signal<string>('');
  readonly bredcrumbs = signal<BreadCrumbModel[]>([]);
  readonly result = httpResource<Result<ProtectionPackageModel>>(() => `/rent/protection-packages/${this.id()}`);
  readonly data = computed(() => this.result.value()?.data ?? initialProtectionPackageModel);
  readonly loading = computed(() => this.result.isLoading());
  readonly pageTitle = computed(() => this.data().name);

  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);

  constructor() {
    this.#activated.params.subscribe(res => {
      this.id.set(res['id']);
    });

    effect(() => {
      const breadCrumbs: BreadCrumbModel[] = [
        {
          title: 'Koruma Paketleri',
          icon: 'bi-shield-check',
          url: '/protection-packages'
        }
      ];

      if (this.data()) {
        this.bredcrumbs.set(breadCrumbs);
        this.bredcrumbs.update(prev => [...prev, {
          title: this.data().name,
          icon: 'bi-zoom-in',
          url: `/protection-packages/detail/${this.id()}`,
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
      }
    });
  }
}