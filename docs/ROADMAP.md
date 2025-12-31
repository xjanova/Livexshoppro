# 🗺️ Development Roadmap - Live x Shop Pro

> แผนพัฒนาโปรเจค Live x Shop Pro

---

## 📊 ภาพรวมการพัฒนา

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                        DEVELOPMENT PHASES                                    │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  Phase 1: Foundation          ████████████░░░░░░░░░░░░░░░░░░░░  40%        │
│  ──────────────────────                                                     │
│  โครงสร้างพื้นฐาน, Database, Core Entities                                  │
│                                                                              │
│  Phase 2: Core Features       ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░   0%        │
│  ──────────────────────                                                     │
│  Chat Capture, CF Detection, Order Management                               │
│                                                                              │
│  Phase 3: Payment & Shipping  ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░   0%        │
│  ──────────────────────                                                     │
│  OCR, Slip Verification, Shipping Integration                               │
│                                                                              │
│  Phase 4: Advanced Features   ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░   0%        │
│  ──────────────────────                                                     │
│  Mobile App, Packing Station, OBS Overlay                                   │
│                                                                              │
│  Phase 5: Polish & Release    ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░   0%        │
│  ──────────────────────                                                     │
│  Testing, Bug Fixes, Documentation, Release                                 │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## 🏗️ Phase 1: Foundation (โครงสร้างพื้นฐาน)

### สถานะ: 🔄 กำลังดำเนินการ

### ✅ เสร็จแล้ว

| งาน | รายละเอียด |
|-----|-----------|
| Project Structure | สร้างโครงสร้าง Clean Architecture |
| Solution & Projects | สร้าง .sln และ .csproj ทั้งหมด |
| Documentation | README, ARCHITECTURE, FEATURES, DATABASE, API |
| Theme & Styles | Dark Theme, Glassmorphism XAML |
| Git Setup | Repository, .gitignore, LICENSE |

### 📋 ยังไม่ได้ทำ

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| Core Entities | สร้าง Domain Entities ทั้งหมด | 🔴 High |
| DbContext | สร้าง EF Core DbContext | 🔴 High |
| Repositories | Implement Repository Pattern | 🔴 High |
| DI Container | ตั้งค่า Dependency Injection | 🟡 Medium |
| Logging | ตั้งค่า Serilog | 🟡 Medium |
| App Shell | สร้างโครงหน้าต่างหลัก | 🔴 High |
| Navigation | ระบบนำทางระหว่างหน้า | 🔴 High |

---

## 💬 Phase 2: Core Features (ฟีเจอร์หลัก)

### สถานะ: ⏳ รอดำเนินการ

### Chat Capture System

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| Facebook Bot | Browser Bot ดูดแชท FB Live | 🔴 High |
| TikTok Bot | Browser Bot ดูดแชท TikTok | 🔴 High |
| LINE Integration | LINE Messaging API | 🟡 Medium |
| Chat UI | หน้าแสดงแชท Real-time | 🔴 High |
| Chat Storage | บันทึกแชทลง Database | 🔴 High |

### CF Detection System

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| Pattern Matching | ตรวจจับรูปแบบ CF | 🔴 High |
| Multi-Product | รองรับสั่งหลายสินค้า | 🔴 High |
| Thai Language | รองรับภาษาไทย | 🔴 High |
| Duplicate Filter | กรอง CF ซ้ำ | 🔴 High |

### Order Management

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| Order Creation | สร้างออเดอร์จาก CF | 🔴 High |
| Order List | หน้ารายการออเดอร์ | 🔴 High |
| Order Detail | หน้ารายละเอียด | 🔴 High |
| Order Status | ระบบสถานะออเดอร์ | 🔴 High |
| Manual Order | สร้างออเดอร์ Manual | 🟡 Medium |

### Product Management

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| Product CRUD | เพิ่ม/แก้ไข/ลบสินค้า | 🔴 High |
| Live Code | รหัสสินค้าสำหรับไลฟ์ | 🔴 High |
| Stock Tracking | ติดตามสต็อก | 🔴 High |
| Variants | ตัวเลือกสินค้า | 🟡 Medium |
| Images | จัดการรูปสินค้า | 🟡 Medium |

### Customer Management

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| Auto Create | สร้างลูกค้าอัตโนมัติ | 🔴 High |
| Customer List | หน้ารายการลูกค้า | 🔴 High |
| Customer Detail | หน้าข้อมูลลูกค้า | 🟡 Medium |
| Order History | ประวัติสั่งซื้อ | 🟡 Medium |
| Address Book | ที่อยู่จัดส่ง | 🟡 Medium |

---

## 💳 Phase 3: Payment & Shipping

### สถานะ: ⏳ รอดำเนินการ

### Payment System

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| Slip Upload | อัพโหลดสลิป | 🔴 High |
| OCR Engine | Tesseract OCR | 🔴 High |
| Bank Parsing | Parse ข้อมูลจากสลิป | 🔴 High |
| Slip Verification | ตรวจสอบสลิปปลอม | 🔴 High |
| SMS Reading | อ่าน SMS ธนาคาร | 🟡 Medium |
| Auto Match | จับคู่ SMS กับออเดอร์ | 🟡 Medium |
| Verification Modes | โหมด Auto/Manual/Hybrid | 🔴 High |

### Shipping Integration

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| Kerry API | เชื่อมต่อ Kerry Express | 🔴 High |
| Flash API | เชื่อมต่อ Flash Express | 🔴 High |
| J&T API | เชื่อมต่อ J&T Express | 🟡 Medium |
| Label Printing | พิมพ์ใบปะหน้า | 🔴 High |
| Tracking Sync | ติดตามสถานะอัตโนมัติ | 🟡 Medium |
| COD Support | รองรับเก็บเงินปลายทาง | 🔴 High |

