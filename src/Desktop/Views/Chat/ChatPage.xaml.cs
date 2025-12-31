// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Chat Page Code-Behind
//  หน้าจัดการ CF และสร้างออเดอร์
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using System.Windows;
using System.Windows.Controls;

namespace LiveXShopPro.Desktop.Views.Chat;

/// <summary>
/// Chat Page - หน้าจัดการ CF และสร้างออเดอร์
/// </summary>
public partial class ChatPage : Page
{
    /// <summary>
    /// สร้าง ChatPage Instance
    /// </summary>
    public ChatPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// ข้าม CF นี้
    /// </summary>
    private void OnSkipClicked(object sender, RoutedEventArgs e)
    {
        // TODO: ข้าม CF และไปยัง CF ถัดไป
        MessageBox.Show("ข้าม CF นี้", "แจ้งเตือน", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    /// สร้างออเดอร์
    /// </summary>
    private void OnCreateOrderClicked(object sender, RoutedEventArgs e)
    {
        // TODO: สร้างออเดอร์จากข้อมูลที่กรอก
        MessageBox.Show("สร้างออเดอร์สำเร็จ!", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
