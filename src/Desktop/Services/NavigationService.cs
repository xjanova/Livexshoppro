// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Navigation Service
//  ระบบ Navigation สำหรับ WPF Frame
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace LiveXShopPro.Desktop.Services;

/// <summary>
/// Navigation Service สำหรับ WPF
/// ใช้ Frame ในการแสดงหน้าต่างๆ
/// </summary>
public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private Frame? _frame;
    private readonly Stack<string> _navigationStack = new();
    private readonly Dictionary<string, Type> _pageRegistry = new();

    /// <summary>
    /// หน้าปัจจุบัน
    /// </summary>
    public string CurrentPage { get; private set; } = string.Empty;

    /// <summary>
    /// สามารถกลับได้หรือไม่
    /// </summary>
    public bool CanGoBack => _navigationStack.Count > 1;

    /// <summary>
    /// Event เมื่อมีการนำทาง
    /// </summary>
    public event EventHandler<NavigationEventArgs>? Navigated;

    /// <summary>
    /// สร้าง NavigationService
    /// </summary>
    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        RegisterPages();
    }

    /// <summary>
    /// ลงทะเบียนหน้าทั้งหมด
    /// </summary>
    private void RegisterPages()
    {
        // TODO: ลงทะเบียนหน้าทั้งหมดเมื่อสร้างเสร็จ
        // _pageRegistry["Dashboard"] = typeof(DashboardPage);
        // _pageRegistry["Live"] = typeof(LivePage);
        // _pageRegistry["Chat"] = typeof(ChatPage);
        // _pageRegistry["Orders"] = typeof(OrdersPage);
        // _pageRegistry["Products"] = typeof(ProductsPage);
        // _pageRegistry["Customers"] = typeof(CustomersPage);
        // _pageRegistry["POS"] = typeof(POSPage);
        // _pageRegistry["Payments"] = typeof(PaymentsPage);
        // _pageRegistry["SlipVerify"] = typeof(SlipVerifyPage);
        // _pageRegistry["Shipping"] = typeof(ShippingPage);
        // _pageRegistry["Packing"] = typeof(PackingPage);
        // _pageRegistry["Reports"] = typeof(ReportsPage);
        // _pageRegistry["Analytics"] = typeof(AnalyticsPage);
        // _pageRegistry["Settings"] = typeof(SettingsPage);
    }

    /// <summary>
    /// ตั้งค่า Frame ที่ใช้แสดงหน้า
    /// </summary>
    /// <param name="frame">WPF Frame</param>
    public void SetFrame(Frame frame)
    {
        _frame = frame;
    }

    /// <summary>
    /// ลงทะเบียนหน้าใหม่
    /// </summary>
    public void RegisterPage(string pageName, Type pageType)
    {
        _pageRegistry[pageName] = pageType;
    }

    /// <summary>
    /// นำทางไปยังหน้าที่ระบุ
    /// </summary>
    public async Task NavigateToAsync(string pageName, object? parameter = null)
    {
        if (_frame == null)
        {
            throw new InvalidOperationException("Frame ยังไม่ได้ตั้งค่า กรุณาเรียก SetFrame ก่อน");
        }

        if (!_pageRegistry.TryGetValue(pageName, out var pageType))
        {
            // ถ้าไม่มีหน้าลงทะเบียน ให้แสดง placeholder
            System.Diagnostics.Debug.WriteLine($"หน้า {pageName} ยังไม่ได้ลงทะเบียน");
            CurrentPage = pageName;
            _navigationStack.Push(pageName);
            Navigated?.Invoke(this, new NavigationEventArgs(pageName, parameter));
            return;
        }

        // สร้างหน้าจาก DI Container
        var page = _serviceProvider.GetRequiredService(pageType);

        // ถ้าหน้ามี DataContext ที่เป็น ViewModel ให้เรียก OnNavigatedTo
        if (page is Page wpfPage && wpfPage.DataContext is ViewModels.BaseViewModel viewModel)
        {
            await viewModel.OnNavigatedToAsync();
        }

        // นำทาง
        _frame.Navigate(page);
        CurrentPage = pageName;
        _navigationStack.Push(pageName);

        Navigated?.Invoke(this, new NavigationEventArgs(pageName, parameter));
    }

    /// <summary>
    /// นำทางด้วย Type
    /// </summary>
    public async Task NavigateToAsync<TPage>(object? parameter = null) where TPage : class
    {
        var pageName = _pageRegistry.FirstOrDefault(x => x.Value == typeof(TPage)).Key;
        if (string.IsNullOrEmpty(pageName))
        {
            pageName = typeof(TPage).Name;
        }

        await NavigateToAsync(pageName, parameter);
    }

    /// <summary>
    /// กลับไปหน้าก่อนหน้า
    /// </summary>
    public async Task GoBackAsync()
    {
        if (!CanGoBack) return;

        if (_frame?.CanGoBack == true)
        {
            // แจ้ง ViewModel ปัจจุบันว่ากำลังออก
            if (_frame.Content is Page currentPage &&
                currentPage.DataContext is ViewModels.BaseViewModel currentViewModel)
            {
                await currentViewModel.OnNavigatedFromAsync();
            }

            _navigationStack.Pop(); // Pop หน้าปัจจุบัน
            _frame.GoBack();

            if (_navigationStack.TryPeek(out var previousPage))
            {
                CurrentPage = previousPage;
                Navigated?.Invoke(this, new NavigationEventArgs(previousPage));
            }
        }
    }
}
