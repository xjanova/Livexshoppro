// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Mobile App
//  Application Entry Point
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

namespace LiveXShopPro.Mobile;

/// <summary>
/// Application Class หลักของแอพมือถือ
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// สร้าง Application Instance
    /// </summary>
    public App()
    {
        InitializeComponent();

        // ตั้งค่าหน้าแรก
        MainPage = new AppShell();
    }
}
