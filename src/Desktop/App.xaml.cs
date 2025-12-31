// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Application Entry Point
//  ตั้งค่า DI Container และเริ่มต้นแอพพลิเคชัน
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using LiveXShopPro.Desktop.Services;
using LiveXShopPro.Desktop.ViewModels;
using LiveXShopPro.Desktop.Views.Main;
using LiveXShopPro.Infrastructure.Data.Context;
using LiveXShopPro.Core.Interfaces;

namespace LiveXShopPro.Desktop;

/// <summary>
/// Application Entry Point
/// ตั้งค่า DI Container และเริ่มต้นแอพพลิเคชัน
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Host สำหรับ DI Container
    /// </summary>
    private readonly IHost _host;

    /// <summary>
    /// Service Provider สำหรับเข้าถึง Services
    /// </summary>
    public static IServiceProvider Services { get; private set; } = null!;

    /// <summary>
    /// สร้าง App Instance
    /// </summary>
    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services);
            })
            .Build();

        Services = _host.Services;
    }

    /// <summary>
    /// ตั้งค่า Services ทั้งหมดใน DI Container
    /// </summary>
    private void ConfigureServices(IServiceCollection services)
    {
        // ═══════════════════════════════════════════════════════════════════════
        // Database
        // ═══════════════════════════════════════════════════════════════════════
        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LiveXShopPro",
            "data.db");

        // สร้างโฟลเดอร์ถ้ายังไม่มี
        var dbFolder = Path.GetDirectoryName(dbPath);
        if (!string.IsNullOrEmpty(dbFolder) && !Directory.Exists(dbFolder))
        {
            Directory.CreateDirectory(dbFolder);
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        // ═══════════════════════════════════════════════════════════════════════
        // Repositories
        // ═══════════════════════════════════════════════════════════════════════
        // TODO: ลงทะเบียน Repository implementations
        // services.AddScoped<ICustomerRepository, CustomerRepository>();
        // services.AddScoped<IProductRepository, ProductRepository>();
        // services.AddScoped<IOrderRepository, OrderRepository>();
        // services.AddScoped<IUnitOfWork, UnitOfWork>();

        // ═══════════════════════════════════════════════════════════════════════
        // Application Services
        // ═══════════════════════════════════════════════════════════════════════
        services.AddSingleton<INavigationService, NavigationService>();

        // TODO: ลงทะเบียน Application Services
        // services.AddSingleton<ILiveSessionService, LiveSessionService>();
        // services.AddSingleton<IChatCaptureService, ChatCaptureService>();
        // services.AddSingleton<ISlipVerificationService, SlipVerificationService>();
        // services.AddSingleton<IShippingService, ShippingService>();
        // services.AddSingleton<IMobileConnectionService, MobileConnectionService>();
        // services.AddSingleton<ILicenseService, LicenseService>();

        // ═══════════════════════════════════════════════════════════════════════
        // ViewModels
        // ═══════════════════════════════════════════════════════════════════════
        services.AddTransient<MainViewModel>();
        services.AddTransient<DashboardViewModel>();
        // TODO: ลงทะเบียน ViewModels เพิ่มเติม
        // services.AddTransient<LiveViewModel>();
        // services.AddTransient<ChatViewModel>();
        // services.AddTransient<OrdersViewModel>();
        // services.AddTransient<ProductsViewModel>();
        // services.AddTransient<CustomersViewModel>();
        // services.AddTransient<POSViewModel>();
        // services.AddTransient<PaymentsViewModel>();
        // services.AddTransient<SlipVerifyViewModel>();
        // services.AddTransient<ShippingViewModel>();
        // services.AddTransient<PackingViewModel>();
        // services.AddTransient<ReportsViewModel>();
        // services.AddTransient<AnalyticsViewModel>();
        // services.AddTransient<SettingsViewModel>();

        // ═══════════════════════════════════════════════════════════════════════
        // Views (Windows & Pages)
        // ═══════════════════════════════════════════════════════════════════════
        services.AddTransient<MainWindow>();
        // TODO: ลงทะเบียน Pages เพิ่มเติม
        // services.AddTransient<DashboardPage>();
        // services.AddTransient<LivePage>();
        // etc.
    }

    /// <summary>
    /// เริ่มต้นแอพพลิเคชัน
    /// </summary>
    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        // สร้าง/อัพเดท Database
        await InitializeDatabaseAsync();

        // แสดงหน้าต่างหลัก
        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    /// <summary>
    /// สร้างและ Migrate Database
    /// </summary>
    private async Task InitializeDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            // สร้าง Database และ Migrate
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            // ถ้า Migrate ไม่ได้ ให้สร้าง Database ใหม่
            System.Diagnostics.Debug.WriteLine($"Migration failed: {ex.Message}");
            await dbContext.Database.EnsureCreatedAsync();
        }
    }

    /// <summary>
    /// ปิดแอพพลิเคชัน
    /// </summary>
    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
        }

        base.OnExit(e);
    }
}
