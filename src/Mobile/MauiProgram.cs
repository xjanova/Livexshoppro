// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Mobile App
//  แอพมือถือสำหรับดูดแชทจากการไลฟ์ผ่านโทรศัพท์
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using Microsoft.Extensions.Logging;

namespace LiveXShopPro.Mobile;

/// <summary>
/// จุดเริ่มต้นของแอพ MAUI
/// ตั้งค่า Services และ Configuration ต่างๆ
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// สร้างและตั้งค่า MAUI App
    /// </summary>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                // ตั้งค่า Font Sarabun สำหรับภาษาไทย
                fonts.AddFont("Sarabun-Regular.ttf", "SarabunRegular");
                fonts.AddFont("Sarabun-Bold.ttf", "SarabunBold");
                fonts.AddFont("Sarabun-Light.ttf", "SarabunLight");
            });

        // ตั้งค่า Dependency Injection
        ConfigureServices(builder.Services);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    /// <summary>
    /// ลงทะเบียน Services สำหรับ Dependency Injection
    /// </summary>
    private static void ConfigureServices(IServiceCollection services)
    {
        // TODO: เพิ่ม Services เมื่อพัฒนาต่อ
        // services.AddSingleton<IConnectionService, ConnectionService>();
        // services.AddSingleton<IChatCaptureService, ChatCaptureService>();
        // services.AddTransient<MainViewModel>();
    }
}
