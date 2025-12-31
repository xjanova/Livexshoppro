// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Base ViewModel
//  คลาสพื้นฐานสำหรับ ViewModel ทั้งหมด พร้อม MVVM Toolkit
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LiveXShopPro.Desktop.ViewModels;

/// <summary>
/// Base ViewModel ที่ใช้ CommunityToolkit.Mvvm
/// ทุก ViewModel ควร inherit จากคลาสนี้
/// </summary>
public abstract partial class BaseViewModel : ObservableObject
{
    /// <summary>
    /// สถานะกำลังโหลดข้อมูล
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    /// <summary>
    /// สถานะไม่ได้โหลด (inverse ของ IsBusy)
    /// </summary>
    public bool IsNotBusy => !IsBusy;

    /// <summary>
    /// ชื่อหน้า/Section ปัจจุบัน
    /// </summary>
    [ObservableProperty]
    private string _title = string.Empty;

    /// <summary>
    /// ข้อความ Error ถ้ามี
    /// </summary>
    [ObservableProperty]
    private string? _errorMessage;

    /// <summary>
    /// มี Error หรือไม่
    /// </summary>
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    /// <summary>
    /// เรียกเมื่อ Navigate เข้ามาหน้านี้
    /// </summary>
    public virtual Task OnNavigatedToAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// เรียกเมื่อ Navigate ออกจากหน้านี้
    /// </summary>
    public virtual Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// ล้าง Error Message
    /// </summary>
    protected void ClearError()
    {
        ErrorMessage = null;
        OnPropertyChanged(nameof(HasError));
    }

    /// <summary>
    /// ตั้งค่า Error Message
    /// </summary>
    protected void SetError(string message)
    {
        ErrorMessage = message;
        OnPropertyChanged(nameof(HasError));
    }

    /// <summary>
    /// Execute action ที่มี busy state และ error handling
    /// </summary>
    protected async Task ExecuteAsync(Func<Task> action, string? errorPrefix = null)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();
            await action();
        }
        catch (Exception ex)
        {
            var prefix = errorPrefix ?? "เกิดข้อผิดพลาด";
            SetError($"{prefix}: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Execute action ที่มี return value, busy state และ error handling
    /// </summary>
    protected async Task<T?> ExecuteAsync<T>(Func<Task<T>> action, string? errorPrefix = null)
    {
        if (IsBusy) return default;

        try
        {
            IsBusy = true;
            ClearError();
            return await action();
        }
        catch (Exception ex)
        {
            var prefix = errorPrefix ?? "เกิดข้อผิดพลาด";
            SetError($"{prefix}: {ex.Message}");
            return default;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