### POS System

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| POS UI | หน้าขายหน้าร้าน | 🟡 Medium |
| Barcode Scan | สแกน Barcode | 🟡 Medium |
| Cash Payment | รับเงินสด | 🟡 Medium |
| Receipt Print | พิมพ์ใบเสร็จ | 🟡 Medium |
| Shift Management | เปิด/ปิดกะ | 🟢 Low |

---

## 📱 Phase 4: Advanced Features

### สถานะ: ⏳ รอดำเนินการ

### Mobile App

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| MAUI Project | สร้างโปรเจค MAUI | 🔴 High |
| Network Discovery | ค้นหา Desktop ใน LAN | 🔴 High |
| QR Pairing | จับคู่ด้วย QR Code | 🔴 High |
| SignalR Client | เชื่อมต่อ Real-time | 🔴 High |
| Chat Capture | ดูดแชทจากมือถือ | 🔴 High |
| Android Build | Build APK | 🔴 High |
| iOS Build | Build IPA | 🟡 Medium |

### Packing Station

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| Station Mode | โหมดหน้าจอแพค | 🔴 High |
| Order Queue | คิวออเดอร์ | 🔴 High |
| Barcode Verify | ยืนยันสินค้าด้วย Barcode | 🟡 Medium |
| Label Print | พิมพ์ใบปะหน้า | 🔴 High |
| LAN/VPN Connect | เชื่อมต่อระยะไกล | 🔴 High |

### Built-in Web Server

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| EmbedIO Setup | ตั้งค่า Web Server | 🔴 High |
| SignalR Hub | ตั้งค่า Real-time Hub | 🔴 High |
| Customer Portal | หน้าเช็คสถานะ | 🟡 Medium |
| API Endpoints | REST API | 🟡 Medium |

### OBS Overlay

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| Overlay Pages | หน้า Overlay สำหรับ OBS | 🟡 Medium |
| Sales Counter | ยอดขาย Real-time | 🟡 Medium |
| Latest Orders | ออเดอร์ล่าสุด | 🟡 Medium |
| WebSocket | Real-time Updates | 🟡 Medium |

### License System

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| XMan API Client | เชื่อมต่อ XMan Studio | 🔴 High |
| License Validation | ตรวจสอบ License | 🔴 High |
| Feature Flags | เปิด/ปิด Feature ตาม License | 🔴 High |
| Activation | Activate/Deactivate | 🔴 High |
| Offline Mode | ทำงานแบบ Offline | 🟡 Medium |

---

## 🎯 Phase 5: Polish & Release

### สถานะ: ⏳ รอดำเนินการ

### Testing

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| Unit Tests | ทดสอบ Unit | 🔴 High |
| Integration Tests | ทดสอบ Integration | 🔴 High |
| UI Tests | ทดสอบ UI | 🟡 Medium |
| Load Tests | ทดสอบ Load (แชทเยอะ) | 🔴 High |
| Beta Testing | ทดสอบกับผู้ใช้จริง | 🔴 High |

### Bug Fixes & Optimization

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| Bug Fixes | แก้ไข Bugs | 🔴 High |
| Performance | ปรับปรุงประสิทธิภาพ | 🔴 High |
| Memory Leaks | แก้ไข Memory Leaks | 🔴 High |
| UX Improvements | ปรับปรุง UX | 🟡 Medium |

### Documentation & Release

| งาน | รายละเอียด | Priority |
|-----|-----------|:--------:|
| User Manual | คู่มือการใช้งาน | 🔴 High |
| Video Tutorials | วิดีโอสอนการใช้งาน | 🟡 Medium |
| Installer | สร้าง Installer | 🔴 High |
| Auto Update | ระบบอัพเดทอัตโนมัติ | 🟡 Medium |
| Release Notes | บันทึกการเปลี่ยนแปลง | 🔴 High |

---

## 🎯 Milestones

### Milestone 1: MVP (Minimum Viable Product)

> ฟีเจอร์ขั้นต่ำที่ใช้งานได้จริง

- ✅ โครงสร้างโปรเจค
- ⬜ ดูดแชท Facebook Live
- ⬜ ตรวจจับ CF
- ⬜ สร้างออเดอร์อัตโนมัติ
- ⬜ จัดการสินค้า/สต็อก
- ⬜ OCR สลิป
- ⬜ พิมพ์ใบปะหน้า Kerry

### Milestone 2: Full Feature

> ฟีเจอร์ครบถ้วนตามที่ออกแบบ

- ⬜ TikTok Live Support
- ⬜ LINE OA Support
- ⬜ Mobile App
- ⬜ Packing Station
- ⬜ OBS Overlay
- ⬜ Customer Portal

### Milestone 3: Production Ready

> พร้อมใช้งานจริง

- ⬜ Full Testing
- ⬜ Bug Free
- ⬜ Documentation
- ⬜ Installer
- ⬜ License System

---

## 📝 หมายเหตุ

### Priority Legend

| Icon | ความหมาย |
|:----:|----------|
| 🔴 | High - สำคัญมาก ต้องทำก่อน |
| 🟡 | Medium - สำคัญ แต่ทำทีหลังได้ |
| 🟢 | Low - ทำเมื่อมีเวลา |

### Status Legend

| Icon | ความหมาย |
|:----:|----------|
| ✅ | เสร็จแล้ว |
| 🔄 | กำลังดำเนินการ |
| ⏳ | รอดำเนินการ |
| ⬜ | ยังไม่เริ่ม |

---

<p align="center">
  <strong>Live x Shop Pro Development Roadmap</strong><br>
  Version 1.0 | Xman Studio
</p>
