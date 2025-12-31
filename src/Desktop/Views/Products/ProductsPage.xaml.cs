// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Products Page Code-Behind
//  หน้าจัดการสินค้าและสต็อก
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using System.Windows;
using System.Windows.Controls;

namespace LiveXShopPro.Desktop.Views.Products;

/// <summary>
/// Products Page - หน้าจัดการสินค้าและสต็อก
/// </summary>
public partial class ProductsPage : Page
{
    /// <summary>
    /// สร้าง ProductsPage Instance
    /// </summary>
    public ProductsPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// เปิด Dialog เพิ่มสินค้า
    /// </summary>
    private void OnAddProductClicked(object sender, RoutedEventArgs e)
    {
        // TODO: แสดง Dialog สำหรับเพิ่มสินค้า
        MessageBox.Show("เพิ่มสินค้าใหม่", "เพิ่มสินค้า", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
