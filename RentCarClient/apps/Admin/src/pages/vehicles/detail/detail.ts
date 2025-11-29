import Blank from 'apps/Admin/src/components/blank/blank';
import { ExtraModel, initialExtraModel } from '../../../models/extra.model';
import { TrCurrencyPipe } from 'tr-currency';
import { ChangeDetectionStrategy, Component, computed, effect, inject, signal, ViewEncapsulation } from '@angular/core';
import { httpResource } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { BreadCrumbModel, BreadcrumbService } from 'apps/Admin/src/services/breadcrumb';
import { Result } from 'apps/Admin/src/models/result.model';
import { CommonModule } from '@angular/common';
import { initialVehicleModel, VehicleModel } from 'apps/Admin/src/models/vehicle.model';
import { FeatureGroup } from '../create/create';

@Component({
  imports: [
    Blank,
    CommonModule,
    TrCurrencyPipe
  ],
  templateUrl: './detail.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class VehicleDetail {
  readonly id = signal<string>('');
  readonly bredcrumbs = signal<BreadCrumbModel[]>([]);
  readonly result = httpResource<Result<VehicleModel>>(() => `/rent/vehicles/${this.id()}`);
  readonly data = computed(() => this.result.value()?.data ?? initialVehicleModel);
  readonly loading = computed(() => this.result.isLoading());
  readonly pageTitle = signal<string>("Araç Detay");

  readonly featureGroups: FeatureGroup[] = [
      {
        group: 'Güvenlik Özellikleri',
        features: [
          { key: 'Airbag', label: 'Airbag', icon: 'bi-shield' },
          { key: 'ABS', label: 'ABS', icon: 'bi-shield-exclamation' },
          { key: 'ESP', label: 'ESP', icon: 'bi-shield-check' },
          { key: 'Alarm Sistemi', label: 'Alarm Sistemi', icon: 'bi-bell' }
        ]
      },
      {
        group: 'Sürüş Destekleri',
        features: [
          { key: 'GPS Navigasyon', label: 'GPS Navigasyon', icon: 'bi-geo-alt' },
          { key: 'Park Sensörü', label: 'Park Sensörü', icon: 'bi-broadcast-pin' },
          { key: 'Geri Görüş Kamerası', label: 'Geri Görüş Kamerası', icon: 'bi-camera-video' },
          { key: 'Cruise Control', label: 'Cruise Control', icon: 'bi-speedometer2' }
        ]
      },
      {
        group: 'Konfor Özellikleri',
        features: [
          { key: 'Klima', label: 'Klima', icon: 'bi-snow' },
          { key: 'Isıtmalı Koltuk', label: 'Isıtmalı Koltuk', icon: 'bi-thermometer-half' },
          { key: 'Sunroof', label: 'Sunroof', icon: 'bi-brightness-high' },
          { key: 'Bluetooth', label: 'Bluetooth', icon: 'bi-bluetooth' }
        ]
      },
      {
        group: 'Multimedya',
        features: [
          { key: 'Dokunmatik Ekran', label: 'Dokunmatik Ekran', icon: 'bi-tablet' },
          { key: 'USB Bağlantısı', label: 'USB Bağlantısı', icon: 'bi-usb' },
          { key: 'Premium Ses Sistemi', label: 'Premium Ses Sistemi', icon: 'bi-music-note-beamed' },
          { key: 'Apple CarPlay', label: 'Apple CarPlay', icon: 'bi-phone' }
        ]
      }
    ];
    
  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);

  constructor() {
    this.#activated.params.subscribe(res => {
      this.id.set(res['id']);
    });

    effect(() => {
      const breadCrumbs: BreadCrumbModel[] = [
        {
          title: 'Araçlar',
          icon: 'bi-car-front',
          url: '/vehicles'
        }
      ];

      if (this.data()) {
        this.bredcrumbs.set(breadCrumbs);
        this.bredcrumbs.update(prev => [
          ...prev,
          {
            title: this.data().brand + ' ' + this.data().model,
            icon: 'bi-zoom-in',
            url: `/vehicles/detail/${this.id()}`,
            isActive: true
          }
        ]);
        this.#breadcrumb.reset(this.bredcrumbs());
      }
    });
  }

  showImageUrl(){
    if(this.data().imageUrl){
        return `https://localhost:7104/images/${this.data().imageUrl}`
    }else{
        return '/no-noimage.png'
    }
  }

isFeatureSelected(feature: string): boolean {
    return this.data().features.includes(feature);
  }
}