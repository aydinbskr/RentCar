export interface NavigationModel{
    title: string;
    url?: string;
    icon?: string;
    haveSubNav?: boolean;
    subNavs?: NavigationModel[],
    permission: string;
}

export const navigations: NavigationModel[] = [
    {
        title: "Dashboard",
        url: "/",
        icon: 'bi-speedometer2',
        permission: 'dashboard:view'
    },
    {
        title: 'Şubeler',
        url: '/branches',
        icon: 'bi-buildings',
        permission: 'branch:view'
    },
    {
        title: 'Roller',
        url: '/roles',
        icon: 'bi-clipboard2-check',
        permission: 'role:view'
    },
    {
        title: 'Kategoriler',
        url: '/categories',
        icon: 'bi-tags',
        permission: 'category:view'
    },
    {
        title: 'Koruma Paketleri',
        url: '/protection-packages',
        icon: 'bi-shield-check',
        permission: 'protection_package:view'
    },
    {
        title: 'Kullanıcılar',
        url: '/users',
        icon: 'bi-people',
        permission: 'user:view'
    },
     {
        title: 'Ekstralar',
        url: '/extra',
        icon: 'bi-plus-square',
        permission: 'extra:view'
    },
    {
        title: 'Araçlar',
        url: '/vehicles',
        icon: 'bi-car-front',
        permission: 'vehicle:view'
    },
    {
        title: 'Müşteriler',
        url: '/customers',
        icon: 'bi-person-vcard',
        permission: 'customer:view'
    },
]