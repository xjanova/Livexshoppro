// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Dashboard ViewModel
//  ViewModel สำหรับหน้า Dashboard แสดงภาพรวมการขาย
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace LiveXShopPro.Desktop.ViewModels;

/// <summary>
/// ViewModel สำหรับหน้า Dashboard
/// </summary>
public partial class DashboardViewModel : BaseViewModel
{
    #region Observable Properties

    // ═══════════════════════════════════════════════════════════════════════
    // สถิติหลัก
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// ยอดขายวันนี้
    /// </summary>
    [ObservableProperty]
    private decimal _todaySales;

    /// <summary>
    /// ยอดขายเมื่อวาน (สำหรับเปรียบเทียบ)
    /// </summary>
    [ObservableProperty]
    private decimal _yesterdaySales;

    /// <summary>
    /// จำนวนออเดอร์วันนี้
    /// </summary>
    [ObservableProperty]
    private int _todayOrders;

    /// <summary>
    /// จำนวนออเดอร์เมื่อวาน
    /// </summary>
    [ObservableProperty]
    private int _yesterdayOrders;

    /// <summary>
    /// จำนวนลูกค้าใหม่วันนี้
    /// </summary>
    [ObservableProperty]
    private int _newCustomersToday;

    /// <summary>
    /// มูลค่าเฉลี่ยต่อออเดอร์
    /// </summary>
    [ObservableProperty]
    private decimal _averageOrderValue;

    // ═══════════════════════════════════════════════════════════════════════
    // สถานะงาน
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// จำนวน CF รอดำเนินการ
    /// </summary>
    [ObservableProperty]
    private int _pendingCF;

    /// <summary>
    /// จำนวนออเดอร์รอยืนยัน
    /// </summary>
    [ObservableProperty]
    private int _pendingConfirmation;

    /// <summary>
    /// จำนวนสลิปรอตรวจ
    /// </summary>
    [ObservableProperty]
    private int _pendingSlips;

    /// <summary>
    /// จำนวนออเดอร์รอแพ็ค
    /// </summary>
    [ObservableProperty]
    private int _pendingPacking;

    /// <summary>
    /// จำนวนพัสดุรอส่ง
    /// </summary>
    [ObservableProperty]
    private int _pendingShipment;

    // ═══════════════════════════════════════════════════════════════════════
    // Live Session
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// กำลัง Live อยู่หรือไม่
    /// </summary>
    [ObservableProperty]
    private bool _isLive;

    /// <summary>
    /// ชื่อ Live Session ปัจจุบัน
    /// </summary>
    [ObservableProperty]
    private string? _currentLiveTitle;

    /// <summary>
    /// Platform ที่กำลัง Live
    /// </summary>
    [ObservableProperty]
    private string? _currentLivePlatform;

    /// <summary>
    /// ระยะเวลา Live
    /// </summary>
    [ObservableProperty]
    private TimeSpan _liveDuration;

    /// <summary>
    /// จำนวนแชททั้งหมดใน Live
    /// </summary>
    [ObservableProperty]
    private int _liveTotalChat;

    /// <summary>
    /// จำนวน CF ใน Live
    /// </summary>
    [ObservableProperty]
    private int _liveCFCount;

    // ═══════════════════════════════════════════════════════════════════════
    // ออเดอร์ล่าสุด
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// รายการออเดอร์ล่าสุด
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<RecentOrderItem> _recentOrders = new();

    // ═══════════════════════════════════════════════════════════════════════
    // สินค้าขายดี
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// สินค้าขายดี
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<TopProductItem> _topProducts = new();

    #endregion

    #region Computed Properties

    /// <summary>
    /// ยอดขายวันนี้แบบ Format
    /// </summary>
    public string TodaySalesFormatted => $"฿{TodaySales:N0}";

    /// <summary>
    /// % เปลี่ยนแปลงจากเมื่อวาน
    /// </summary>
    public decimal SalesChangePercent => YesterdaySales > 0
        ? Math.Round((TodaySales - YesterdaySales) / YesterdaySales * 100, 1)
        : 0;

    /// <summary>
    /// ยอดขายเพิ่มขึ้นหรือไม่
    /// </summary>
    public bool IsSalesUp => SalesChangePercent >= 0;

    /// <summary>
    /// % เปลี่ยนแปลงออเดอร์จากเมื่อวาน
    /// </summary>
    public decimal OrderChangePercent => YesterdayOrders > 0
        ? Math.Round((decimal)(TodayOrders - YesterdayOrders) / YesterdayOrders * 100, 1)
        : 0;

    /// <summary>
    /// มูลค่าเฉลี่ยต่อออเดอร์แบบ Format
    /// </summary>
    public string AverageOrderValueFormatted => $"฿{AverageOrderValue:N0}";

    /// <summary>
    /// ระยะเวลา Live แบบ Format
    /// </summary>
    public string LiveDurationFormatted => LiveDuration.ToString(@"hh\:mm\:ss");

    #endregion

    /// <summary>
    /// สร้าง DashboardViewModel Instance
    /// </summary>
    public DashboardViewModel()
    {
        Title = "แดชบอร์ด";
    }

