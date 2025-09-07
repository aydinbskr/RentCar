import { AfterViewInit, ChangeDetectionStrategy, Component, computed, contentChild, contentChildren, inject, input, signal, TemplateRef, ViewEncapsulation } from '@angular/core';
import { BreadCrumbModel, BreadcrumbService } from '../../services/breadcrumb';
import { FlexiGridColumnComponent, FlexiGridModule, FlexiGridService, StateModel } from 'flexi-grid';
import { httpResource } from '@angular/common/http';
import { ODataModel } from '../../models/odata.model';
import { FlexiToastService } from 'flexi-toast';
import { HttpService } from '../../services/http';
import { RouterLink } from '@angular/router';
import { NgTemplateOutlet } from '@angular/common';
import { Common } from '../../services/common';

export interface btnOptions{
  url: string;
  permission: string
}

@Component({
  selector: 'grid',
  imports: [
    FlexiGridModule,
    RouterLink,
    NgTemplateOutlet
  ],
  templateUrl: './grid.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Grid implements AfterViewInit {
  readonly pageTitle = input.required<string>();
  readonly endpoint = input.required<string>();
  readonly showAudit = input<boolean>(true);
  readonly addOptions = input.required<btnOptions>();
  readonly editOptions = input.required<btnOptions>();
  readonly detailOptions = input.required<btnOptions>();
  readonly deleteOptions = input.required<btnOptions>();
  readonly breadcrumbs = input.required<BreadCrumbModel[]>();
 
  readonly commandColumnWidth = input<string>("150px");
  readonly showIndex = input<boolean>(true);
  readonly captionTitle = input.required<string>();

  readonly columns = contentChildren(FlexiGridColumnComponent, {descendants: true});
  readonly commandTemplateRef = contentChild<TemplateRef<any>>("commandTemplate");
  readonly columnCommandTemplateRef = contentChild<TemplateRef<any>>('columnCommandTemplate');

  readonly state = signal<StateModel>(new StateModel());
  readonly result = httpResource<ODataModel<any>>(() => {
    debugger;
    let enpoint = this.endpoint() + '?$count=true';
    const part = this.#grid.getODataEndpoint(this.state());
    enpoint += `&${part}`;

    return enpoint;
  });
  readonly data = computed(() => this.result.value()?.value ?? []);
  readonly total = computed(() => this.result.value()?.['@odata.count'] ?? 0);
  readonly loading = computed(() => this.result.isLoading());

  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #grid = inject(FlexiGridService);
  readonly #toast = inject(FlexiToastService);
  readonly #http = inject(HttpService);
  readonly #common = inject(Common);

  ngAfterViewInit(): void {
    this.#breadcrumb.reset(this.breadcrumbs());
  }

  dataStateChange(state: StateModel){
    this.state.set(state);
  }

  delete(id: string){
    this.#toast.showSwal('Sil?', 'Kaydı silmek istiyor musunuz?','Sil', () => {
      this.#http.delete(`${this.deleteOptions().url}/${id}`,res => {
        //toast ile mesaj göster
        this.result.reload();
      });
    })
  }

  checkPermission(permission: string){
    return this.#common.checkPermission(permission);
  }
}
