// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Connect Page Code Behind
//  Logic สำหรับหน้าเชื่อมต่อ
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

namespace LiveXShopPro.Mobile.Views;

/// <summary>
/// หน้าเชื่อมต่อกับโปรแกรม Desktop
/// </summary>
public partial class ConnectPage : ContentPage
{
    /// <summary>
    /// สร้าง ConnectPage Instance
    /// </summary>
    public ConnectPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// เมื่อกดปุ่มสแกน QR Code
    /// </summary>
    private async void OnScanQrClicked(object sender, EventArgs e)
    {
        // TODO: เปิดหน้าสแกน QR Code
        await DisplayAlert(
            "สแกน QR Code",
            "ฟีเจอร์นี้กำลังพัฒนา",
            "ตกลง");
    }

    /// <summary>
    /// เมื่อกดปุ่มเชื่อมต่อ
    /// </summary>
    private async void OnConnectClicked(object sender, EventArgs e)
    {
        var ipAddress = IpEntry.Text?.Trim();

        if (string.IsNullOrEmpty(ipAddress))
        {
            await DisplayAlert(
                "ข้อผิดพลาด",
                "กรุณาใส่ IP Address",
                "ตกลง");
            return;
        }

        // อัพเดทสถานะ
        StatusLabel.Text = "กำลังเชื่อมต่อ...";
        StatusLabel.TextColor = Color.FromArgb("#60A5FA"); // Info color

        // TODO: เชื่อมต่อด้วย SignalR
        await Task.Delay(2000); // จำลองการเชื่อมต่อ

        // จำลองการเชื่อมต่อสำเร็จ
        StatusLabel.Text = $"เชื่อมต่อกับ {ipAddress} สำเร็จ";
        StatusLabel.TextColor = Color.FromArgb("#4ADE80"); // Success color

        await DisplayAlert(
            "สำเร็จ",
            $"เชื่อมต่อกับ {ipAddress} เรียบร้อย",
            "ตกลง");
    }
}
