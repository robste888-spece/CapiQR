# HANDOFF — CapiQR QR Link Scanner for Windows 11
อัปเดตล่าสุด: 7 พฤศจิกายน 2569 (2026-11-07)

## 🟢 สถานะ
เสร็จและ Build รุ่น 1.0.0 แล้ว: Portable EXE/ZIP ใช้งานได้, QR ตัวอย่างผ่าน Decode และเส้นทาง Hotkey → เลือกพื้นที่ → แสดงลิงก์ผ่าน End-to-End QA บน Windows จริง

## ✅ ทำรอบนี้
- สร้าง WinForms utility โทน Capybara พร้อม Global Hotkey, System tray, Startup option และปุ่มเปิด Chrome
- จำกัดการเปิดอัตโนมัติเฉพาะ `http/https`; QR ประเภทอื่นแสดงและคัดลอกได้
- Embed ZXing.Net 0.16.10 ใน EXE; ไม่ต้องติดตั้ง .NET Runtime/ไฟล์ DLL เพิ่มบน Windows 11
- แก้ Mixed-DPI หลายจอโดยจับเฉพาะจอที่เมาส์อยู่ตอนกด Hotkey
- ทดสอบ QR ในภาพผู้ใช้: Decode ได้ลิงก์ Google Forms ตรงกับต้นฉบับ
- เพิ่ม Build, Install/Uninstall scripts, README, License, App icon และ Portable ZIP

## 📁 ของอยู่ที่ไหน
- โปรแกรม: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\dist\CapiQR.exe`
- Portable package: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\dist\CapiQR-Portable-v1.0.0.zip`
- Source/Build: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\src` และ `build.ps1`
- คู่มือ: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\README.md`

## 🧭 การตัดสินใจ + เหตุผล (สะสม, ใหม่บนสุด)
- 7 พ.ย. 2569 จับจอที่เมาส์อยู่แทน Virtual Screen รวม | เหตุผล: Virtual Screen ที่มีพิกัดติดลบและ Mixed DPI ทำให้กรอบเลือกคลาด; แบบจอเดียวผ่าน End-to-End QA
- 7 พ.ย. 2569 ใช้ Windows utility แทน Outlook add-in | เหตุผล: ใช้กับ Mail, PDF, Browser และรูปทุกโปรแกรมโดยไม่ผูกเวอร์ชัน Outlook
- 7 พ.ย. 2569 ใช้ .NET Framework 4.8 + Embed ZXing.Net | เหตุผล: เครื่องเป้าหมาย Windows 11 ใช้งานได้โดยไม่ติดตั้ง Runtime เพิ่มและได้ EXE เดียว
- 7 พ.ย. 2569 เปิด Chrome เฉพาะ `http/https` | เหตุผล: ลดความเสี่ยงจาก custom URI/file scheme ใน QR ที่ไม่ไว้ใจ

## ❌ ทางเลือกที่ปัดตก (สะสม, ใหม่บนสุด)
- 7 พ.ย. 2569 ไม่ใช้ Overlay ครอบทุกจอเป็นหน้าต่างเดียว | เหตุผล: ทดสอบพบพิกัดคลาดจริงบนหลายจอ Mixed DPI/Negative Coordinate
- 7 พ.ย. 2569 ไม่ทำ Outlook-specific add-in | เหตุผล: ขอบเขตแคบกว่าและ Deploy/Compatibility ซับซ้อนโดยไม่เพิ่มคุณค่าหลัก
- 7 พ.ย. 2569 ไม่ใช้บริการ QR ออนไลน์ | เหตุผล: ภาพ Mail อาจเป็นข้อมูลภายใน; ประมวลผล Local ปลอดภัยกว่า

## ⚠️ ข้อห้าม/ข้อควรระวัง (สะสม, ใหม่บนสุด)
- EXE ยังไม่มี Code Signing; Windows SmartScreen อาจเตือนบนเครื่องอื่น
- หลายจอ: ต้องเลื่อนเมาส์ไปจอที่มี QR ก่อนกด Hotkey
- ตัวเลือกเปิดพร้อม Windows เขียนค่าเฉพาะผู้ใช้ที่ `HKCU\Software\Microsoft\Windows\CurrentVersion\Run`

## ⏭️ งานถัดไป
- ให้ Robs เปิด `dist\CapiQR.exe` และทำ User Acceptance กับ Mail จริง 2-3 แบบ
- หากต้องกระจายหลายเครื่อง ให้ Code Sign และทำ Installer ที่ฝ่าย IT อนุมัติ

## ❓ ยังไม่ยืนยัน
- ยังไม่มี User Acceptance จาก Robs หลังส่งมอบ
- ยังไม่ได้ทดสอบ SmartScreen/Policy บนเครื่ององค์กรเครื่องอื่น
- ยังไม่ได้เปิด Startup option จริงระหว่าง QA เพื่อหลีกเลี่ยงการเปลี่ยนค่าถาวรในเครื่องผู้ใช้
