# 🔌 API Documentation - Live x Shop Pro

> เอกสาร API สำหรับ Built-in Web Server, SignalR Hub, และ External Integrations

---

## 📋 สารบัญ

1. [ภาพรวม API](#ภาพรวม-api)
2. [SignalR Hub API](#signalr-hub-api)
3. [REST API (Built-in Web Server)](#rest-api)
4. [External API Integrations](#external-api-integrations)
5. [XMan Studio License API](#xman-studio-license-api)

---

## 🎯 ภาพรวม API

### Server Configuration

```
┌─────────────────────────────────────────────────────────────────┐
│                    API ENDPOINTS                                 │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  Built-in Web Server: http://localhost:5000                     │
│  ──────────────────────────────────────────                     │
│  • REST API for Customer Portal                                 │
│  • OBS Overlay Endpoints                                        │
│  • Static File Serving                                          │
│                                                                  │
│  SignalR Hub: ws://localhost:5000/livehub                       │
│  ──────────────────────────────────────────                     │
│  • Real-time Chat Messages                                      │
│  • Order Notifications                                          │
│  • Packing Station Updates                                      │
│  • Mobile App Communication                                     │
│                                                                  │
│  mDNS Discovery: _livexshop._tcp.local                          │
│  ──────────────────────────────────────────                     │
│  • Auto-discovery for Mobile App                                │
│  • Packing Station Discovery                                    │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## 📡 SignalR Hub API

### Connection URL

```
ws://[HOST]:5000/livehub
```

### Hub Methods (Server → Client)

#### 1. OnChatMessageReceived
ส่งข้อความแชทใหม่ไปยัง Client

```csharp
// Method Signature
Task OnChatMessageReceived(ChatMessageDto message);

// ChatMessageDto
{
    "id": "guid",
    "liveSessionId": "guid",
    "userId": "fb_12345",
    "userName": "สมชาย",
    "profilePicture": "https://...",
    "message": "cf1 2ชิ้น",
    "platform": "Facebook",
    "isCF": true,
    "cfProducts": [
        { "code": "1", "quantity": 2 }
    ],
    "receivedAt": "2024-12-25T14:30:00Z"
}
```

#### 2. OnOrderCreated
แจ้งเตือนเมื่อมีออเดอร์ใหม่

```csharp
Task OnOrderCreated(OrderDto order);

// OrderDto
{
    "id": "guid",
    "orderNumber": "ORD-20241225-001",
    "customerName": "สมชาย",
    "customerPhone": "081-234-5678",
    "items": [
        {
            "productName": "เสื้อยืด #1",
            "quantity": 2,
            "unitPrice": 299,
            "total": 598
        }
    ],
    "subTotal": 598,
    "shippingFee": 50,
    "total": 648,
    "status": "Pending",
    "paymentStatus": "Unpaid",
    "source": "Live",
    "platform": "Facebook",
    "createdAt": "2024-12-25T14:30:00Z"
}
```

#### 3. OnPaymentReceived
แจ้งเตือนเมื่อได้รับการชำระเงิน

```csharp
Task OnPaymentReceived(PaymentDto payment);

// PaymentDto
{
    "id": "guid",
    "orderId": "guid",
    "orderNumber": "ORD-20241225-001",
    "amount": 648,
    "paymentMethod": "Transfer",
    "bankName": "KBANK",
    "verificationStatus": "Verified",
    "verifiedAt": "2024-12-25T14:35:00Z"
}
```

#### 4. OnOrderStatusChanged
แจ้งเตือนเมื่อสถานะออเดอร์เปลี่ยน

```csharp
Task OnOrderStatusChanged(OrderStatusDto status);

// OrderStatusDto
{
    "orderId": "guid",
    "orderNumber": "ORD-20241225-001",
    "oldStatus": "Pending",
    "newStatus": "Confirmed",
    "changedAt": "2024-12-25T14:40:00Z"
}
```

#### 5. OnLiveSessionStats
อัพเดทสถิติไลฟ์ (ทุก 5 วินาที)

```csharp
Task OnLiveSessionStats(LiveStatsDto stats);

// LiveStatsDto
{
    "sessionId": "guid",
    "totalMessages": 1250,
    "totalOrders": 45,
    "totalSales": 28500,
    "ordersPerMinute": 3.2,
    "topProducts": [
        { "code": "1", "name": "เสื้อยืด", "quantity": 25 },
        { "code": "3", "name": "กางเกง", "quantity": 15 }
    ]
}
```

### Hub Methods (Client → Server)

#### 1. JoinSession
เข้าร่วมเซสชันไลฟ์

```csharp
Task JoinSession(Guid sessionId);
```

#### 2. LeaveSession
ออกจากเซสชันไลฟ์

```csharp
Task LeaveSession(Guid sessionId);
```

#### 3. SendChatMessage (Mobile App)
ส่งข้อความแชทจาก Mobile App

```csharp
Task SendChatMessage(MobileChatMessage message);

// MobileChatMessage
{
    "userId": "fb_12345",
    "userName": "สมชาย",
    "message": "cf1",
    "platform": "Facebook",
    "timestamp": "2024-12-25T14:30:00Z"
}
```

#### 4. RequestOrderSync (Packing Station)
ขอ Sync ออเดอร์ทั้งหมด

```csharp
Task<List<OrderDto>> RequestOrderSync();
```

#### 5. UpdatePackingStatus
อัพเดทสถานะการแพค

```csharp
Task UpdatePackingStatus(PackingUpdateDto update);

// PackingUpdateDto
{
    "orderId": "guid",
    "status": "Packed",
    "packedItems": ["item1-guid", "item2-guid"]
}
```

---

## 🌐 REST API

### Base URL

```
http://[HOST]:5000/api
```

### Authentication

> สำหรับ Customer Portal ไม่ต้อง Auth
> สำหรับ Admin API ใช้ Bearer Token

```http
Authorization: Bearer {token}
```

---

### Customer Portal API

#### GET /api/orders/track/{trackingNumber}
ค้นหาออเดอร์จาก Tracking Number

**Request:**
```http
GET /api/orders/track/EX123456789TH
```

**Response:**
```json
{
    "success": true,
    "data": {
        "orderNumber": "ORD-20241225-001",
        "trackingNumber": "EX123456789TH",
        "carrier": "Kerry",
        "status": "InTransit",
        "customerName": "สมชาย",
        "items": [
            {
                "productName": "เสื้อยืด #1",
                "quantity": 2
            }
        ],
        "timeline": [
            {
                "status": "Created",
                "timestamp": "2024-12-24T10:00:00Z",
                "description": "สร้างออเดอร์"
            },
            {
                "status": "Paid",
                "timestamp": "2024-12-24T10:30:00Z",
                "description": "ชำระเงินแล้ว"
            },
            {
                "status": "Packed",
                "timestamp": "2024-12-25T09:00:00Z",
                "description": "แพคสินค้าแล้ว"
            },
            {
                "status": "Shipped",
                "timestamp": "2024-12-25T14:00:00Z",
                "description": "ส่งพัสดุแล้ว"
            }
        ],
        "lastUpdate": "2024-12-25T14:00:00Z"
    }
}
```

#### GET /api/orders/phone/{phoneNumber}
ค้นหาออเดอร์จากเบอร์โทร

**Request:**
```http
GET /api/orders/phone/0812345678
```

**Response:**
```json
{
    "success": true,
    "data": [
        {
            "orderNumber": "ORD-20241225-001",
            "status": "InTransit",
            "total": 648,
            "createdAt": "2024-12-24T10:00:00Z"
        },
        {
            "orderNumber": "ORD-20241220-005",
            "status": "Delivered",
            "total": 1299,
            "createdAt": "2024-12-20T15:00:00Z"
        }
    ]
}
```

---

### OBS Overlay API

#### GET /overlay/sales
ยอดขายปัจจุบัน

**Response:**
```json
{
    "totalSales": 28500,
    "totalOrders": 45,
    "todaySales": 15750,
    "todayOrders": 25
}
```

#### GET /overlay/latest-orders
ออเดอร์ล่าสุด

**Query Parameters:**
- `limit` (optional): จำนวนรายการ (default: 5)

**Response:**
```json
{
    "orders": [
        {
            "customerName": "สมชาย",
            "items": "เสื้อยืด #1 x2",
            "total": 648,
            "time": "14:30"
        },
        {
            "customerName": "สมหญิง",
            "items": "กางเกง #3",
            "total": 399,
            "time": "14:28"
        }
    ]
}
```

#### GET /overlay/top-products
สินค้าขายดี

**Response:**
```json
{
    "products": [
        {
            "code": "1",
            "name": "เสื้อยืด",
            "quantity": 25
        },
        {
            "code": "3",
            "name": "กางเกง",
            "quantity": 15
        },
        {
            "code": "5",
            "name": "หมวก",
            "quantity": 8
        }
    ]
}
```

#### WebSocket /overlay/realtime
Real-time updates

**Events:**
```json
// order_created
{
    "type": "order_created",
    "data": {
        "customerName": "สมชาย",
        "items": "เสื้อยืด #1 x2",
        "total": 648
    }
}

// stats_update
{
    "type": "stats_update",
    "data": {
        "totalSales": 28500,
        "totalOrders": 45
    }
}
```

---

### Internal Admin API

#### POST /api/auth/login
ล็อกอินเข้าระบบ

**Request:**
```json
{
    "username": "admin",
    "password": "password123"
}
```

**Response:**
```json
{
    "success": true,
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "expiresAt": "2024-12-26T14:30:00Z"
}
```

#### GET /api/dashboard/stats
สถิติ Dashboard

**Response:**
```json
{
    "today": {
        "sales": 15750,
        "orders": 25,
        "customers": 20
    },
    "week": {
        "sales": 85000,
        "orders": 150,
        "customers": 120
    },
    "month": {
        "sales": 350000,
        "orders": 580,
        "customers": 450
    },
    "pendingOrders": 12,
    "lowStockProducts": 5
}
```

---

## 🔗 External API Integrations

### Kerry Express API

#### สร้าง Booking

```http
POST https://api.kerryexpress.com/booking
Content-Type: application/json
Authorization: Bearer {api_key}

{
    "sender": {
        "name": "ร้านค้าตัวอย่าง",
        "phone": "021234567",
        "address": "123 ถนนตัวอย่าง",
        "province": "กรุงเทพมหานคร",
        "postalCode": "10100"
    },
    "receiver": {
        "name": "สมชาย ใจดี",
        "phone": "0812345678",
        "address": "456 ถนนทดสอบ",
        "province": "เชียงใหม่",
        "postalCode": "50000"
    },
    "parcel": {
        "weight": 1.5,
        "width": 20,
        "height": 10,
        "length": 30
    },
    "cod": {
        "amount": 648
    },
    "serviceType": "NORMAL"
}
```

#### Response

```json
{
    "success": true,
    "data": {
        "trackingNumber": "KERTH12345678",
        "labelUrl": "https://api.kerryexpress.com/labels/KERTH12345678.pdf",
        "estimatedDelivery": "2024-12-27"
    }
}
```

### Flash Express API

#### สร้าง Order

```http
POST https://api.flashexpress.com/v1/orders
Content-Type: application/json
X-API-Key: {api_key}

{
    "order_ref": "ORD-20241225-001",
    "sender_info": {
        "name": "ร้านค้าตัวอย่าง",
        "phone": "021234567"
    },
    "receiver_info": {
        "name": "สมชาย ใจดี",
        "phone": "0812345678",
        "address": "456 ถนนทดสอบ ตำบลช้างเผือก อำเภอเมือง จังหวัดเชียงใหม่ 50000"
    },
    "parcel": {
        "weight": 1500,
        "dimension": {
            "width": 20,
            "height": 10,
            "depth": 30
        }
    }
}
```

### J&T Express API

#### สร้าง Waybill

```http
POST https://api.jtexpress.co.th/api/order/create
Content-Type: application/json

{
    "eccompanyid": "SHOP001",
    "customerid": "12345",
    "sender": {
        "name": "ร้านค้าตัวอย่าง",
        "mobile": "021234567",
        "prov": "กรุงเทพมหานคร",
        "city": "บางซื่อ",
        "area": "บางซื่อ",
        "address": "123 ถนนตัวอย่าง"
    },
    "receiver": {
        "name": "สมชาย ใจดี",
        "mobile": "0812345678",
        "prov": "เชียงใหม่",
        "city": "เมือง",
        "area": "ช้างเผือก",
        "address": "456 ถนนทดสอบ"
    },
    "weight": "1.5",
    "itemname": "เสื้อผ้า"
}
```

---

## 🔐 XMan Studio License API

> Repository: github.com/xjanova/xmanstudio

### Base URL

```
https://api.xmanstudio.com/v1
```

### Validate License

#### POST /license/validate

**Request:**
```json
{
    "licenseKey": "XXXX-XXXX-XXXX-XXXX",
    "machineId": "unique-machine-identifier",
    "productCode": "LIVEXSHOP"
}
```

**Response (Success):**
```json
{
    "success": true,
    "data": {
        "isValid": true,
        "licenseType": "PRO",
        "expiresAt": "2025-12-25T23:59:59Z",
        "features": {
            "chatCapture": true,
            "mobileApp": true,
            "packingStation": true,
            "obsOverlay": true,
            "multiDevice": 3,
            "apiAccess": false
        },
        "customer": {
            "name": "บริษัท ตัวอย่าง จำกัด",
            "email": "contact@example.com"
        }
    }
}
```

**Response (Invalid):**
```json
{
    "success": false,
    "error": {
        "code": "LICENSE_EXPIRED",
        "message": "License has expired"
    }
}
```

### Activate License

#### POST /license/activate

**Request:**
```json
{
    "licenseKey": "XXXX-XXXX-XXXX-XXXX",
    "machineId": "unique-machine-identifier",
    "machineName": "DESKTOP-ABC123",
    "productCode": "LIVEXSHOP"
}
```

**Response:**
```json
{
    "success": true,
    "data": {
        "activated": true,
        "activationsUsed": 2,
        "activationsTotal": 3
    }
}
```

### Deactivate License

#### POST /license/deactivate

**Request:**
```json
{
    "licenseKey": "XXXX-XXXX-XXXX-XXXX",
    "machineId": "unique-machine-identifier"
}
```

### Get Feature Flags

#### GET /config/features?product=LIVEXSHOP

**Response:**
```json
{
    "features": {
        "slipVerificationAI": true,
        "tiktokCapture": true,
        "newDashboard": false
    },
    "minVersion": "1.2.0",
    "latestVersion": "1.5.0",
    "updateUrl": "https://download.xmanstudio.com/livexshop/latest"
}
```

### Report Usage Analytics

#### POST /analytics/usage

**Request:**
```json
{
    "licenseKey": "XXXX-XXXX-XXXX-XXXX",
    "machineId": "unique-machine-identifier",
    "events": [
        {
            "event": "live_session_started",
            "timestamp": "2024-12-25T14:00:00Z",
            "platform": "Facebook"
        },
        {
            "event": "order_created",
            "timestamp": "2024-12-25T14:05:00Z",
            "count": 1
        }
    ]
}
```

---

## 📝 Error Codes

### HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | Success |
| 400 | Bad Request |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 429 | Rate Limited |
| 500 | Server Error |

### Custom Error Codes

| Code | Description |
|------|-------------|
| LICENSE_INVALID | License key ไม่ถูกต้อง |
| LICENSE_EXPIRED | License หมดอายุ |
| LICENSE_BLOCKED | License ถูกบล็อก |
| ACTIVATION_LIMIT | เกินจำนวน Activation |
| ORDER_NOT_FOUND | ไม่พบออเดอร์ |
| TRACKING_NOT_FOUND | ไม่พบ Tracking |
| PAYMENT_DUPLICATE | สลิปซ้ำ |
| SLIP_INVALID | สลิปไม่ถูกต้อง |

---

<p align="center">
  <strong>Live x Shop Pro API</strong><br>
  Version 1.0 | Xman Studio
</p>
