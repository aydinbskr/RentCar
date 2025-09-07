import { httpResource } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, effect, inject, signal, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import Blank from 'apps/Admin/src/components/blank/blank';
import { BranchModel, initialBranch } from 'apps/Admin/src/models/branch.model';
import { Result } from 'apps/Admin/src/models/result.model';
import { BreadCrumbModel, BreadcrumbService } from 'apps/Admin/src/services/breadcrumb';
import { NgxMaskPipe } from 'ngx-mask';

@Component({
  imports: [
    Blank,
    NgxMaskPipe
  ],
  templateUrl: './detail.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Detail {
  readonly id = signal<string>('');
  readonly bredcrumbs = signal<BreadCrumbModel[]>([]);
  readonly result = httpResource<Result<BranchModel>>(() => `/rent/branches/${this.id()}`);
  readonly data = computed(() => this.result.value()?.data ?? initialBranch);
  readonly loading = computed(() => this.result.isLoading());
  readonly pageTitle = computed(() => this.data().name);

  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);

  constructor(){
    this.#activated.params.subscribe(res => {
      this.id.set(res['id']);
    });

    effect(() => {
      const breadCrumbs: BreadCrumbModel[] = [
        {
          title: 'Şubeler',
          icon: 'bi-buildings',
          url: '/branches'
        }
      ]

      if(this.data()){
        this.bredcrumbs.set(breadCrumbs);
        this.bredcrumbs.update(prev => [...prev, {
          title: this.data().name,
          icon: 'bi-zoom-in',
          url: `/branches/detail/${this.id()}`,
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
      }
    })
  }
}