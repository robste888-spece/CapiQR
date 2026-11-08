# CapiQR — QR Link Scanner for Windows 11

โปรแกรมขนาดเล็กสำหรับอ่าน QR Code ที่แสดงอยู่บนหน้าจอ Windows โดยไม่ต้องใช้โทรศัพท์ เหมาะกับ QR ใน Outlook, Mail, PDF, Browser และรูปภาพ

## ดาวน์โหลดสำหรับทีมงาน

ดาวน์โหลดรุ่นพร้อมใช้จากหน้า [Releases](https://github.com/robste888-spece/CapiQR/releases/latest) แล้วเลือกไฟล์ `CapiQR-Portable-v<version>.zip` จากนั้นแตก ZIP และเปิด `CapiQR.exe`

โปรแกรมเป็น Portable และรวมไลบรารีอ่าน QR ไว้ใน EXE แล้ว ไม่ต้องติดตั้ง .NET Runtime หรือ DLL เพิ่มบน Windows 11

## ใช้งาน

1. เปิด `dist\CapiQR.exe`
2. เลื่อนเมาส์ไปยังจอที่มี QR แล้วกด `Ctrl + Shift + Q` (ถ้าชนกับโปรแกรมอื่น ระบบจะใช้ `Ctrl + Alt + Q`)
3. ลากกรอบครอบ QR Code ให้มีขอบขาวรอบ QR เล็กน้อย
4. ตรวจสอบผล แล้วกด **เปิดใน Chrome** หรือ **คัดลอก**

ปิดหน้าต่างหลักแล้วโปรแกรมจะทำงานต่อใน System tray; เลือก **Exit** จากเมนูคลิกขวาที่ไอคอนเมื่อต้องการหยุด

## ติดตั้ง

- แบบ Portable: เปิด `dist\CapiQR.exe` ได้ทันที ไม่ต้องติดตั้ง Runtime เพิ่ม
- แบบ Start Menu: คลิกขวา `Install-CapiQR.ps1` → **Run with PowerShell**
- สร้าง Desktop shortcut ด้วย: `pwsh -File .\Install-CapiQR.ps1 -DesktopShortcut`

เปิดตัวเลือก **เปิด CapiQR พร้อม Windows** ในหน้าหลักหากต้องการให้ Hotkey พร้อมใช้ทุกครั้งที่ Sign in

## Privacy & Security

- จับและอ่านภาพในเครื่องเท่านั้น ไม่มีการอัปโหลดภาพหรือลิงก์
- ปุ่มเปิด Chrome อนุญาตเฉพาะ `http://` และ `https://`
- QR ที่เป็นข้อความ, Wi-Fi, file path หรือ custom scheme แสดง/คัดลอกได้ แต่ไม่เปิดอัตโนมัติ
- โปรแกรมยังไม่มี Code Signing; Windows SmartScreen อาจเตือนเมื่อรันบนเครื่องอื่น

## Build จาก Source

รัน `pwsh -ExecutionPolicy Bypass -File .\build.ps1`

`build.ps1` จะอ่านเลขรุ่นจากไฟล์ `VERSION` แล้วฝังเป็น Assembly/File Version ใน EXE พร้อมสร้าง ZIP ชื่อเดียวกับรุ่น สามารถทดสอบเลขรุ่นอื่นโดยใช้ `pwsh -File .\build.ps1 -Version 1.1.0`

ต้องมี Windows 11/.NET Framework 4.8 (มีใน Windows 11) โค้ดใช้ ZXing.Net 0.16.10 ภายใต้ Apache-2.0; ดู `vendor\ZXing.Net\LICENSE.txt`

## Automatic Build และ Release

GitHub Actions ที่ `.github/workflows/build-release.yml` ทำงานดังนี้:

- ทุก Push หรือ Pull Request เข้า `main`: Build และตรวจ File Version แล้วเก็บ EXE/ZIP เป็น Artifact 30 วัน
- Push เข้า `main` โดยยังใช้เลขเดิม: Build เพื่อ QA แต่ไม่เขียนทับ Release เดิม
- Push เข้า `main` หลังเปลี่ยน `VERSION` เป็นเลขใหม่: สร้าง Tag และ GitHub Release พร้อมแนบ EXE/ZIP อัตโนมัติ

ขั้นตอนออกรุ่นใหม่:

1. แก้ Source และทดสอบในเครื่อง
2. เปลี่ยน `VERSION` ตาม Semantic Versioning เช่น เพิ่ม Function จาก `1.0.0` เป็น `1.1.0`
3. เพิ่มรายละเอียดใน `CHANGELOG.md`
4. Commit และ Push เข้า `main`
5. ตรวจผลที่ [GitHub Actions](https://github.com/robste888-spece/CapiQR/actions) และดาวน์โหลดจากหน้า Releases

Workflow ยังไม่ได้ทำ Code Signing ดังนั้น SmartScreen อาจยังเตือน แม้ไฟล์จะ Build จาก GitHub สำเร็จ

## ข้อจำกัด

- หาก QR เล็กหรือเบลอ ให้ Zoom Mail/PDF ก่อนสแกน
- QR ต้องมองเห็นบนจอและไม่ถูกหน้าต่างอื่นบัง; กรณีหลายจอ แอปจะจับจอที่เมาส์อยู่ตอนกด Hotkey
- ตัวโปรแกรมเป็น Windows utility ระดับระบบ ไม่ผูกกับ Outlook จึงใช้กับทุกแอปได้

## Source และการสนับสนุน

- Repository: [robste888-spece/CapiQR](https://github.com/robste888-spece/CapiQR)
- แจ้งปัญหาหรือเสนอฟังก์ชัน: [GitHub Issues](https://github.com/robste888-spece/CapiQR/issues)
