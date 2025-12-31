# 🗄️ โครงสร้างฐานข้อมูล Live x Shop Pro

> Database Schema Documentation - SQLite + Entity Framework Core

---

## 📋 สารบัญ

1. [ภาพรวม ERD](#ภาพรวม-erd)
2. [ตารางหลัก](#ตารางหลัก)
3. [ตารางสนับสนุน](#ตารางสนับสนุน)
4. [ความสัมพันธ์](#ความสัมพันธ์)
5. [Indexes และ Constraints](#indexes-และ-constraints)

---

## 📊 ภาพรวม ERD

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                           DATABASE SCHEMA                                    │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ┌─────────────┐         ┌─────────────┐         ┌─────────────┐           │
│  │  Customers  │◀───────▶│   Orders    │◀───────▶│ OrderItems  │           │
│  └─────────────┘    1:N  └──────┬──────┘    1:N  └──────┬──────┘           │
│                                 │                       │                   │
│                                 │ 1:N                   │ N:1               │
│                                 ▼                       ▼                   │
│                          ┌─────────────┐         ┌─────────────┐           │
│                          │  Payments   │         │  Products   │           │
│                          └─────────────┘         └──────┬──────┘           │
│                                                         │ 1:N               │
│  ┌─────────────┐         ┌─────────────┐         ┌──────▼──────┐           │
│  │LiveSessions │◀───────▶│ChatMessages │         │  Variants   │           │
│  └─────────────┘    1:N  └─────────────┘         └─────────────┘           │
│                                                                              │
│  ┌─────────────┐         ┌─────────────┐         ┌─────────────┐           │
│  │  Shipments  │◀────────│   Orders    │         │  Settings   │           │
│  └─────────────┘    1:1  └─────────────┘         └─────────────┘           │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## 📦 ตารางหลัก

### 1. Customers (ลูกค้า)

```sql
CREATE TABLE Customers (
    -- Primary Key
    Id                  TEXT PRIMARY KEY,       -- GUID

    -- ข้อมูลพื้นฐาน
    Name                TEXT NOT NULL,          -- ชื่อลูกค้า
    Nickname            TEXT,                   -- ชื่อเล่น
    Phone               TEXT,                   -- เบอร์โทร
    Email               TEXT,                   -- อีเมล

    -- ที่อยู่
    Address             TEXT,                   -- ที่อยู่
    Province            TEXT,                   -- จังหวัด
    District            TEXT,                   -- อำเภอ
    SubDistrict         TEXT,                   -- ตำบล
    PostalCode          TEXT,                   -- รหัสไปรษณีย์

    -- ข้อมูล Social
    FacebookId          TEXT,                   -- Facebook User ID
    FacebookName        TEXT,                   -- ชื่อ Facebook
    TikTokId            TEXT,                   -- TikTok User ID
    TikTokName          TEXT,                   -- ชื่อ TikTok
    LineUserId          TEXT,                   -- LINE User ID

    -- สถิติ
    TotalOrders         INTEGER DEFAULT 0,      -- จำนวนออเดอร์ทั้งหมด
    TotalSpent          REAL DEFAULT 0,         -- ยอดซื้อสะสม
    LastOrderDate       TEXT,                   -- วันที่สั่งล่าสุด

    -- สถานะ
    CustomerType        TEXT DEFAULT 'Normal',  -- ประเภท: Normal/VIP/Blocked
    Notes               TEXT,                   -- หมายเหตุ
    IsActive            INTEGER DEFAULT 1,      -- เปิดใช้งาน

    -- Timestamps
    CreatedAt           TEXT NOT NULL,          -- วันที่สร้าง
    UpdatedAt           TEXT                    -- วันที่แก้ไข
);
```

### 2. Products (สินค้า)

```sql
CREATE TABLE Products (
    -- Primary Key
    Id                  TEXT PRIMARY KEY,       -- GUID

    -- รหัสสินค้า
    SKU                 TEXT UNIQUE,            -- Stock Keeping Unit
    Barcode             TEXT,                   -- Barcode
    LiveCode            TEXT,                   -- รหัสสำหรับไลฟ์ (CF1, CF2)

    -- ข้อมูลสินค้า
    Name                TEXT NOT NULL,          -- ชื่อสินค้า
    Description         TEXT,                   -- รายละเอียด
    CategoryId          TEXT,                   -- หมวดหมู่ (FK)

    -- ราคา
    Price               REAL NOT NULL,          -- ราคาขาย
    Cost                REAL DEFAULT 0,         -- ราคาทุน
    CompareAtPrice      REAL,                   -- ราคาเปรียบเทียบ (ก่อนลด)

    -- สต็อก
    StockQuantity       INTEGER DEFAULT 0,      -- จำนวนในสต็อก
    ReorderLevel        INTEGER DEFAULT 5,      -- แจ้งเตือนเมื่อต่ำกว่า
    TrackStock          INTEGER DEFAULT 1,      -- ติดตามสต็อก

    -- รูปภาพ
    MainImage           TEXT,                   -- รูปหลัก (Path/URL)

    -- น้ำหนักและขนาด (สำหรับขนส่ง)
    Weight              REAL DEFAULT 0,         -- น้ำหนัก (กรัม)
    Width               REAL DEFAULT 0,         -- กว้าง (ซม.)
    Height              REAL DEFAULT 0,         -- สูง (ซม.)
    Length              REAL DEFAULT 0,         -- ยาว (ซม.)

    -- สถานะ
    IsActive            INTEGER DEFAULT 1,      -- เปิดใช้งาน
    IsFeatured          INTEGER DEFAULT 0,      -- สินค้าแนะนำ

    -- Timestamps
    CreatedAt           TEXT NOT NULL,
    UpdatedAt           TEXT
);
```

### 3. Orders (ออเดอร์)

```sql
CREATE TABLE Orders (
    -- Primary Key
    Id                  TEXT PRIMARY KEY,       -- GUID
    OrderNumber         TEXT UNIQUE NOT NULL,   -- เลขที่ออเดอร์ (ORD-20241225-001)

    -- ลูกค้า
    CustomerId          TEXT NOT NULL,          -- FK -> Customers
    CustomerName        TEXT,                   -- ชื่อลูกค้า (snapshot)
    CustomerPhone       TEXT,                   -- เบอร์โทร (snapshot)

    -- ที่อยู่จัดส่ง
    ShippingAddress     TEXT,                   -- ที่อยู่จัดส่ง
    ShippingProvince    TEXT,
    ShippingDistrict    TEXT,
    ShippingSubDistrict TEXT,
    ShippingPostalCode  TEXT,

    -- ยอดเงิน
    SubTotal            REAL NOT NULL,          -- ยอดรวมสินค้า
    ShippingFee         REAL DEFAULT 0,         -- ค่าจัดส่ง
    Discount            REAL DEFAULT 0,         -- ส่วนลด
    Total               REAL NOT NULL,          -- ยอดรวมทั้งหมด

    -- สถานะ
    Status              TEXT DEFAULT 'Pending', -- สถานะออเดอร์
    PaymentStatus       TEXT DEFAULT 'Unpaid',  -- สถานะชำระเงิน
    ShippingStatus      TEXT DEFAULT 'Pending', -- สถานะจัดส่ง

    -- แหล่งที่มา
    Source              TEXT DEFAULT 'Live',    -- Live/POS/Manual
    Platform            TEXT,                   -- Facebook/TikTok/LINE
    LiveSessionId       TEXT,                   -- FK -> LiveSessions

    -- หมายเหตุ
    CustomerNote        TEXT,                   -- หมายเหตุจากลูกค้า
    AdminNote           TEXT,                   -- หมายเหตุจากแอดมิน

    -- Timestamps
    CreatedAt           TEXT NOT NULL,
    UpdatedAt           TEXT,
    PaidAt              TEXT,                   -- วันที่ชำระ
    ShippedAt           TEXT,                   -- วันที่ส่ง
    CompletedAt         TEXT,                   -- วันที่เสร็จสิ้น
    CancelledAt         TEXT,                   -- วันที่ยกเลิก

    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
```

### 4. OrderItems (รายการสินค้าในออเดอร์)

```sql
CREATE TABLE OrderItems (
    -- Primary Key
    Id                  TEXT PRIMARY KEY,       -- GUID

    -- Foreign Keys
    OrderId             TEXT NOT NULL,          -- FK -> Orders
    ProductId           TEXT NOT NULL,          -- FK -> Products
    VariantId           TEXT,                   -- FK -> ProductVariants

    -- ข้อมูลสินค้า (snapshot)
    ProductName         TEXT NOT NULL,          -- ชื่อสินค้า
    ProductSKU          TEXT,                   -- SKU
    VariantName         TEXT,                   -- ชื่อ Variant

    -- ราคาและจำนวน
    Quantity            INTEGER NOT NULL,       -- จำนวน
    UnitPrice           REAL NOT NULL,          -- ราคาต่อหน่วย
    Discount            REAL DEFAULT 0,         -- ส่วนลด
    Total               REAL NOT NULL,          -- ยอดรวม

    -- สถานะการแพค
    IsPacked            INTEGER DEFAULT 0,      -- แพคแล้วหรือยัง
    PackedAt            TEXT,                   -- วันที่แพค

    FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
```

### 5. Payments (การชำระเงิน)

```sql
CREATE TABLE Payments (
    -- Primary Key
    Id                  TEXT PRIMARY KEY,       -- GUID

    -- Foreign Keys
    OrderId             TEXT NOT NULL,          -- FK -> Orders

    -- ข้อมูลการชำระ
    Amount              REAL NOT NULL,          -- จำนวนเงิน
    PaymentMethod       TEXT NOT NULL,          -- วิธีชำระ: Transfer/Cash/COD

    -- ข้อมูลสลิป
    SlipImage           TEXT,                   -- รูปสลิป (Path)
    BankName            TEXT,                   -- ชื่อธนาคาร
    AccountNumber       TEXT,                   -- เลขบัญชีผู้โอน
    AccountName         TEXT,                   -- ชื่อบัญชีผู้โอน
    TransferDate        TEXT,                   -- วันที่โอน
    ReferenceNo         TEXT,                   -- เลขอ้างอิง

    -- การตรวจสอบ
    VerificationStatus  TEXT DEFAULT 'Pending', -- สถานะตรวจสอบ
    VerificationMode    TEXT,                   -- โหมด: Auto/Manual
    ConfidenceScore     REAL,                   -- คะแนนความมั่นใจ (0-100)
    VerifiedBy          TEXT,                   -- ผู้ตรวจสอบ (ถ้า Manual)
    VerifiedAt          TEXT,                   -- วันที่ตรวจสอบ
    VerificationNote    TEXT,                   -- หมายเหตุการตรวจสอบ

    -- SMS Matching
    SmsMatched          INTEGER DEFAULT 0,      -- ตรงกับ SMS หรือไม่
    SmsId               TEXT,                   -- FK -> BankSms

    -- สถานะ
    Status              TEXT DEFAULT 'Pending', -- Pending/Confirmed/Rejected

    -- Timestamps
    CreatedAt           TEXT NOT NULL,
    UpdatedAt           TEXT,

    FOREIGN KEY (OrderId) REFERENCES Orders(Id)
);
```

### 6. Shipments (การจัดส่ง)

```sql
CREATE TABLE Shipments (
    -- Primary Key
    Id                  TEXT PRIMARY KEY,       -- GUID

    -- Foreign Keys
    OrderId             TEXT UNIQUE NOT NULL,   -- FK -> Orders (1:1)

    -- ขนส่ง
    Carrier             TEXT NOT NULL,          -- Kerry/Flash/J&T/ThaiPost
    TrackingNumber      TEXT,                   -- เลข Tracking

    -- ข้อมูลพัสดุ
    Weight              REAL,                   -- น้ำหนัก
    Width               REAL,
    Height              REAL,
    Length              REAL,

    -- ค่าจัดส่ง
    ShippingCost        REAL,                   -- ค่าจัดส่ง
    CodAmount           REAL,                   -- ยอด COD (ถ้ามี)

    -- สถานะ
    Status              TEXT DEFAULT 'Pending', -- สถานะ

    -- ใบปะหน้า
    LabelPrinted        INTEGER DEFAULT 0,      -- พิมพ์ใบปะหน้าแล้ว
    LabelPrintedAt      TEXT,

    -- Tracking History (JSON)
    TrackingHistory     TEXT,                   -- JSON Array

    -- Timestamps
    CreatedAt           TEXT NOT NULL,
    UpdatedAt           TEXT,
    ShippedAt           TEXT,                   -- วันที่ส่ง
    DeliveredAt         TEXT,                   -- วันที่ส่งถึง

    FOREIGN KEY (OrderId) REFERENCES Orders(Id)
);
```

---

## 📺 ตารางไลฟ์และแชท

### 7. LiveSessions (เซสชันไลฟ์)

```sql
CREATE TABLE LiveSessions (
    -- Primary Key
    Id                  TEXT PRIMARY KEY,       -- GUID

    -- ข้อมูลไลฟ์
    Title               TEXT,                   -- ชื่อไลฟ์
    Platform            TEXT NOT NULL,          -- Facebook/TikTok/LINE
    StreamUrl           TEXT,                   -- URL ของไลฟ์

    -- สถิติ
    TotalMessages       INTEGER DEFAULT 0,      -- จำนวนข้อความ
    TotalOrders         INTEGER DEFAULT 0,      -- จำนวนออเดอร์
    TotalSales          REAL DEFAULT 0,         -- ยอดขาย
    PeakViewers         INTEGER DEFAULT 0,      -- ยอดผู้ชมสูงสุด

    -- สถานะ
    Status              TEXT DEFAULT 'Active',  -- Active/Ended/Cancelled

    -- Timestamps
    StartedAt           TEXT NOT NULL,
    EndedAt             TEXT
);
```

### 8. ChatMessages (ข้อความแชท)

```sql
CREATE TABLE ChatMessages (
    -- Primary Key
    Id                  TEXT PRIMARY KEY,       -- GUID

    -- Foreign Keys
    LiveSessionId       TEXT NOT NULL,          -- FK -> LiveSessions
    CustomerId          TEXT,                   -- FK -> Customers (ถ้า match ได้)

    -- ผู้ส่ง
    UserId              TEXT NOT NULL,          -- Platform User ID
    UserName            TEXT NOT NULL,          -- ชื่อผู้ใช้
    ProfilePicture      TEXT,                   -- รูปโปรไฟล์

    -- ข้อความ
    Message             TEXT NOT NULL,          -- ข้อความ
    Platform            TEXT NOT NULL,          -- Facebook/TikTok/LINE

    -- CF Detection
    IsCF                INTEGER DEFAULT 0,      -- เป็น CF หรือไม่
    CFProducts          TEXT,                   -- JSON: [{"code": "1", "qty": 2}]
    ProcessedToOrder    INTEGER DEFAULT 0,      -- สร้างออเดอร์แล้วหรือยัง
    OrderId             TEXT,                   -- FK -> Orders

    -- Duplicate Check
    IsDuplicate         INTEGER DEFAULT 0,      -- ซ้ำหรือไม่
    DuplicateOf         TEXT,                   -- FK -> ChatMessages

    -- Timestamps
    ReceivedAt          TEXT NOT NULL,          -- เวลาที่ได้รับ

    FOREIGN KEY (LiveSessionId) REFERENCES LiveSessions(Id),
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
```

---

## 💰 ตาราง SMS ธนาคาร

### 9. BankSms (SMS ธนาคาร)

```sql
CREATE TABLE BankSms (
    -- Primary Key
    Id                  TEXT PRIMARY KEY,       -- GUID

    -- ข้อมูล SMS
    Sender              TEXT NOT NULL,          -- ผู้ส่ง (KBANK, SCB, ...)
    Message             TEXT NOT NULL,          -- ข้อความ
    ReceivedAt          TEXT NOT NULL,          -- เวลาที่ได้รับ

    -- ข้อมูลที่ Parse ได้
    BankName            TEXT,                   -- ชื่อธนาคาร
    Amount              REAL,                   -- จำนวนเงิน
    TransferFrom        TEXT,                   -- เลขบัญชีผู้โอน
    TransferDate        TEXT,                   -- วันที่โอน
    ReferenceNo         TEXT,                   -- เลขอ้างอิง
    Balance             REAL,                   -- ยอดคงเหลือ

    -- Matching
    IsMatched           INTEGER DEFAULT 0,      -- Match กับ Payment แล้ว
    PaymentId           TEXT,                   -- FK -> Payments

    FOREIGN KEY (PaymentId) REFERENCES Payments(Id)
);
```

---

## 📁 ตารางสนับสนุน

### 10. Categories (หมวดหมู่สินค้า)

```sql
CREATE TABLE Categories (
    Id                  TEXT PRIMARY KEY,
    Name                TEXT NOT NULL,
    ParentId            TEXT,                   -- Self-reference
    SortOrder           INTEGER DEFAULT 0,
    IsActive            INTEGER DEFAULT 1,

    FOREIGN KEY (ParentId) REFERENCES Categories(Id)
);
```

### 11. ProductVariants (ตัวเลือกสินค้า)

```sql
CREATE TABLE ProductVariants (
    Id                  TEXT PRIMARY KEY,
    ProductId           TEXT NOT NULL,
    Name                TEXT NOT NULL,          -- ชื่อ Variant (เช่น "ไซส์ M - สีดำ")
    SKU                 TEXT,
    Barcode             TEXT,
    Price               REAL,                   -- ราคา (ถ้าต่างจากหลัก)
    StockQuantity       INTEGER DEFAULT 0,
    IsActive            INTEGER DEFAULT 1,

    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE
);
```

### 12. ProductImages (รูปสินค้า)

```sql
CREATE TABLE ProductImages (
    Id                  TEXT PRIMARY KEY,
    ProductId           TEXT NOT NULL,
    ImagePath           TEXT NOT NULL,          -- Path รูปภาพ
    SortOrder           INTEGER DEFAULT 0,
    IsMain              INTEGER DEFAULT 0,

    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE
);
```

### 13. Settings (การตั้งค่า)

```sql
CREATE TABLE Settings (
    Key                 TEXT PRIMARY KEY,       -- Key ของการตั้งค่า
    Value               TEXT,                   -- Value (JSON หรือ Text)
    Category            TEXT,                   -- หมวดหมู่
    Description         TEXT                    -- คำอธิบาย
);

-- ตัวอย่างข้อมูล
INSERT INTO Settings (Key, Value, Category) VALUES
('shop_name', 'ร้านค้าตัวอย่าง', 'general'),
('shop_phone', '081-234-5678', 'general'),
('slip_verification_mode', 'auto', 'payment'),
('duplicate_check_minutes', '5', 'order'),
('default_carrier', 'Kerry', 'shipping');
```

### 14. AuditLogs (บันทึกการใช้งาน)

```sql
CREATE TABLE AuditLogs (
    Id                  TEXT PRIMARY KEY,
    Action              TEXT NOT NULL,          -- Create/Update/Delete
    EntityType          TEXT NOT NULL,          -- ชื่อ Entity
    EntityId            TEXT NOT NULL,          -- ID ของ Entity
    OldValues           TEXT,                   -- JSON ค่าเดิม
    NewValues           TEXT,                   -- JSON ค่าใหม่
    UserId              TEXT,                   -- ผู้ทำรายการ
    IpAddress           TEXT,
    CreatedAt           TEXT NOT NULL
);
```

---

## 🔗 Indexes และ Constraints

### Indexes

```sql
-- Customers
CREATE INDEX IX_Customers_Phone ON Customers(Phone);
CREATE INDEX IX_Customers_FacebookId ON Customers(FacebookId);
CREATE INDEX IX_Customers_TikTokId ON Customers(TikTokId);

-- Products
CREATE INDEX IX_Products_SKU ON Products(SKU);
CREATE INDEX IX_Products_Barcode ON Products(Barcode);
CREATE INDEX IX_Products_LiveCode ON Products(LiveCode);

-- Orders
CREATE INDEX IX_Orders_CustomerId ON Orders(CustomerId);
CREATE INDEX IX_Orders_Status ON Orders(Status);
CREATE INDEX IX_Orders_CreatedAt ON Orders(CreatedAt);
CREATE INDEX IX_Orders_LiveSessionId ON Orders(LiveSessionId);

-- OrderItems
CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
CREATE INDEX IX_OrderItems_ProductId ON OrderItems(ProductId);

-- Payments
CREATE INDEX IX_Payments_OrderId ON Payments(OrderId);
CREATE INDEX IX_Payments_ReferenceNo ON Payments(ReferenceNo);

-- Shipments
CREATE INDEX IX_Shipments_TrackingNumber ON Shipments(TrackingNumber);
CREATE INDEX IX_Shipments_Status ON Shipments(Status);

-- ChatMessages
CREATE INDEX IX_ChatMessages_LiveSessionId ON ChatMessages(LiveSessionId);
CREATE INDEX IX_ChatMessages_UserId ON ChatMessages(UserId);
CREATE INDEX IX_ChatMessages_ReceivedAt ON ChatMessages(ReceivedAt);

-- BankSms
CREATE INDEX IX_BankSms_ReceivedAt ON BankSms(ReceivedAt);
CREATE INDEX IX_BankSms_ReferenceNo ON BankSms(ReferenceNo);
```

---

## 📝 Entity Framework Configuration

```csharp
// ตัวอย่าง Configuration
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // ตั้งค่าตาราง
        builder.ToTable("Orders");

        // Primary Key
        builder.HasKey(o => o.Id);

        // Properties
        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(o => o.Total)
            .HasColumnType("decimal(18,2)");

        // Relationships
        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId);

        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(o => o.OrderNumber).IsUnique();
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.CreatedAt);
    }
}
```

---

<p align="center">
  <strong>Live x Shop Pro Database Schema</strong><br>
  Version 1.0 | SQLite + Entity Framework Core
</p>
