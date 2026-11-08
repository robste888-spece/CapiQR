# HANDOFF — CapiQR QR Link Scanner for Windows 11
อัปเดตล่าสุด: 8 พฤศจิกายน 2569 (2026-11-08)

## 🟢 สถานะ
รุ่น Production ปัจจุบันคือ `1.1.0`; โปรแกรมตรวจ Release ล่าสุดอัตโนมัติเมื่อเปิด แจ้งผู้ใช้เมื่อมีรุ่นใหม่ และเปิดหน้าดาวน์โหลดบน GitHub ได้ทันที โดย GitHub Actions Run #4 Build/Test/Release สำเร็จครบ

## ✅ ทำรอบนี้
- เพิ่ม Update Checker ที่เรียก Public GitHub Releases API โดยไม่ใช้ Token และเปรียบเทียบ Semantic Version กับ `VERSION`
- เพิ่มปุ่ม `ตรวจสอบอัปเดต` ในหน้าหลักและเมนู Tray พร้อมตรวจอัตโนมัติหลังเปิดโปรแกรม
- เพิ่มหน้าต่างแจ้งรุ่นใหม่โทน Capybara; เปิด GitHub Release ได้ แต่ไม่ดาวน์โหลด/รัน EXE แบบเงียบ
- เพิ่ม Unit Test สำหรับเวอร์ชันใหม่/เท่ากัน/เก่ากว่า/Tag ผิดรูปแบบ และการป้องกัน URL ที่ไม่ใช่ GitHub HTTPS
- ทดสอบ Local สำเร็จ: Version Test, Build `1.1.0`, File/Product Version, Portable ZIP, Live GitHub API และ Visual QA 2 หน้าจอ
- ตรวจ Secret Pattern ซ้ำแล้วไม่พบ Token/Password/ข้อมูลรับรองใน Source ที่เพิ่ม
- Push Commit `1a47300` และ Merge Commit `8278762`; GitHub Actions Run #4 สำเร็จใน 34 วินาที
- ตรวจ GitHub Release `v1.1.0` แล้ว: มี `CapiQR.exe` และ `CapiQR-Portable-v1.1.0.zip` พร้อม SHA-256