    /// <summary>
    /// เรียกเมื่อเข้าหน้านี้
    /// </summary>
    public override async Task OnNavigatedToAsync()
    {
        await RefreshDataAsync();
    }

    #region Commands

    /// <summary>
    /// โหลดข้อมูลใหม่
    /// </summary>
    [RelayCommand]
    private async Task RefreshDataAsync()
    {
        await ExecuteAsync(async () =>
        {
            // TODO: โหลดข้อมูลจาก Service จริง
            await Task.Delay(500); // Placeholder

            // Demo data
            TodaySales = 12500;
            YesterdaySales = 10000;
            TodayOrders = 15;
            YesterdayOrders = 12;
            NewCustomersToday = 5;
            AverageOrderValue = TodayOrders > 0 ? TodaySales / TodayOrders : 0;

            PendingCF = 8;
            PendingConfirmation = 3;
            PendingSlips = 5;
            PendingPacking = 7;
            PendingShipment = 4;

            // Demo recent orders
            RecentOrders.Clear();
            RecentOrders.Add(new RecentOrderItem
            {
                OrderNumber = "ORD-001",
                CustomerName = "สมชาย ใจดี",
                Amount = 1500,
                Status = "รอแพ็ค",
                Time = DateTime.Now.AddMinutes(-5)
            });
            RecentOrders.Add(new RecentOrderItem
            {
                OrderNumber = "ORD-002",
                CustomerName = "สมหญิง รักสวย",
                Amount = 2300,
                Status = "รอตรวจสลิป",
                Time = DateTime.Now.AddMinutes(-15)
            });

            // Demo top products
            TopProducts.Clear();
            TopProducts.Add(new TopProductItem
            {
                ProductName = "เสื้อยืด Cotton Premium",
                SKU = "TSH-001",
                SoldCount = 25,
                Revenue = 7500
            });
            TopProducts.Add(new TopProductItem
            {
                ProductName = "กางเกงขาสั้น",
                SKU = "PNT-002",
                SoldCount = 18,
                Revenue = 5400
            });

            // Notify computed properties
            OnPropertyChanged(nameof(TodaySalesFormatted));
            OnPropertyChanged(nameof(SalesChangePercent));
            OnPropertyChanged(nameof(IsSalesUp));
            OnPropertyChanged(nameof(OrderChangePercent));
            OnPropertyChanged(nameof(AverageOrderValueFormatted));
        }, "ไม่สามารถโหลดข้อมูล Dashboard ได้");
    }

    /// <summary>
    /// ไปหน้า CF ที่รอดำเนินการ
    /// </summary>
    [RelayCommand]
    private void GoToPendingCF()
    {
        // TODO: Navigate to CF page with filter
    }

    /// <summary>
    /// ไปหน้าสลิปรอตรวจ
    /// </summary>
    [RelayCommand]
    private void GoToPendingSlips()
    {
        // TODO: Navigate to Slip verification page
    }

    /// <summary>
    /// ไปหน้าออเดอร์รอแพ็ค
    /// </summary>
    [RelayCommand]
    private void GoToPendingPacking()
    {
        // TODO: Navigate to Packing page
    }

    /// <summary>
    /// เริ่ม Live ใหม่
    /// </summary>
    [RelayCommand]
    private async Task StartNewLiveAsync()
    {
        await ExecuteAsync(async () =>
        {
            // TODO: Show dialog and start live
            await Task.Delay(100);
        }, "ไม่สามารถเริ่ม Live ได้");
    }

    #endregion
}

#region Data Models

/// <summary>
/// รายการออเดอร์ล่าสุดสำหรับแสดงใน Dashboard
/// </summary>
public class RecentOrderItem
{
    /// <summary>
    /// เลขที่ออเดอร์
    /// </summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// ชื่อลูกค้า
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// ยอดเงิน
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// ยอดเงินแบบ Format
    /// </summary>
    public string AmountFormatted => $"฿{Amount:N0}";

    /// <summary>
    /// สถานะ
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// เวลาสั่งซื้อ
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// เวลาแบบ Format (กี่นาทีที่แล้ว)
    /// </summary>
    public string TimeAgo
    {
        get
        {
            var diff = DateTime.Now - Time;
            if (diff.TotalMinutes < 1) return "เมื่อสักครู่";
            if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} นาทีที่แล้ว";
            if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} ชม.ที่แล้ว";
            return Time.ToString("d MMM", new System.Globalization.CultureInfo("th-TH"));
        }
    }
}

/// <summary>
/// สินค้าขายดีสำหรับแสดงใน Dashboard
/// </summary>
public class TopProductItem
{
    /// <summary>
    /// ชื่อสินค้า
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// รหัสสินค้า
    /// </summary>
    public string SKU { get; set; } = string.Empty;

    /// <summary>
    /// จำนวนที่ขายได้
    /// </summary>
    public int SoldCount { get; set; }

    /// <summary>
    /// รายได้
    /// </summary>
    public decimal Revenue { get; set; }

    /// <summary>
    /// รายได้แบบ Format
    /// </summary>
    public string RevenueFormatted => $"฿{Revenue:N0}";
}

#endregion
