# Changelog

## Unreleased

- เพิ่ม GitHub Actions สำหรับ Build EXE/Portable ZIP ทุก Push และ Pull Request
- สร้าง Tag และ GitHub Release อัตโนมัติเมื่อไฟล์ `VERSION` เปลี่ยนเป็นเลขรุ่นใหม่
- ใช้ `VERSION` เป็นแหล่งเลขรุ่นเดียวสำหรับ UI, EXE, ZIP และ Release

## 1.0.0 — 2026-11-07

- เพิ่ม Global Hotkey `Ctrl + Shift + Q` และ fallback `Ctrl + Alt + Q`
- เลือกพื้นที่ QR จากจอที่เมาส์อยู่ รองรับหลายจอและ Mixed DPI
- แสดงผล, คัดลอก และเปิดลิงก์ `http/https` ใน Google Chrome
- เพิ่ม System tray และตัวเลือกเปิดพร้อม Windows
- ประมวลผลภาพในเครื่อง ไม่อัปโหลดหรือบันทึกภาพหน้าจอ
- เพิ่ม Portable EXE/ZIP และสคริปต์ติดตั้ง/ถอนการติดตั้ง

### Known limitations

- ยังไม่มี Code Signing; Windows SmartScreen อาจเตือนบนเครื่องอื่น
- ต้องเลื่อนเมาส์ไปจอที่มี QR ก่อนกด Hotkey