## 📁 ของอยู่ที่ไหน
- GitHub Public Repo: `https://github.com/robste888-spece/CapiQR`
- Published Release: `https://github.com/robste888-spece/CapiQR/releases/tag/v1.1.0`
- Portable ZIP download: `https://github.com/robste888-spece/CapiQR/releases/download/v1.1.0/CapiQR-Portable-v1.1.0.zip`
- EXE download: `https://github.com/robste888-spece/CapiQR/releases/download/v1.1.0/CapiQR.exe`
- Auto Build Workflow: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\.github\workflows\build-release.yml`
- GitHub Actions Run #4: `https://github.com/robste888-spece/CapiQR/actions/runs/42470566385`
- Version source: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\VERSION`
- โปรแกรม: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\dist\CapiQR.exe`
- Update source: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\src\UpdateChecker.cs`
- Update notification UI: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\src\UpdateAvailableForm.cs`
- Source/Build: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\src` และ `build.ps1`
- คู่มือ: `D:\OneDrive\Temp Ton\AI\Project\20261107 Add in OCR on Windows\README.md`

## 🧭 การตัดสินใจ + เหตุผล (สะสม, ใหม่บนสุด)
- 8 พ.ย. 2569 แจ้งเตือนและเปิด GitHub Release แทน Silent Update | เหตุผล: โปรแกรมยังไม่มี Code Signing; การดาวน์โหลดและรัน EXE เงียบเพิ่มความเสี่ยง Security/SmartScreen
- 8 พ.ย. 2569 ใช้ Public GitHub Releases API โดยไม่ใช้ Token | เหตุผล: Repo เป็น Public, ลดการเก็บ Secret และเพียงพอสำหรับตรวจรุ่นล่าสุด
- 8 พ.ย. 2569 ให้ Auto Check ล้มเหลวแบบ Non-blocking | เหตุผล: อินเทอร์เน็ต/GitHub ขัดข้องต้องไม่กระทบการสแกน QR
- 8 พ.ย. 2569 ใช้ `VERSION` เป็น Single Source of Truth | เหตุผล: ป้องกัน UI, EXE, ZIP, Tag และ Release ใช้เลขไม่ตรงกัน
- 8 พ.ย. 2569 Build ทุก Push แต่สร้าง Release เฉพาะเมื่อ `VERSION` เป็นเลขใหม่ | เหตุผล: ได้ QA ต่อเนื่องโดยไม่สร้าง/เขียนทับ Release ซ้ำ
- 8 พ.ย. 2569 ให้ Workflow เรียก `build.ps1` เดิมบน Windows Runner | เหตุผล: ลดความต่างระหว่าง Build ในเครื่องกับ GitHub
- 7 พ.ย. 2569 เปลี่ยน Repository เป็น Public หลังตรวจ Secret/Privacy และได้รับการยืนยันขั้นสุดท้าย | เหตุผล: ทีมดาวน์โหลดได้โดยไม่ต้องมีบัญชีหรือสิทธิ์ Collaborator; ไม่พบ Secret/ข้อมูลลูกค้าใน Code และ history
- 7 พ.ย. 2569 ใช้ Semantic Versioning และเริ่มที่ `1.0.0` | เหตุผล: ฟังก์ชันหลักพร้อมใช้งานครบและเหมาะเป็นรุ่น Production แรก
- 7 พ.ย. 2569 ตั้ง Repository เป็น Private | เหตุผล: ใช้แบ่งปันภายในทีมและลดความเสี่ยงเผยแพร่ Source/Executable สู่สาธารณะ
- 7 พ.ย. 2569 แจกทั้ง Portable ZIP และ EXE ใน Release | เหตุผล: ZIP เหมาะกับผู้ใช้ทั่วไป ส่วน EXE รองรับการดาวน์โหลดตรง
- 7 พ.ย. 2569 จับจอที่เมาส์อยู่แทน Virtual Screen รวม | เหตุผล: Virtual Screen ที่มีพิกัดติดลบและ Mixed DPI ทำให้กรอบเลือกคลาด; แบบจอเดียวผ่าน End-to-End QA
- 7 พ.ย. 2569 ใช้ Windows utility แทน Outlook add-in | เหตุผล: ใช้กับ Mail, PDF, Browser และรูปทุกโปรแกรมโดยไม่ผูกเวอร์ชัน Outlook
- 7 พ.ย. 2569 ใช้ .NET Framework 4.8 + Embed ZXing.Net | เหตุผล: เครื่องเป้าหมาย Windows 11 ใช้งานได้โดยไม่ติดตั้ง Runtime เพิ่มและได้ EXE เดียว
- 7 พ.ย. 2569 เปิด Chrome เฉพาะ `http/https` | เหตุผล: ลดความเสี่ยงจาก custom URI/file scheme ใน QR ที่ไม่ไว้ใจ

## ❌ ทางเลือกที่ปัดตก (สะสม, ใหม่บนสุด)
- 8 พ.ย. 2569 ไม่ดาวน์โหลดและรัน EXE อัตโนมัติแบบเงียบ | เหตุผล: ยังไม่มี Installer, Rollback และ Code Signing ที่เหมาะกับการอัปเดตอัตโนมัติเต็มรูปแบบ
- 8 พ.ย. 2569 ไม่เพิ่มเลขเวอร์ชันอัตโนมัติทุก Commit | เหตุผล: Commit เอกสาร/แก้เล็กน้อยไม่ควรถูกประกาศเป็น Product Release ใหม่
- 8 พ.ย. 2569 ไม่สร้าง Release ทดสอบด้วยเลขรุ่นปลอม | เหตุผล: ป้องกันผู้ใช้ดาวน์โหลดรุ่นที่ไม่มี Functional Change
- 7 พ.ย. 2569 ไม่คง Repository แบบ Private | เหตุผล: เพิ่มภาระเชิญ Collaborator และไม่ตอบโจทย์การแชร์ลิงก์ดาวน์โหลดให้ทีมโดยตรง
- 7 พ.ย. 2569 ไม่ใช้ Overlay ครอบทุกจอเป็นหน้าต่างเดียว | เหตุผล: ทดสอบพบพิกัดคลาดจริงบนหลายจอ Mixed DPI/Negative Coordinate
- 7 พ.ย. 2569 ไม่ทำ Outlook-specific add-in | เหตุผล: ขอบเขตแคบกว่าและ Deploy/Compatibility ซับซ้อนโดยไม่เพิ่มคุณค่าหลัก
- 7 พ.ย. 2569 ไม่ใช้บริการ QR ออนไลน์ | เหตุผล: ภาพ Mail อาจเป็นข้อมูลภายใน; ประมวลผล Local ปลอดภัยกว่า

## ⚠️ ข้อห้าม/ข้อควรระวัง (สะสม, ใหม่บนสุด)
- การตรวจอัปเดตส่งเฉพาะ HTTPS Request ปกติไป GitHub เพื่ออ่าน Tag/Release URL; ไม่ส่งภาพหน้าจอ เนื้อหา QR หรือข้อมูล Mail
- Public GitHub API ที่ไม่ใช้ Token มี Rate Limit; การตรวจครั้งเดียวเมื่อเปิดและเมื่อผู้ใช้กดเองเหมาะกับทีมขนาดเล็ก แต่ไม่ควรเพิ่ม Polling ถี่
- ก่อน Push Function ใหม่ ต้องเปลี่ยน `VERSION` และ `CHANGELOG.md`; หากเลขเดิม Workflow จะ Build Artifact แต่ไม่ออก Release ใหม่
- Auto Build ไม่ได้แก้ SmartScreen เพราะ Workflow ยังไม่มี Certificate/Code Signing
- Repository เป็น Public: Source, Release, Git history และ `handoff.md` มองเห็น/ถูก Fork ได้ทั่วโลก; Robs ยืนยันยอมรับชื่อ GitHub และ path งานที่ปรากฏใน history แล้ว
- โปรแกรมไม่ส่งภาพ/QR ออกอินเทอร์เน็ต แต่เมื่อเกิดข้อผิดพลาดจะเขียน Exception ลง `%LOCALAPPDATA%\CapiQR\error.log` ในเครื่องผู้ใช้
- EXE ยังไม่มี Code Signing; Windows SmartScreen อาจเตือนบนเครื่องอื่น
- หลายจอ: ต้องเลื่อนเมาส์ไปจอที่มี QR ก่อนกด Hotkey
- ตัวเลือกเปิดพร้อม Windows เขียนค่าเฉพาะผู้ใช้ที่ `HKCU\Software\Microsoft\Windows\CurrentVersion\Run`

## ⏭️ งานถัดไป
- ให้ทีมดาวน์โหลด/แทนที่ด้วย `v1.1.0` ด้วยตนเองครั้งแรก เพราะ `v1.0.0` ยังไม่มี Update Checker; ตั้งแต่ `v1.1.0` เป็นต้นไปจึงจะแจ้ง Release ใหม่อัตโนมัติ
- เมื่อเพิ่ม Function: แก้ Source → ทดสอบ → เพิ่ม `VERSION` (เช่น `1.2.0`) → อัปเดต `CHANGELOG.md` → Push `main`
- ทำ User Acceptance บนเครื่องทีมอย่างน้อย 1 เครื่อง: ดาวน์โหลด/เปิด `v1.1.0` → ตรวจ SmartScreen → กดตรวจอัปเดต → สแกน QR จริง
- หากต้องลด SmartScreen ให้เพิ่ม Code Signing Certificate เป็นขั้นตอนแยก โดยห้าม Commit Certificate/Password ลง Repo

## ❓ ยังไม่ยืนยัน
- ยังไม่มี User Acceptance บนเครื่องทีมว่า `v1.1.0` เปิดใช้งานและตรวจ Release ได้ตาม Policy องค์กร แม้ Logic, Live API และ Release จะผ่านแล้ว
- ยังไม่ได้ทดสอบ SmartScreen/Policy บนเครื่ององค์กรเครื่องอื่น
- ยังไม่ได้เปิด Startup option จริงระหว่าง QA เพื่อหลีกเลี่ยงการเปลี่ยนค่าถาวรในเครื่องผู้ใช้
