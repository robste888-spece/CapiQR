# HANDOFF — CapiQR QR Link Scanner for Windows 11
อัปเดตล่าสุด: 7 พฤศจิกายน 2569 (2026-11-07)

## 🟢 สถานะ
เผยแพร่ GitHub Release `v1.0.0` สำเร็จและเปลี่ยน Repository เป็น Public แล้ว; ทุกคนเปิด Source, Git history และดาวน์โหลด ZIP/EXE ได้โดยไม่ต้องรับเชิญ

## ✅ ทำรอบนี้
- สร้าง Private Repo `robste888-spece/CapiQR` และ Push branch `main` พร้อม tag `v1.0.0`
- กำหนด Product/Release version `1.0.0` และ Windows EXE file version `1.0.0.0`; แสดง `CapiQR v1.0.0` ใน UI
- เตรียม Release title/notes และแนบ `CapiQR-Portable-v1.0.0.zip` กับ `CapiQR.exe` สำเร็จ
- ตรวจยืนยัน Release Draft: tag, title, notes และไฟล์แนบครบก่อนเผยแพร่
- คืน Git metadata จากชื่อพักชั่วคราวกลับเป็น `.git` เรียบร้อย
- เผยแพร่ Release `v1.0.0` ตามการยืนยันของ Robs และตรวจหน้า Release/Assets หลังเผยแพร่แล้ว
- ตรวจ Source และ Git history ทุก Commit: ไม่พบ Token, API Key, Password, Credential, Email ส่วนตัว หรือโค้ดส่งภาพ/QR ออกอินเทอร์เน็ต
- เปลี่ยน Repository จาก Private เป็น Public ตามการยืนยันของ Robs และตรวจสถานะ GitHub ว่า `currently public`

## 📁 ของอยู่ที่ไหน
- GitHub Public Repo: `https://github.com/robste888-spece/CapiQR`
- Published Release: `https://github.com/robste888-spece/CapiQR/releases/tag/v1.0.0`
- Portable ZIP download: `https://github.com/robste888-spece/CapiQR/releases/download/v1.0.0/CapiQR-Portable-v1.0.0.zip`
- EXE download: `https://github.com/robste888-spece/CapiQR/releases/download/v1.0.0/CapiQR.exe`
- โปรแกรม: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\dist\CapiQR.exe`
- Portable package: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\dist\CapiQR-Portable-v1.0.0.zip`
- Source/Build: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\src` และ `build.ps1`
- คู่มือ: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\README.md`

## 🧭 การตัดสินใจ + เหตุผล (สะสม, ใหม่บนสุด)
- 7 พ.ย. 2569 เปลี่ยน Repository เป็น Public หลังตรวจ Secret/Privacy และได้รับการยืนยันขั้นสุดท้าย | เหตุผล: ทีมดาวน์โหลดได้โดยไม่ต้องมีบัญชีหรือสิทธิ์ Collaborator; ไม่พบ Secret/ข้อมูลลูกค้าใน Code และ history
- 7 พ.ย. 2569 ใช้ Semantic Versioning และเริ่มที่ `1.0.0` | เหตุผล: ฟังก์ชันหลักพร้อมใช้งานครบและเหมาะเป็นรุ่น Production แรก
- 7 พ.ย. 2569 ตั้ง Repository เป็น Private | เหตุผล: ใช้แบ่งปันภายในทีมและลดความเสี่ยงเผยแพร่ Source/Executable สู่สาธารณะ
- 7 พ.ย. 2569 แจกทั้ง Portable ZIP และ EXE ใน Release | เหตุผล: ZIP เหมาะกับผู้ใช้ทั่วไป ส่วน EXE รองรับการดาวน์โหลดตรง
- 7 พ.ย. 2569 จับจอที่เมาส์อยู่แทน Virtual Screen รวม | เหตุผล: Virtual Screen ที่มีพิกัดติดลบและ Mixed DPI ทำให้กรอบเลือกคลาด; แบบจอเดียวผ่าน End-to-End QA
- 7 พ.ย. 2569 ใช้ Windows utility แทน Outlook add-in | เหตุผล: ใช้กับ Mail, PDF, Browser และรูปทุกโปรแกรมโดยไม่ผูกเวอร์ชัน Outlook
- 7 พ.ย. 2569 ใช้ .NET Framework 4.8 + Embed ZXing.Net | เหตุผล: เครื่องเป้าหมาย Windows 11 ใช้งานได้โดยไม่ติดตั้ง Runtime เพิ่มและได้ EXE เดียว
- 7 พ.ย. 2569 เปิด Chrome เฉพาะ `http/https` | เหตุผล: ลดความเสี่ยงจาก custom URI/file scheme ใน QR ที่ไม่ไว้ใจ

