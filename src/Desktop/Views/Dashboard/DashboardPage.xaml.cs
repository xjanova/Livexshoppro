// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Dashboard Page Code-Behind
//  หน้าแดชบอร์ดแสดงภาพรวมการขาย
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using System.Windows.Controls;
using LiveXShopPro.Desktop.ViewModels;

namespace LiveXShopPro.Desktop.Views.Dashboard;

/// <summary>
/// Dashboard Page - หน้าภาพรวมการขายและสถิติ
/// </summary>
public partial class DashboardPage : Page
{
    /// <summary>
    /// สร้าง DashboardPage Instance
    /// </summary>
    public DashboardPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// สร้าง DashboardPage พร้อม ViewModel
    /// </summary>
    /// <param name="viewModel">DashboardViewModel</param>
    public DashboardPage(DashboardViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}
