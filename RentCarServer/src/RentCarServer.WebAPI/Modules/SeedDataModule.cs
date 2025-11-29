using GenericRepository;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.Categories;
using RentCarServer.Domain.Extras;
using RentCarServer.Domain.ProtectionPackages;
using RentCarServer.Domain.Vehicles;
using TS.Result;

namespace RentCarServer.WebAPI.Modules
{
    public static class SeedDataModule
    {
        public static void MapSeedData(this IEndpointRouteBuilder builder)
        {
            var app = builder.MapGroup("seed-data").RequireAuthorization().WithTags("SeedData");

            //categories
            app.MapGet("categories",
                async (ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
                {
                    var categoryNames = await categoryRepository.GetAll().Select(s => s.Name).ToListAsync(cancellationToken);

                    List<Category> newCategories = new()
                    {
                    new("Binek", true),
                    new("Station Wagon", true),
                    new("Minibüs", true),
                    new("SUV", true),
                    new("Üstü Açık", true),
                    new("MPV", true)
                    };

                    var list = newCategories.Where(p => !categoryNames.Contains(p.Name)).ToList();
                    categoryRepository.AddRange(list);
                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    return Results.Ok(Result<string>.Succeed("Kategori seed data başarıyla tamamlandı"));
                })
                .Produces<Result<string>>();

            //protection-packages
            app.MapGet("protection-packages",
                async (IProtectionPackageRepository repository, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
                {
                    var existingNames = await repository.GetAll().Select(p => p.Name).ToListAsync(cancellationToken);

                    var packages = new List<ProtectionPackage>
                    {
                    new(
                        "Mini Güvence Paketi",
                        150,
                        false,
                        1,
                        new List<ProtectionCoverage>
                        {
                            new("Hasar Sorumluluk Güvencesi (CDW)"),
                            new("Hırsızlık Güvencesi (TP)")
                        },
                        true),

                    new(
                        "Standart Güvence Paketi",
                        250,
                        true,
                        2,
                        new List<ProtectionCoverage>
                        {
                            new("Önceki Paketin Tüm Özellikleri Dahil"),
                            new("Mini Pakete Ek: Lastik, Cam, Far Güvencesi (TWH)"),
                            new("Mini Hasar Güvencesi (MI)")
                        },
                        true),

                    new(
                        "Full Güvence Paketi",
                        350,
                        false,
                        3,
                        new List<ProtectionCoverage>
                        {
                            new("Önceki Paketin Tüm Özellikleri Dahil"),
                            new("Standart Pakete Ek: Ek Sürücü"),
                            new("Genç Sürücü")
                        },
                        true),
                    };

                    var newPackages = packages.Where(p => !existingNames.Contains(p.Name)).ToList();
                    repository.AddRange(newPackages);
                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    return Results.Ok(Result<string>.Succeed("Koruma paketi seed data başarıyla tamamlandı"));
                })
                .Produces<Result<string>>();

            //extras
            app.MapGet("extras",
                async (IExtraRepository repository, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
                {
                    var existingNames = await repository.GetAll().Select(e => e.Name).ToListAsync(cancellationToken);

                    List<Extra> extras = new()
                    {
                    // Önerilen Güvenceler
                    new("Mini Hasar Güvencesi", 114, "Ehliyeti ibrazı, kiralama ön şartlarının kabulü (findeksiz, depozito hariç) ve araç teslimi sırasında ek sürücünün de ofiste bizzat bulunması gereklidir.", true),
                    new("Kış Lastiği", 246, "Kış Lastiği (stoklarla sınırlıdır)", true),

                    // Ek Sürücü Paketi
                    new("Genç Sürücü Paketi", 530, "Yaş grubunuzun üst yaş grubundaki aracı kiralayabilmenizi sağlamaktadır.", true),
                    new("Banka Kartı ile Kiralama", 3193, "Kiralama koşulları geçerli Banka Kartı ile kiralamalar için istenilen müşteriler bu ürünü satın alarak araç kiralamaya devam edebilir.", true),
                    new("Depozitosuz Kiralama", 1064, "Depozito ödemek istemeyen müşteriler bu ürünle araç kiralayabilir. Kontrat sonunda çalınmaması kaydıyla depozito gibi talep edilmez.", true),

                    // Koltuk Adaptörü
                    new("Koltuk Adaptörü", 290, "4 yaşından sonra (15–36 kg.) arası çocuklar için arka koltuk yükseltici koltuklar kullanılmalıdır.", true),
                    new("Çocuk Koltuğu", 290, "4 yaşına kadar (9–18 kg.) çocuk güvenlik koltuğu araçta monte edilir.", true),
                    new("Bebek Koltuğu", 290, "0 yaş (0 kilo) bebekler için, arka koltuğa monte edilen ana kucağı modelidir.", true),
                    };

                    var newExtras = extras.Where(e => !existingNames.Contains(e.Name)).ToList();
                    repository.AddRange(newExtras);
                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    return Results.Ok(Result<string>.Succeed("Extra seed data başarıyla tamamlandı"));
                })
                .Produces<Result<string>>();

            //vehicles
            app.MapGet("vehicles",
                async (
                    IVehicleRepository vehicleRepository,
                    ICategoryRepository categoryRepository,
                    IUnitOfWork unitOfWork,
                    CancellationToken cancellationToken) =>
                {
                    var branchId = Guid.Parse("0197de0b-7613-7846-af49-b5a2cc121576");
                    var existingPlates = await vehicleRepository.GetAll().Select(v => v.Plate).ToListAsync(cancellationToken);
                    var categories = await categoryRepository.GetAll().ToListAsync(cancellationToken);

                    var imageUrls = new[]
                    {
                    "citroen-c3.jpg",
                    "fiat-egea-sedan.jpg",
                    "fiat-fiorino.jpg",
                    "renault-clio.jpg",
                    "hyundai-bayon.jpg",
                    "renault-megane-sedan.jpg",
                    "suzuki-vitara.jpg"
                    };

                    var brands = new[] { "Toyota", "Volkswagen", "Renault", "Ford", "Hundai", "Peugeot", "Opel", "Honda", "BMW" };
                    var models = new[] { "C3", "Egea", "Fiorino", "Clio", "Bayon", "Megane", "Vitara" };
                    var allFeatures = new[]
                    {
                    "Airbag", "ABS", "ESP", "Alarm Sistemi",
                    "GPS Navigasyon", "Park Sensörü", "Geri Görüş Kamerası", "Cruise Control",
                    "Klima", "Isıtmalı Koltuk", "Sunroof", "Bluetooth",
                    "Dokunmatik Ekran", "USB Bağlantısı", "Premium Ses Sistemi", "Apple CarPlay"
                    };
                    var vehicles = new List<Vehicle>();
                    var random = new Random();
                    int imageIndex = 0;

                    foreach (var category in categories)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            var featureCount = new Random().Next(4, 9); // 4-8 arası özellik
                            var selectedFeatures = allFeatures.OrderBy(_ => new Random().Next()).Take(featureCount).ToList();
                            var features = selectedFeatures.Select(f => new Feature(f)).ToList();
                            var brand = brands[random.Next(brands.Length)];
                            var model = models[random.Next(models.Length)];
                            var plate = $"34{brand[..2].ToUpper()}{category.Name[..1].ToUpper()}{i + 1}";

                            if (existingPlates.Contains(plate))
                                continue;

                            var vehicle = new Vehicle(
                                brand,
                                $"{model} {category.Name[..2].ToUpper()}",
                                2022 + i % 2,
                                "Gri",
                                plate,
                                category.Id,
                                branchId,
                                $"VIN{category.Name[..1].ToUpper()}{i}{Guid.NewGuid():N}".Substring(0, 16),
                                $"ENG{i}{Guid.NewGuid():N}".Substring(0, 12),
                                $"{brand} {model} açıklaması",
                                imageUrls[imageIndex++ % imageUrls.Length],
                                i % 2 == 0 ? "Benzin" : "Dizel",
                                i % 2 == 0 ? "Otomatik" : "Manuel",
                                1.4m + i * 0.1m,
                                100 + i * 10,
                                "Önden Çekiş",
                                5.5m + i * 0.3m,
                                5,
                                10000 + i * 3000,
                                1800 + i * 50,
                                10,
                                15,
                                "Kasko & Sigorta",
                                DateOnly.FromDateTime(DateTime.Now.AddMonths(-3)),
                                15000 + i * 1000,
                                20000 + i * 1000,
                                DateOnly.FromDateTime(DateTime.Now.AddMonths(9)),
                                DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
                                DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
                                "İyi",
                                "Çok İyi",
                                features,
                                true
                            );

                            vehicles.Add(vehicle);
                        }
                    }

                    vehicleRepository.AddRange(vehicles);
                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    return Results.Ok(Result<string>.Succeed("Tüm araçlar başarıyla eklendi"));
                })
                .Produces<Result<string>>();

        }
    }
}
