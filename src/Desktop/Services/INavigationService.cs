// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Navigation Service Interface
//  Interface สำหรับระบบ Navigation ในแอพ
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

namespace LiveXShopPro.Desktop.Services;

/// <summary>
/// Interface สำหรับ Navigation Service
/// ใช้ในการนำทางระหว่างหน้าต่างๆ ในแอพ
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// หน้าปัจจุบันที่แสดงอยู่
    /// </summary>
    string CurrentPage { get; }

    /// <summary>
    /// สามารถกลับไปหน้าก่อนหน้าได้หรือไม่
    /// </summary>
    bool CanGoBack { get; }

    /// <summary>
    /// นำทางไปยังหน้าที่ระบุ
    /// </summary>
    /// <param name="pageName">ชื่อหน้าที่ต้องการไป</param>
    /// <param name="parameter">พารามิเตอร์ที่ต้องการส่งไป (ถ้ามี)</param>
    Task NavigateToAsync(string pageName, object? parameter = null);

    /// <summary>
    /// นำทางไปยังหน้าที่ระบุด้วย Type
    /// </summary>
    /// <typeparam name="TPage">Type ของหน้าที่ต้องการไป</typeparam>
    /// <param name="parameter">พารามิเตอร์ที่ต้องการส่งไป (ถ้ามี)</param>
    Task NavigateToAsync<TPage>(object? parameter = null) where TPage : class;

    /// <summary>
    /// กลับไปหน้าก่อนหน้า
    /// </summary>
    Task GoBackAsync();

    /// <summary>
    /// Event เมื่อมีการเปลี่ยนหน้า
    /// </summary>
    event EventHandler<NavigationEventArgs>? Navigated;
}

/// <summary>
/// Event Arguments สำหรับ Navigation
/// </summary>
public class NavigationEventArgs : EventArgs
{
    /// <summary>
    /// ชื่อหน้าที่นำทางไป
    /// </summary>
    public string PageName { get; }

    /// <summary>
    /// พารามิเตอร์ที่ส่งไป
    /// </summary>
    public object? Parameter { get; }

    /// <summary>
    /// สร้าง NavigationEventArgs
    /// </summary>
    public NavigationEventArgs(string pageName, object? parameter = null)
    {
        PageName = pageName;
        Parameter = parameter;
    }
}
