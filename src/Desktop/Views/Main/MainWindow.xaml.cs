// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Main Window Code-Behind
//  Event Handlers และ Navigation Logic
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace LiveXShopPro.Desktop.Views.Main;

/// <summary>
/// MainWindow - หน้าต่างหลักของโปรแกรม
/// </summary>
public partial class MainWindow : MetroWindow
{
    /// <summary>
    /// สร้าง MainWindow Instance
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        // ตั้งค่า Event Handlers
        SetupEventHandlers();

        // โหลดหน้าแรก
        NavigateToDashboard();
    }

    /// <summary>
    /// ตั้งค่า Event Handlers สำหรับ Navigation และ UI
    /// </summary>
    private void SetupEventHandlers()
    {
        // Navigation Events
        NavDashboard.Checked += (s, e) => NavigateToDashboard();
        NavLive.Checked += (s, e) => NavigateToLive();
        NavChat.Checked += (s, e) => NavigateToChat();
        NavOrders.Checked += (s, e) => NavigateToOrders();
        NavProducts.Checked += (s, e) => NavigateToProducts();
        NavCustomers.Checked += (s, e) => NavigateToCustomers();
        NavPOS.Checked += (s, e) => NavigateToPOS();
        NavPayments.Checked += (s, e) => NavigateToPayments();
        NavSlipVerify.Checked += (s, e) => NavigateToSlipVerify();
        NavShipping.Checked += (s, e) => NavigateToShipping();
        NavPacking.Checked += (s, e) => NavigateToPacking();
        NavReports.Checked += (s, e) => NavigateToReports();
        NavAnalytics.Checked += (s, e) => NavigateToAnalytics();

        // Title Bar Button Events
        SettingsButton.Click += (s, e) => NavigateToSettings();
        StartLiveButton.Click += (s, e) => StartLiveSession();

        // Keyboard Shortcuts
        this.PreviewKeyDown += MainWindow_PreviewKeyDown;
    }

    /// <summary>
    /// จัดการ Keyboard Shortcuts
    /// </summary>
    private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Ctrl+K = Focus Search
        if (e.Key == Key.K && Keyboard.Modifiers == ModifierKeys.Control)
        {
            SearchBox.Focus();
            SearchBox.SelectAll();
            e.Handled = true;
        }

        // F1 = Dashboard
        if (e.Key == Key.F1)
        {
            NavDashboard.IsChecked = true;
            e.Handled = true;
        }

        // F2 = Live
        if (e.Key == Key.F2)
        {
            NavLive.IsChecked = true;
            e.Handled = true;
        }

        // F3 = Chat
        if (e.Key == Key.F3)
        {
            NavChat.IsChecked = true;
            e.Handled = true;
        }

        // F4 = Orders
        if (e.Key == Key.F4)
        {
            NavOrders.IsChecked = true;
            e.Handled = true;
        }
    }

    #region Navigation Methods

    /// <summary>
    /// อัพเดท Page Header
    /// </summary>
    private void UpdatePageHeader(string title, string subtitle)
    {
        PageTitle.Text = title;
        PageSubtitle.Text = subtitle;
    }

    /// <summary>
    /// ไปหน้าแดชบอร์ด
    /// </summary>
    private void NavigateToDashboard()
    {
        UpdatePageHeader("แดชบอร์ด", "ภาพรวมการขายและสถิติของร้าน");
        // TODO: Navigate to DashboardPage
        // ContentFrame.Navigate(new DashboardPage());
    }

    /// <summary>
    /// ไปหน้าไลฟ์สด
    /// </summary>
    private void NavigateToLive()
    {
        UpdatePageHeader("ไลฟ์สด", "จัดการการถ่ายทอดสดและดึงแชท");
        // TODO: Navigate to LivePage
    }

    /// <summary>
    /// ไปหน้าแชท CF
    /// </summary>
    private void NavigateToChat()
    {
        UpdatePageHeader("แชท CF", "จัดการข้อความ CF และสร้างออเดอร์");
        // TODO: Navigate to ChatPage
    }

    /// <summary>
    /// ไปหน้าออเดอร์
    /// </summary>
    private void NavigateToOrders()
    {
        UpdatePageHeader("ออเดอร์", "จัดการออเดอร์ทั้งหมด");
        // TODO: Navigate to OrdersPage
    }

    /// <summary>
    /// ไปหน้าสินค้า
    /// </summary>
    private void NavigateToProducts()
    {
        UpdatePageHeader("สินค้า", "จัดการสินค้าและสต็อก");
        // TODO: Navigate to ProductsPage
    }

    /// <summary>
    /// ไปหน้าลูกค้า
    /// </summary>
    private void NavigateToCustomers()
    {
        UpdatePageHeader("ลูกค้า", "ข้อมูลลูกค้าและประวัติการสั่งซื้อ");
        // TODO: Navigate to CustomersPage
    }

    /// <summary>
    /// ไปหน้า POS
    /// </summary>
    private void NavigateToPOS()
    {
        UpdatePageHeader("POS", "ขายหน้าร้าน ณ จุดขาย");
        // TODO: Navigate to POSPage
    }

    /// <summary>
    /// ไปหน้าการชำระเงิน
    /// </summary>
    private void NavigateToPayments()
    {
        UpdatePageHeader("การชำระเงิน", "จัดการการชำระเงินและยืนยันการโอน");
        // TODO: Navigate to PaymentsPage
    }

    /// <summary>
    /// ไปหน้าตรวจสลิป
    /// </summary>
    private void NavigateToSlipVerify()
    {
        UpdatePageHeader("ตรวจสลิป", "ตรวจสอบสลิปโอนเงินด้วย OCR");
        // TODO: Navigate to SlipVerifyPage
    }

    /// <summary>
    /// ไปหน้าการจัดส่ง
    /// </summary>
    private void NavigateToShipping()
    {
        UpdatePageHeader("การจัดส่ง", "จัดการพัสดุและติดตามการจัดส่ง");
        // TODO: Navigate to ShippingPage
    }

    /// <summary>
    /// ไปหน้าสถานีแพ็ค
    /// </summary>
    private void NavigateToPacking()
    {
        UpdatePageHeader("สถานีแพ็ค", "แพ็คสินค้าและพิมพ์ใบปะหน้า");
        // TODO: Navigate to PackingPage
    }

    /// <summary>
    /// ไปหน้ารายงาน
    /// </summary>
    private void NavigateToReports()
    {
        UpdatePageHeader("รายงาน", "รายงานยอดขายและสรุปข้อมูล");
        // TODO: Navigate to ReportsPage
    }

    /// <summary>
    /// ไปหน้าวิเคราะห์
    /// </summary>
    private void NavigateToAnalytics()
    {
        UpdatePageHeader("วิเคราะห์", "วิเคราะห์ข้อมูลเชิงลึกและแนวโน้ม");
        // TODO: Navigate to AnalyticsPage
    }

    /// <summary>
    /// ไปหน้าตั้งค่า
    /// </summary>
    private void NavigateToSettings()
    {
        UpdatePageHeader("ตั้งค่า", "ตั้งค่าโปรแกรมและการเชื่อมต่อ");
        // TODO: Navigate to SettingsPage
    }

    #endregion

    #region Actions

    /// <summary>
    /// เริ่มเซสชันไลฟ์ใหม่
    /// </summary>
    private void StartLiveSession()
    {
        // TODO: Show dialog to start new live session
        NavLive.IsChecked = true;
    }

    #endregion
}