## ❌ ทางเลือกที่ปัดตก (สะสม, ใหม่บนสุด)
- 7 พ.ย. 2569 ไม่คง Repository แบบ Private | เหตุผล: เพิ่มภาระเชิญ Collaborator และไม่ตอบโจทย์การแชร์ลิงก์ดาวน์โหลดให้ทีมโดยตรง
- 7 พ.ย. 2569 ไม่ใช้ Overlay ครอบทุกจอเป็นหน้าต่างเดียว | เหตุผล: ทดสอบพบพิกัดคลาดจริงบนหลายจอ Mixed DPI/Negative Coordinate
- 7 พ.ย. 2569 ไม่ทำ Outlook-specific add-in | เหตุผล: ขอบเขตแคบกว่าและ Deploy/Compatibility ซับซ้อนโดยไม่เพิ่มคุณค่าหลัก
- 7 พ.ย. 2569 ไม่ใช้บริการ QR ออนไลน์ | เหตุผล: ภาพ Mail อาจเป็นข้อมูลภายใน; ประมวลผล Local ปลอดภัยกว่า

## ⚠️ ข้อห้าม/ข้อควรระวัง (สะสม, ใหม่บนสุด)
- Repository เป็น Public: Source, Release, Git history และ `handoff.md` มองเห็น/ถูก Fork ได้ทั่วโลก; Robs ยืนยันยอมรับชื่อ GitHub และ path งานที่ปรากฏใน history แล้ว
- โปรแกรมไม่ส่งภาพ/QR ออกอินเทอร์เน็ต แต่เมื่อเกิดข้อผิดพลาดจะเขียน Exception ลง `%LOCALAPPDATA%\CapiQR\error.log` ในเครื่องผู้ใช้
- EXE ยังไม่มี Code Signing; Windows SmartScreen อาจเตือนบนเครื่องอื่น
- หลายจอ: ต้องเลื่อนเมาส์ไปจอที่มี QR ก่อนกด Hotkey
- ตัวเลือกเปิดพร้อม Windows เขียนค่าเฉพาะผู้ใช้ที่ `HKCU\Software\Microsoft\Windows\CurrentVersion\Run`

## ⏭️ งานถัดไป
- ให้ Robs ทำ User Acceptance กับ Mail จริง 2-3 แบบ; หากกระจายหลายเครื่องให้พิจารณา Code Signing/Installer ที่ฝ่าย IT อนุมัติ
- ส่งลิงก์ Public Release ให้ทีม: `https://github.com/robste888-spece/CapiQR/releases/tag/v1.0.0`

## ❓ ยังไม่ยืนยัน
- ยังไม่มี User Acceptance จาก Robs หลังส่งมอบ
- ยังไม่ได้ทดสอบ SmartScreen/Policy บนเครื่ององค์กรเครื่องอื่น
- ยังไม่ได้เปิด Startup option จริงระหว่าง QA เพื่อหลีกเลี่ยงการเปลี่ยนค่าถาวรในเครื่องผู้ใช้
