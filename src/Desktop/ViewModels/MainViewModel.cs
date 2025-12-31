// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Main ViewModel
//  ViewModel หลักสำหรับ MainWindow
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LiveXShopPro.Desktop.ViewModels;

/// <summary>
/// ViewModel หลักสำหรับ MainWindow
/// จัดการ Navigation, Live Status, และข้อมูลหลักของแอพ
/// </summary>
public partial class MainViewModel : BaseViewModel
{
    #region Observable Properties

    /// <summary>
    /// หน้าที่เลือกอยู่ใน Navigation
    /// </summary>
    [ObservableProperty]
    private string _selectedPage = "Dashboard";

    /// <summary>
    /// สถานะ Live อยู่หรือไม่
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(LiveStatusText))]
    [NotifyPropertyChangedFor(nameof(LiveStatusColor))]
    private bool _isLive;

    /// <summary>
    /// สถานะเชื่อมต่อ Mobile อยู่หรือไม่
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MobileStatusText))]
    private bool _isMobileConnected;

    /// <summary>
    /// จำนวนออเดอร์วันนี้
    /// </summary>
    [ObservableProperty]
    private int _todayOrderCount;

    /// <summary>
    /// จำนวนออเดอร์รอแพ็ค
    /// </summary>
    [ObservableProperty]
    private int _pendingPackCount;

    /// <summary>
    /// ยอดขายวันนี้
    /// </summary>
    [ObservableProperty]
    private decimal _todaySales;

    /// <summary>
    /// จำนวน CF ที่รอดำเนินการ
    /// </summary>
    [ObservableProperty]
    private int _pendingCFCount;

    /// <summary>
    /// จำนวนการแจ้งเตือน
    /// </summary>
    [ObservableProperty]
    private int _notificationCount;

    /// <summary>
    /// สถานะ License
    /// </summary>
    [ObservableProperty]
    private string _licenseType = "Pro";

    /// <summary>
    /// วันหมดอายุ License
    /// </summary>
    [ObservableProperty]
    private DateTime? _licenseExpiry;

    /// <summary>
    /// Version ของโปรแกรม
    /// </summary>
    [ObservableProperty]
    private string _appVersion = "1.0.0";

    #endregion

    #region Computed Properties

    /// <summary>
    /// ข้อความสถานะ Live
    /// </summary>
    public string LiveStatusText => IsLive ? "กำลัง Live" : "ไม่ได้ Live";

    /// <summary>
    /// สี สถานะ Live (ใช้เป็น key สำหรับ binding)
    /// </summary>
    public string LiveStatusColor => IsLive ? "Success" : "Error";

    /// <summary>
    /// ข้อความสถานะ Mobile
    /// </summary>
    public string MobileStatusText => IsMobileConnected ? "เชื่อมต่อแล้ว" : "ไม่ได้เชื่อมต่อ";

    /// <summary>
    /// ยอดขายวันนี้แบบ Format
    /// </summary>
    public string TodaySalesFormatted => $"฿{TodaySales:N0}";

    /// <summary>
    /// วันหมดอายุ License แบบ Format
    /// </summary>
    public string LicenseExpiryFormatted => LicenseExpiry?.ToString("d MMM yyyy",
        new System.Globalization.CultureInfo("th-TH")) ?? "ไม่จำกัด";

    #endregion

    /// <summary>
    /// สร้าง MainViewModel Instance
    /// </summary>
    public MainViewModel()
    {
        Title = "Live x Shop Pro";

        // TODO: โหลดข้อมูลจาก Service
        LoadDemoData();
    }

    /// <summary>
    /// โหลดข้อมูล Demo สำหรับ Preview
    /// </summary>
    private void LoadDemoData()
    {
        TodayOrderCount = 0;
        PendingPackCount = 0;
        TodaySales = 0;
        PendingCFCount = 0;
        NotificationCount = 3;
        LicenseExpiry = new DateTime(2568, 12, 31); // พ.ศ. 2568
    }

    #region Commands

    /// <summary>
    /// เริ่มเซสชันไลฟ์
    /// </summary>
    [RelayCommand]
    private async Task StartLiveAsync()
    {
        await ExecuteAsync(async () =>
        {
            // TODO: เริ่ม Live Session จริง
            IsLive = true;
            await Task.Delay(100); // Placeholder
        }, "ไม่สามารถเริ่ม Live ได้");
    }

    /// <summary>
    /// หยุดเซสชันไลฟ์
    /// </summary>
    [RelayCommand]
    private async Task StopLiveAsync()
    {
        await ExecuteAsync(async () =>
        {
            // TODO: หยุด Live Session จริง
            IsLive = false;
            await Task.Delay(100); // Placeholder
        }, "ไม่สามารถหยุด Live ได้");
    }

    /// <summary>
    /// สลับสถานะ Live
    /// </summary>
    [RelayCommand]
    private async Task ToggleLiveAsync()
    {
        if (IsLive)
            await StopLiveAsync();
        else
            await StartLiveAsync();
    }

    /// <summary>
    /// โหลดข้อมูลสถิติใหม่
    /// </summary>
    [RelayCommand]
    private async Task RefreshStatsAsync()
    {
        await ExecuteAsync(async () =>
        {
            // TODO: โหลดสถิติจาก Database
            await Task.Delay(500); // Placeholder

            // Simulate data update
            TodayOrderCount = new Random().Next(0, 50);
            PendingPackCount = new Random().Next(0, 20);
            TodaySales = new Random().Next(0, 50000);
        }, "ไม่สามารถโหลดข้อมูลได้");
    }

    /// <summary>
    /// เปิดหน้าตั้งค่า
    /// </summary>
    [RelayCommand]
    private void OpenSettings()
    {
        SelectedPage = "Settings";
        // TODO: Navigate to Settings
    }

    /// <summary>
    /// แสดงการแจ้งเตือน
    /// </summary>
    [RelayCommand]
    private void ShowNotifications()
    {
        // TODO: แสดง Notification Flyout
    }

    #endregion
}
