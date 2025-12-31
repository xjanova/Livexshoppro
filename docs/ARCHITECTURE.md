# 🏗️ สถาปัตยกรรมระบบ Live x Shop Pro

> เอกสารอธิบายโครงสร้างและการออกแบบระบบ

---

## 📋 สารบัญ

1. [ภาพรวมสถาปัตยกรรม](#ภาพรวมสถาปัตยกรรม)
2. [Clean Architecture](#clean-architecture)
3. [Project Structure](#project-structure)
4. [Design Patterns](#design-patterns)
5. [Communication Flow](#communication-flow)
6. [Security Architecture](#security-architecture)

---

## 🎯 ภาพรวมสถาปัตยกรรม

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                           LIVE x SHOP PRO ECOSYSTEM                          │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ┌─────────────┐     ┌─────────────┐     ┌─────────────┐                    │
│  │  📱 Mobile  │     │  🖥️ Desktop │     │  📦 Packing │                    │
│  │    App      │────▶│    (Main)   │◀────│   Station   │                    │
│  │ (ดูดแชท)    │     │             │     │             │                    │
│  └─────────────┘     └──────┬──────┘     └─────────────┘                    │
│         │                   │                   │                           │
│         │    SignalR Hub    │                   │                           │
│         └───────────────────┼───────────────────┘                           │
│                             │                                               │
│                    ┌────────▼────────┐                                      │
│                    │  Built-in Web   │                                      │
│                    │     Server      │                                      │
│                    └────────┬────────┘                                      │
│                             │                                               │
│         ┌───────────────────┼───────────────────┐                           │
│         ▼                   ▼                   ▼                           │
│  ┌─────────────┐     ┌─────────────┐     ┌─────────────┐                    │
│  │  🌐 OBS     │     │  👥 Customer│     │  ⚙️ XMan    │                    │
│  │  Overlay    │     │   Portal    │     │   Studio    │                    │
│  └─────────────┘     └─────────────┘     │  (License)  │                    │
│                                          └─────────────┘                    │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## 🧅 Clean Architecture

### Layer Diagram

```
                    ┌─────────────────────────────────┐
                    │        🖥️ PRESENTATION          │
                    │   Desktop (WPF) / Mobile (MAUI) │
                    │   Views, ViewModels, UI Logic   │
                    └──────────────┬──────────────────┘
                                   │
                                   ▼
                    ┌─────────────────────────────────┐
                    │      🔧 INFRASTRUCTURE          │
                    │   EF Core, APIs, External Svcs  │
                    │   Repositories Implementation   │
                    └──────────────┬──────────────────┘
                                   │
                                   ▼
                    ┌─────────────────────────────────┐
                    │       📋 APPLICATION            │
                    │   Use Cases, CQRS, Validators   │
                    │   DTOs, Mappings, Behaviors     │
                    └──────────────┬──────────────────┘
                                   │
                                   ▼
                    ┌─────────────────────────────────┐
                    │          💎 CORE                │
                    │   Entities, Interfaces, Enums   │
                    │   Domain Events, Value Objects  │
                    └─────────────────────────────────┘
```

### Dependency Rule

```
Desktop ──────▶ Infrastructure ──────▶ Application ──────▶ Core
   │                  │                     │                │
   │                  │                     │                │
   ▼                  ▼                     ▼                ▼
UI Layer        Data Access           Business Logic    Domain Layer
(Presentation)  (Implementation)      (Use Cases)       (Entities)
```

**กฎ:** Dependencies ต้องชี้เข้าหา Core เท่านั้น ห้ามชี้ออก

---

## 📁 Project Structure

### Core Layer (LiveXShopPro.Core)

```
Core/
├── Entities/                      # Domain Entities
│   ├── BaseEntity.cs             # Entity พื้นฐาน
│   ├── Customer.cs               # ลูกค้า
│   ├── Product.cs                # สินค้า
│   ├── Order.cs                  # ออเดอร์
│   ├── OrderItem.cs              # รายการสินค้าในออเดอร์
│   ├── Payment.cs                # การชำระเงิน
│   ├── Shipment.cs               # การจัดส่ง
│   ├── LiveSession.cs            # เซสชันไลฟ์
│   ├── ChatMessage.cs            # ข้อความแชท
│   └── SlipVerification.cs       # การตรวจสอบสลิป
│
├── Interfaces/                    # Repository Interfaces
│   ├── IRepository.cs            # Generic Repository
│   ├── ICustomerRepository.cs    # Customer Repository
│   ├── IProductRepository.cs     # Product Repository
│   ├── IOrderRepository.cs       # Order Repository
│   └── IUnitOfWork.cs            # Unit of Work Pattern
│
├── Enums/                         # Enumerations
│   ├── OrderStatus.cs            # สถานะออเดอร์
│   ├── PaymentStatus.cs          # สถานะการชำระเงิน
│   ├── ShipmentStatus.cs         # สถานะการจัดส่ง
│   ├── Platform.cs               # แพลตฟอร์ม (FB/TikTok/LINE)
│   └── SlipVerificationMode.cs   # โหมดตรวจสลิป
│
├── Events/                        # Domain Events
│   ├── OrderCreatedEvent.cs      # ออเดอร์ถูกสร้าง
│   ├── PaymentReceivedEvent.cs   # ได้รับชำระเงิน
│   └── ShipmentSentEvent.cs      # ส่งสินค้าแล้ว
│
└── Exceptions/                    # Custom Exceptions
    ├── DomainException.cs        # Domain Exception พื้นฐาน
    ├── OrderException.cs         # ข้อผิดพลาดออเดอร์
    └── PaymentException.cs       # ข้อผิดพลาดการชำระเงิน
```

### Application Layer (LiveXShopPro.Application)

```
Application/
├── Common/
│   ├── Behaviors/                 # MediatR Behaviors
│   │   ├── LoggingBehavior.cs    # Logging Pipeline
│   │   ├── ValidationBehavior.cs # Validation Pipeline
│   │   └── TransactionBehavior.cs# Transaction Pipeline
│   │
│   ├── Interfaces/                # Application Interfaces
│   │   ├── IChatBotService.cs    # Chat Bot Service
│   │   ├── IOcrService.cs        # OCR Service
│   │   ├── ISmsService.cs        # SMS Service
│   │   ├── IShippingService.cs   # Shipping Service
│   │   └── ILicenseService.cs    # License Service
│   │
│   ├── Mappings/                  # AutoMapper Profiles
│   │   └── MappingProfile.cs     # Object Mappings
│   │
│   └── Models/                    # DTOs
│       ├── OrderDto.cs           # Order DTO
│       ├── CustomerDto.cs        # Customer DTO
│       └── ProductDto.cs         # Product DTO
│
├── Features/                      # Use Cases (CQRS)
│   ├── Chat/                      # Chat Features
│   │   ├── Commands/
│   │   │   ├── StartChatBot/     # เริ่มดูดแชท
│   │   │   └── StopChatBot/      # หยุดดูดแชท
│   │   └── Queries/
│   │       └── GetChatMessages/  # ดึงข้อความแชท
│   │
│   ├── Orders/                    # Order Features
│   │   ├── Commands/
│   │   │   ├── CreateOrder/      # สร้างออเดอร์
│   │   │   ├── UpdateOrder/      # อัพเดทออเดอร์
│   │   │   └── CancelOrder/      # ยกเลิกออเดอร์
│   │   └── Queries/
│   │       ├── GetOrderById/     # ดึงออเดอร์ตาม ID
│   │       └── GetOrdersList/    # ดึงรายการออเดอร์
│   │
│   ├── Products/                  # Product Features
│   ├── Customers/                 # Customer Features
│   ├── Payments/                  # Payment Features
│   ├── Shipping/                  # Shipping Features
│   ├── Reports/                   # Report Features
│   └── Settings/                  # Settings Features
│
└── Services/                      # Application Services
    └── ApplicationService.cs     # Shared Logic
```

### Infrastructure Layer (LiveXShopPro.Infrastructure)

```
Infrastructure/
├── Data/
│   ├── Context/
│   │   └── AppDbContext.cs       # EF Core DbContext
│   │
│   ├── Configurations/            # EF Core Configurations
│   │   ├── CustomerConfiguration.cs
│   │   ├── ProductConfiguration.cs
│   │   └── OrderConfiguration.cs
│   │
│   ├── Migrations/                # EF Core Migrations
│   │
│   └── Repositories/              # Repository Implementations
│       ├── Repository.cs         # Generic Repository
│       ├── CustomerRepository.cs
│       ├── ProductRepository.cs
│       └── OrderRepository.cs
│
├── Services/
│   ├── ChatBot/                   # Chat Bot Services
│   │   ├── FacebookChatBot.cs    # Facebook Live Bot
│   │   ├── TikTokChatBot.cs      # TikTok Live Bot
│   │   └── LineChatBot.cs        # LINE OA Bot
│   │
│   ├── OCR/                       # OCR Services
│   │   ├── TesseractOcrService.cs# Tesseract OCR
│   │   └── SlipVerifier.cs       # ตรวจสลิปปลอม
│   │
│   ├── SMS/                       # SMS Services
│   │   └── BankSmsReader.cs      # อ่าน SMS ธนาคาร
│   │
│   ├── Shipping/                  # Shipping Services
│   │   ├── KerryService.cs       # Kerry Express API
│   │   ├── FlashService.cs       # Flash Express API
│   │   └── JAndTService.cs       # J&T Express API
│   │
│   ├── License/                   # License Services
│   │   └── XManLicenseService.cs # XMan Studio License
│   │
│   └── WebServer/                 # Built-in Web Server
│       ├── EmbedIoWebServer.cs   # EmbedIO Server
│       └── SignalRHub.cs         # SignalR Hub
│
└── External/                      # External Integrations
    └── XManStudioClient.cs       # XMan Studio API Client
```

### Desktop Layer (LiveXShopPro.Desktop)

```
Desktop/
├── App.xaml                       # Application Entry
├── App.xaml.cs
│
├── Views/
│   ├── Main/
│   │   ├── MainWindow.xaml       # หน้าต่างหลัก
│   │   ├── DashboardView.xaml    # Dashboard
│   │   └── NavigationView.xaml   # เมนูนำทาง
│   │
│   ├── Chat/
│   │   ├── LiveChatView.xaml     # หน้าดูดแชท
│   │   └── ChatSettingsView.xaml # ตั้งค่าแชท
│   │
│   ├── Orders/
│   │   ├── OrderListView.xaml    # รายการออเดอร์
│   │   ├── OrderDetailView.xaml  # รายละเอียดออเดอร์
│   │   └── CreateOrderView.xaml  # สร้างออเดอร์
│   │
│   ├── Products/                  # หน้าสินค้า
│   ├── Customers/                 # หน้าลูกค้า
│   ├── Payments/                  # หน้าการชำระเงิน
│   ├── Shipping/                  # หน้าขนส่ง
│   ├── Reports/                   # หน้ารายงาน
│   ├── Settings/                  # หน้าตั้งค่า
│   │
│   ├── PackingStation/            # หน้าจอห้องแพค
│   │   └── PackingView.xaml      # แสดงออเดอร์ Real-time
│   │
│   └── Overlay/                   # OBS Overlay
│       └── ObsOverlayView.xaml   # Overlay สำหรับ OBS
│
├── ViewModels/
│   ├── MainViewModel.cs          # Main ViewModel
│   ├── DashboardViewModel.cs     # Dashboard ViewModel
│   ├── LiveChatViewModel.cs      # Live Chat ViewModel
│   ├── OrderListViewModel.cs     # Order List ViewModel
│   └── ...
│
├── Controls/                      # Custom Controls
│   ├── ChatBubbleControl.xaml    # Chat Bubble
│   ├── OrderCardControl.xaml     # Order Card
│   └── ProductCardControl.xaml   # Product Card
│
├── Themes/                        # UI Themes
│   ├── DarkTheme.xaml            # Dark Theme
│   ├── GlassmorphismStyles.xaml  # Glassmorphism Styles
│   └── Colors.xaml               # Color Definitions
│
├── Resources/
│   ├── Fonts/
│   │   ├── Sarabun-Regular.ttf   # Font หลัก
│   │   └── Sarabun-Bold.ttf
│   └── Images/                    # รูปภาพ
│
├── Converters/                    # Value Converters
│   └── BoolToVisibilityConverter.cs
│
└── Helpers/                       # Helper Classes
    └── ThemeHelper.cs
```

---

## 🎨 Design Patterns

### 1. CQRS (Command Query Responsibility Segregation)

```csharp
// Command - สร้างออเดอร์
public record CreateOrderCommand(
    Guid CustomerId,
    List<OrderItemDto> Items
) : IRequest<OrderDto>;

// Handler
public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        // สร้างออเดอร์ใหม่
        var order = new Order(request.CustomerId);
        // เพิ่มรายการสินค้า
        foreach (var item in request.Items)
        {
            order.AddItem(item.ProductId, item.Quantity);
        }
        // บันทึกลง Database
        await _orderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<OrderDto>(order);
    }
}
```

### 2. Repository Pattern

```csharp
// Interface (Core Layer)
public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByTrackingNumberAsync(string trackingNumber);
    Task<IEnumerable<Order>> GetPendingOrdersAsync();
}

// Implementation (Infrastructure Layer)
public class OrderRepository : Repository<Order>, IOrderRepository
{
    public async Task<Order?> GetByTrackingNumberAsync(string trackingNumber)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.TrackingNumber == trackingNumber);
    }
}
```

### 3. MVVM (Model-View-ViewModel)

```csharp
// ViewModel
public partial class OrderListViewModel : ObservableObject
{
    private readonly IMediator _mediator;

    [ObservableProperty]
    private ObservableCollection<OrderDto> _orders = new();

    [ObservableProperty]
    private bool _isLoading;

    [RelayCommand]
    private async Task LoadOrdersAsync()
    {
        IsLoading = true;
        var result = await _mediator.Send(new GetOrdersListQuery());
        Orders = new ObservableCollection<OrderDto>(result);
        IsLoading = false;
    }
}
```

---

## 🔄 Communication Flow

### LAN/VPN Connection Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                    NETWORK COMMUNICATION                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌──────────────┐          SignalR Hub          ┌──────────────┐│
│  │   📱 Mobile  │◀──────────────────────────────▶│  🖥️ Desktop  ││
│  │     App      │    WebSocket (Port 5000)       │    (Main)    ││
│  └──────────────┘                                └──────────────┘│
│         │                                               │        │
│         │                 Discovery                     │        │
│         └──────────────────────────────────────────────┘        │
│                    mDNS/ZeroConf                                │
│                                                                  │
│  ┌──────────────┐                                ┌──────────────┐│
│  │   📦 Pack    │◀───────── SignalR ───────────▶│  🖥️ Desktop  ││
│  │   Station    │    Real-time Orders            │    (Main)    ││
│  └──────────────┘                                └──────────────┘│
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### Chat Message Flow

```
┌──────────────────────────────────────────────────────────────────┐
│                     CHAT MESSAGE FLOW                             │
├──────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌─────────┐     ┌─────────────┐     ┌─────────────┐             │
│  │ FB Live │────▶│ Browser Bot │────▶│   Desktop   │             │
│  │ TikTok  │     │ (Puppeteer) │     │   (Main)    │             │
│  │  LINE   │     └─────────────┘     └──────┬──────┘             │
│  └─────────┘                                │                     │
│                                             ▼                     │
│  ┌─────────┐     ┌─────────────┐     ┌─────────────┐             │
│  │ 📱 Live │────▶│ Mobile App  │────▶│  SignalR    │             │
│  │ Stream  │     │ (Capture)   │     │    Hub      │             │
│  └─────────┘     └─────────────┘     └──────┬──────┘             │
│                                             ▼                     │
│                               ┌─────────────────────┐            │
│                               │    CF Detector      │            │
│                               │  (Pattern Matching) │            │
│                               └──────────┬──────────┘            │
│                                          ▼                        │
│                               ┌─────────────────────┐            │
│                               │  Duplicate Filter   │            │
│                               │ (Customer + Product)│            │
│                               └──────────┬──────────┘            │
│                                          ▼                        │
│                               ┌─────────────────────┐            │
│                               │    Create Order     │            │
│                               └─────────────────────┘            │
│                                                                   │
└──────────────────────────────────────────────────────────────────┘
```

---

## 🔐 Security Architecture

### License System Integration

```
┌──────────────────────────────────────────────────────────────────┐
│                    LICENSE & SECURITY                             │
├──────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌─────────────┐        HTTPS         ┌─────────────────────┐   │
│  │  Desktop    │◀─────────────────────▶│   XMan Studio       │   │
│  │    App      │                       │   License Server    │   │
│  └──────┬──────┘                       │                     │   │
│         │                              │  • Validate License │   │
│         │                              │  • Feature Flags    │   │
│         ▼                              │  • Usage Analytics  │   │
│  ┌─────────────┐                       │  • Remote Config    │   │
│  │  Local      │                       └─────────────────────┘   │
│  │  Cache      │                                                 │
│  │ (Encrypted) │                       Repository:               │
│  └─────────────┘                       github.com/xjanova/       │
│                                        xmanstudio                │
│                                                                   │
└──────────────────────────────────────────────────────────────────┘
```

### Data Protection

| ข้อมูล | การป้องกัน |
|--------|-----------|
| License Key | AES-256 Encryption |
| Customer Data | SQLite Encryption |
| API Keys | Secure Storage |
| Slip Images | Local Only (No Cloud) |

---

## 📡 API Endpoints (Built-in Web Server)

### Customer Portal API

```
GET  /api/orders/{trackingNumber}    # ลูกค้าเช็คสถานะ
GET  /api/orders/{id}/slip           # ดูสลิปการชำระเงิน
```

### OBS Overlay API

```
GET  /overlay/stats                  # สถิติยอดขายปัจจุบัน
GET  /overlay/latest-orders          # ออเดอร์ล่าสุด
WS   /overlay/realtime               # Real-time Updates
```

### Internal API (SignalR)

```
Hub: /livehub
├── OnChatMessage(message)           # รับข้อความแชท
├── OnOrderCreated(order)            # ออเดอร์ใหม่
├── OnPaymentReceived(payment)       # ได้รับชำระเงิน
└── OnOrderStatusChanged(status)     # สถานะออเดอร์เปลี่ยน
```

---

<p align="center">
  <strong>Live x Shop Pro Architecture</strong><br>
  Version 1.0 | Xman Studio
</p>
