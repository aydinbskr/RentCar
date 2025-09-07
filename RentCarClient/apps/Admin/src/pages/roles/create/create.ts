import { NgClass } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, inject, linkedSignal, resource, signal, ViewEncapsulation } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import Blank from 'apps/Admin/src/components/blank/blank';
import { BranchModel, initialBranch } from 'apps/Admin/src/models/branch.model';
import { BreadCrumbModel, BreadcrumbService } from 'apps/Admin/src/services/breadcrumb';
import { HttpService } from 'apps/Admin/src/services/http';
import { FlexiToastService } from 'flexi-toast';
import { FormValidateDirective } from 'form-validate-angular';
import { NgxMaskDirective } from 'ngx-mask';
import { lastValueFrom } from 'rxjs';

@Component({
  imports: [
    Blank,
    FormsModule,
    FormValidateDirective,
    NgClass
  ],
  templateUrl: './create.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Create {
  readonly id = signal<string | undefined>(undefined);
  readonly bredcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Roller',
      icon: 'bi-buildings',
      url: '/roles'
    }
  ]);
  readonly pageTitle = computed(() => this.id() ? 'Rol Güncelle' : 'Rol Ekle');
  readonly pageIcon = computed(() => this.id() ? 'bi-pen' : 'bi-plus');
  readonly btnName = computed(() => this.id() ? 'Güncelle' : 'Kaydet');

  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      var res = await lastValueFrom(this.#http.getResource<BranchModel>(`/rent/roles/${this.id()}`));

      this.bredcrumbs.update(prev => [...prev, {
          title: res.data!.name,
          icon: 'bi-pen',
          url: `/roles/edit/${this.id()}`,
          isActive: true
      }]);
      this.#breadcrumb.reset(this.bredcrumbs());
      return res.data;
    }
  });
  readonly data = linkedSignal(() => this.result.value() ?? {...initialBranch});
  readonly loading = linkedSignal(() => this.result.isLoading());

  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #activated = inject(ActivatedRoute);
  readonly #http = inject(HttpService);
  readonly #toast = inject(FlexiToastService);
  readonly #router = inject(Router);

  constructor() {
    this.#activated.params.subscribe(res => {
      if (res['id']) {
        this.id.set(res['id']);
      } else {
        this.bredcrumbs.update(prev => [...prev, {
          title: 'Ekle',
          icon: 'bi-plus',
          url: '/roles/add',
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
      }

    })
  }

  save(form: NgForm){
    if(!form.valid) return;

    if(!this.id()){
      this.loading.set(true);
      this.#http.post<string>('/rent/roles', this.data(), (res) => {
        this.#toast.showToast("Başarılı",res,"success");
        this.#router.navigateByUrl("/roles");
        this.loading.set(false);
      },() => this.loading.set(false));
    }else{
      this.loading.set(true);
      this.#http.put<string>('/rent/roles', this.data(), (res) => {
        this.#toast.showToast("Başarılı",res,"info");
        this.#router.navigateByUrl("/roles");
        this.loading.set(false);
      },() => this.loading.set(false));
    }
  }

  changeStatus(status:boolean){
    this.data.update(prev => ({
      ...prev,
      isActive: status
    }));

    console.log(this.data());
  }
}