# 🛒 Live x Shop Pro

> ระบบจัดการไลฟ์ขายของครบวงจร | พัฒนาโดย **Xman Studio**

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![WPF](https://img.shields.io/badge/WPF-Desktop-0078D6?style=for-the-badge&logo=windows)](https://docs.microsoft.com/wpf/)
[![SQLite](https://img.shields.io/badge/SQLite-Database-003B57?style=for-the-badge&logo=sqlite)](https://www.sqlite.org/)
[![License](https://img.shields.io/badge/License-Proprietary-red?style=for-the-badge)](LICENSE)

---

## 📋 สารบัญ

- [เกี่ยวกับโปรเจค](#-เกี่ยวกับโปรเจค)
- [ฟีเจอร์หลัก](#-ฟีเจอร์หลัก)
- [สถาปัตยกรรม](#-สถาปัตยกรรม)
- [การติดตั้ง](#-การติดตั้ง)
- [การใช้งาน](#-การใช้งาน)
- [เอกสารประกอบ](#-เอกสารประกอบ)
- [การพัฒนา](#-การพัฒนา)
- [ติดต่อ](#-ติดต่อ)

---

## 📖 เกี่ยวกับโปรเจค

**Live x Shop Pro** เป็นระบบจัดการไลฟ์ขายของแบบครบวงจร ออกแบบมาเพื่อผู้ประกอบการไทยที่ขายสินค้าผ่าน Facebook Live, TikTok Live และ LINE โดยเฉพาะ

### ✨ จุดเด่น

```
┌─────────────────────────────────────────────────────────────┐
│  🎯 All-in-One Solution                                     │
│  ─────────────────────                                      │
│  • ดูดแชทจากทุกแพลตฟอร์มในที่เดียว                          │
│  • ตรวจจับคำสั่งซื้ออัตโนมัติ (CF Detection)                 │
│  • ระบบตรวจสลิปปลอมป้องกันโกง                               │
│  • เชื่อมต่อขนส่งพร้อมพิมพ์ใบปะหน้า                          │
│                                                             │
│  🔗 Multi-Device Support                                    │
│  ─────────────────────                                      │
│  • แอพมือถือดูดแชทเมื่อไลฟ์ผ่านโทรศัพท์                      │
│  • หน้าจอแพคของแยกเชื่อมต่อ LAN/VPN                          │
│  • ระบบ Overlay สำหรับ OBS                                  │
│                                                             │
│  💰 Cost Effective                                          │
│  ─────────────────────                                      │
│  • ซื้อขาด ไม่มีค่ารายเดือน                                  │
│  • ทำงาน Offline ได้                                        │
│  • ข้อมูลอยู่ในเครื่องของคุณ                                  │
└─────────────────────────────────────────────────────────────┘
```

---

## 🚀 ฟีเจอร์หลัก

### 📺 ดูดแชท Multi-Platform
| Platform | Browser Bot | Mobile App | สถานะ |
|----------|:-----------:|:----------:|:-----:|
| Facebook Live | ✅ | ✅ | พัฒนา |
| TikTok Live | ✅ | ✅ | พัฒนา |
| LINE OA | ✅ | - | พัฒนา |

### 💳 ระบบการเงิน
- **OCR สลิป** - อ่านสลิปอัตโนมัติด้วย AI
- **ตรวจ SMS ธนาคาร** - ยืนยันยอดเงินเข้าอัตโนมัติ
- **ตรวจสลิปปลอม** - ป้องกันการโกง พร้อมโหมดตั้งค่าได้
- **รายงานยอดขาย** - สรุปยอดแบบ Real-time

### 📦 จัดการคลังและขนส่ง
- **POS System** - ขายหน้าร้านได้
- **Stock Management** - จัดการสต็อกสินค้า
- **Barcode Support** - พิมพ์และสแกน Barcode
- **Shipping Integration** - Kerry / Flash / J&T

### 🖥️ Multi-Screen System
- **หน้าจอแพคของ** - แสดงออเดอร์แบบ Real-time
- **เชื่อมต่อ LAN/VPN** - ทำงานคนละที่ได้
- **OBS Overlay** - แสดงยอดขายบนไลฟ์

---

## 🏗️ สถาปัตยกรรม

โปรเจคใช้ **Clean Architecture** แบ่งเป็น 4 Layer:

```
┌─────────────────────────────────────────────────────────────┐
│                    📱 Desktop (WPF)                         │
│              UI, Views, ViewModels, Themes                  │
├─────────────────────────────────────────────────────────────┤
│                 🔧 Infrastructure                           │
│      Database, External APIs, Services Implementation       │
├─────────────────────────────────────────────────────────────┤
│                  📋 Application                             │
│          Use Cases, CQRS, Business Logic                    │
├─────────────────────────────────────────────────────────────┤
│                    💎 Core                                  │
│           Entities, Interfaces, Domain Events               │
└─────────────────────────────────────────────────────────────┘
```

### Tech Stack

| Layer | เทคโนโลยี |
|-------|----------|
| **Framework** | .NET 10 |
| **Desktop UI** | WPF + MaterialDesignInXAML + MahApps.Metro |
| **Mobile** | .NET MAUI |
| **Database** | SQLite + Entity Framework Core |
| **Real-time** | SignalR |
| **Web Server** | EmbedIO (Built-in) |

---

## 💻 การติดตั้ง

### ความต้องการของระบบ

| รายการ | ขั้นต่ำ | แนะนำ |
|--------|--------|------|
| **OS** | Windows 10 (1903+) | Windows 11 |
| **RAM** | 4 GB | 8 GB |
| **Storage** | 500 MB | 2 GB |
| **Display** | 1366x768 | 1920x1080 |

### สำหรับผู้ใช้งาน

1. ดาวน์โหลด Installer จาก [Releases](https://github.com/xjanova/Livexshoppro/releases)
2. รัน `LiveXShopPro-Setup.exe`
3. ทำตามขั้นตอนการติดตั้ง
4. เปิดใช้งานด้วย License Key

### สำหรับนักพัฒนา

```bash
# Clone repository
git clone https://github.com/xjanova/Livexshoppro.git
cd Livexshoppro

# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run desktop app
dotnet run --project src/Desktop/LiveXShopPro.Desktop.csproj
```

---

## 📚 เอกสารประกอบ

| เอกสาร | รายละเอียด |
|--------|-----------|
| [ARCHITECTURE.md](docs/ARCHITECTURE.md) | รายละเอียดสถาปัตยกรรมระบบ |
| [FEATURES.md](docs/FEATURES.md) | รายละเอียดฟีเจอร์ทั้งหมด |
| [DATABASE.md](docs/DATABASE.md) | โครงสร้างฐานข้อมูล |
| [API.md](docs/API.md) | เอกสาร API |
| [ROADMAP.md](docs/ROADMAP.md) | แผนพัฒนา |

---

## 🛠️ การพัฒนา

### โครงสร้างโปรเจค

```
LiveXShopPro/
├── src/
│   ├── Core/                 # Domain Layer
│   │   ├── Entities/         # Domain Entities
│   │   ├── Interfaces/       # Repository Interfaces
│   │   ├── Enums/           # Enumerations
│   │   └── Events/          # Domain Events
│   │
│   ├── Application/          # Application Layer
│   │   ├── Features/        # Use Cases (CQRS)
│   │   ├── Common/          # Shared Logic
│   │   └── Services/        # Application Services
│   │
│   ├── Infrastructure/       # Infrastructure Layer
│   │   ├── Data/            # EF Core, Repositories
│   │   ├── Services/        # External Service Implementations
│   │   └── External/        # Third-party Integrations
│   │
│   ├── Desktop/             # WPF Desktop App
│   │   ├── Views/           # XAML Views
│   │   ├── ViewModels/      # MVVM ViewModels
│   │   ├── Themes/          # Dark Theme, Glassmorphism
│   │   └── Resources/       # Fonts, Images
│   │
│   └── Mobile/              # .NET MAUI Mobile App
│
├── tests/                    # Unit Tests
├── docs/                     # Documentation
└── scripts/                  # Build Scripts
```

### การ Contribute

1. Fork repository
2. สร้าง feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'เพิ่มฟีเจอร์ใหม่'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. สร้าง Pull Request

---

## 🔗 ระบบที่เกี่ยวข้อง

| Repository | รายละเอียด |
|------------|-----------|
| [Livexshoppro](https://github.com/xjanova/Livexshoppro) | โปรเจคหลัก |
| [xmanstudio](https://github.com/xjanova/xmanstudio) | ระบบ License และ Admin Panel |

---

## 📞 ติดต่อ

**Xman Studio**

- 🌐 Website: [xmanstudio.com](https://xmanstudio.com)
- 📧 Email: support@xmanstudio.com
- 📱 LINE: @xmanstudio

---

## 📄 License

โปรเจคนี้เป็น **Proprietary Software** ของ Xman Studio

ดูรายละเอียดเพิ่มเติมที่ [LICENSE](LICENSE)

---

<p align="center">
  พัฒนาด้วย ❤️ โดย <strong>Xman Studio</strong>
</p>
