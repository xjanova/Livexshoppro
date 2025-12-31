// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Live Page Code-Behind
//  หน้าจัดการการถ่ายทอดสด
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LiveXShopPro.Desktop.Views.Live;

/// <summary>
/// Live Page - หน้าจัดการการถ่ายทอดสด
/// </summary>
public partial class LivePage : Page
{
    private bool _isLive = false;

    /// <summary>
    /// สร้าง LivePage Instance
    /// </summary>
    public LivePage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// เริ่มดึงแชท
    /// </summary>
    private void OnStartLiveClicked(object sender, RoutedEventArgs e)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(LiveTitleInput.Text))
        {
            MessageBox.Show("กรุณาใส่ชื่อ Live Session", "แจ้งเตือน",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Start live
        _isLive = true;
        UpdateLiveStatus();

        // Hide/Show buttons
        StartLiveButton.Visibility = Visibility.Collapsed;
        StopLiveButton.Visibility = Visibility.Visible;

        // TODO: เริ่ม Chat Capture Service จริง
    }

    /// <summary>
    /// หยุดดึงแชท
    /// </summary>
    private void OnStopLiveClicked(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show(
            "ต้องการหยุดดึงแชทหรือไม่?",
            "ยืนยัน",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            _isLive = false;
            UpdateLiveStatus();

            // Show/Hide buttons
            StartLiveButton.Visibility = Visibility.Visible;
            StopLiveButton.Visibility = Visibility.Collapsed;

            // TODO: หยุด Chat Capture Service
        }
    }

    /// <summary>
    /// แสดง QR Code สำหรับเชื่อมต่อ Mobile
    /// </summary>
    private void OnShowQRClicked(object sender, RoutedEventArgs e)
    {
        // TODO: แสดง Dialog พร้อม QR Code
        MessageBox.Show(
            "QR Code จะแสดงที่นี่\nสแกนด้วยแอพ Live x Shop Pro Mobile",
            "QR Code",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    /// <summary>
    /// อัพเดทสถานะ Live
    /// </summary>
    private void UpdateLiveStatus()
    {
        if (_isLive)
        {
            LiveStatusText.Text = "กำลัง Live อยู่";
            LiveIndicator.Background = new SolidColorBrush(Color.FromRgb(74, 222, 128)); // Success
        }
        else
        {
            LiveStatusText.Text = "ยังไม่ได้เริ่ม Live";
            LiveIndicator.Background = new SolidColorBrush(Color.FromRgb(248, 113, 113)); // Error
            LiveDurationText.Text = "";
        }
    }
}
