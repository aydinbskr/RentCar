import { httpResource } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, effect, inject, signal, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import Blank from 'apps/Admin/src/components/blank/blank';
import { BranchModel, initialBranch } from 'apps/Admin/src/models/branch.model';
import { CategoryModel, initialCategoryModel } from 'apps/Admin/src/models/category.model';
import { Result } from 'apps/Admin/src/models/result.model';
import { BreadCrumbModel, BreadcrumbService } from 'apps/Admin/src/services/breadcrumb';
import { NgxMaskPipe } from 'ngx-mask';


@Component({
  imports: [
    Blank
  ],
  templateUrl: './detail.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class CategoryDetail {
  readonly id = signal<string>('');
  readonly bredcrumbs = signal<BreadCrumbModel[]>([]);
  readonly result = httpResource<Result<CategoryModel>>(() => `/rent/categories/${this.id()}`);
  readonly data = computed(() => this.result.value()?.data ?? initialCategoryModel);
  readonly loading = computed(() => this.result.isLoading());
  readonly pageTitle = signal<string>("Kategori Detay");

  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);

  constructor() {
    this.#activated.params.subscribe(res => {
      this.id.set(res['id']);
    });

    effect(() => {
      const breadCrumbs: BreadCrumbModel[] = [
        {
          title: 'Kategoriler',
          icon: 'bi-tags',
          url: '/categories'
        }
      ];

      if (this.data()) {
        this.bredcrumbs.set(breadCrumbs);
        this.bredcrumbs.update(prev => [...prev, {
          title: this.data().name,
          icon: 'bi-zoom-in',
          url: `/categories/detail/${this.id()}`,
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
      }
    });
  }
}