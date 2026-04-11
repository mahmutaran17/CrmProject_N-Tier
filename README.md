# 💎 Executive Ledger & CRM V1

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.Net Core](https://img.shields.io/badge/.NET%208-%23512BD4.svg?style=for-the-badge&logo=dotnet&logoColor=white)
![MicrosoftSQLServer](https://img.shields.io/badge/SQL%20Server-%23CC2927.svg?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![TailwindCSS](https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)

>## 📌 Proje Hakkında
**Executive Ledger & CRM**, işletmelerin dijital dönüşüm süreçlerini hızlandırmak amacıyla tasarlanmıştır. Güçlü bir back-end altyapısına sahip olan bu sistem; rol tabanlı erişim kısıtlamaları, gelişmiş veri bütünlüğü kontrolleri ve kriptografik güvenlik önlemleri ile "Kurumsal Yazılım" standartlarında geliştirilmiştir. Tasarım süreci tamamen temiz kod (Clean Code) prensiplerine ve Çok Katmanlı Mimari'ye (N-Tier Architecture) sadık kalınarak tamamlanmıştır.
---
## 🌟 Sistemin Öne Çıkan Özellikleri (Core Features)
Sistemin sunduğu temel yetenekler ve iş süreçlerine kattığı değerler:
* 🛡️ **Soft-Delete (Güvenli Veri Silme):** Veritabanındaki hiçbir müşteri veya kritik sistem kaydı fiziksel olarak silinmez (Hard-Delete yapılmaz). Sadece durumu "*Pasif/Silinmiş*" olarak işaretlenir. Bu sayede olası veri kayıplarının ve kaza eseri silinmelerin önüne geçilirken geçmişe dönük kurumsal raporlamaların bozulması engellenir.
* 🔐 **Kriptografik Güvenlik (BCrypt):** Kullanıcı şifreleri veritabanına asla düz metin (plain-text) olarak kaydedilmez. Dünyaca kabul görmüş tek yönlü (one-way) **BCrypt** algoritması ile hashlenerek saklanır. Veritabanı sızıntılarına karşı en üst düzey parola güvenliği sağlanmıştır.
* 🎫 **Kimlik Doğrulama & Yetkilendirme (Auth):** Modern güvenlik standartlarına uygun olarak token mantığıyla çalışan **Claim-Tabanlı Cookie Authentication** uygulanmıştır. Durumsuz (Stateless) mimariyle güvenli Login/Logout süreçleri inşa edilmiştir.
* 👥 **Rol Tabanlı Erişim Kontrolü (RBAC - Role Based Access Control):** Sistemde *Admin* ve *Personel* olmak üzere yönetimsel hiyerarşi kurgulanmıştır. Sadece menüler değil, Controller ve Action (işlem) bazında da (Route Authorization) sıkı yetki izolasyonları uygulanmıştır.
---
## 🏗️ Mimari Yaklaşım ve Geliştirme Yaşam Döngüsü
Bu proje sıradan bir web uygulamasından ziyade sürdürülebilir bir mühendislik ürünü olarak aşağıdaki yaşam döngüsüne (Development Lifecycle) göre inşa edilmiştir:
1. 🗂️ **Veritabanı ve Varlık Modellenmesi:** Geliştirme sürecinin ilk adımı olarak **Entity** (Varlık) sınıfları belirlenmiş, tablolar arası kurumsal ilişkiler (Relational Database Design - Bire-Çok, Çoka-Çok) kurgulanmıştır.
2. 🧩 **Katmanların Soyutlanması (Separation of Concerns):** `DataAccess` (Veri Erişimi) ve `Business` (İş Kaynakları) katmanlarında somut bağımlılıkların önüne geçmek için **Soyut (Abstract/Interface)** ve **Somut (Concrete/Manager)** sınıflar birbirinden ayrıştırılmıştır. Veritabanı işlemleri **Repository Pattern** ile tek merkezde toplanmıştır.
3. 🔌 **Veri Tabanı Bağlantısı:** Tasarlanan varlıklar **Entity Framework Core** aracılığıyla DbContext üzerinden SQL Server ile Code-First yaklaşımıyla senkronize edilmiştir. 
4. 🧠 **İş Mantığı (Business Rules) Kodlaması:** Verilerin arayüzden alınıp veritabanına işlenmesi arasında gereken tüm sistemsel doğrulamalar (Örn: "Bir şifre boş olamaz", "Aynı mail adresiyle iki kişi kayıt olamaz") tamamen `Business` katmanına hapsedilerek arayüzün (UI) sorumluluğu hafifletilmiştir.
5. 🎨 **Kullanıcı Arayüzü (UI) Tasarımı:** Son aşamada MVC (Model-View-Controller) prensibine tam uygun olarak, node.js bağımlılığı yaratmayan harici **Tailwind CSS** kullanılıp modern ve duyarlı (responsive) **Razor Views** sayfaları tasarlanmış ve arka plandaki Controller'lara bağlanmıştır.
---
## 🛠️ Teknoloji Yığını (Tech Stack)
* **Backend / Core:** C#, ASP.NET Core 8 MVC
* **Yazılım Mimarisi:** N-Tier Architecture (Çok Katmanlı Mimari), Repository Design Pattern, Dependency Injection (DI)
* **Veritabanı & ORM:** MS SQL Server, Entity Framework Core (Code-First)
* **Frontend:** Tailwind CSS, HTML5, Razor Views


## 🚀 Kurulum ve Çalıştırma

### 1. Projeyi Klonla

```bash
git clone https://github.com/KULLANICI_ADINIZ/DEPONUN_ADI.git
cd DEPONUN_ADI
```

---

### 2. Connection String Ayarla

`WebUI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SIZIN_SUNUCU_ADINIZ;Database=ExecutiveLedgerDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

---

### 3. Veritabanını Oluştur

#### Visual Studio

```powershell
Update-Database
```

#### Terminal

```bash
dotnet ef database update --startup-project ../CrmProject.WebUI
```

---

### 4. Uygulamayı Başlat

```bash
cd CrmProject.WebUI
dotnet run
```

Tarayıcıda aç:
```
http://localhost:xxxx
```

---

## ✅ Gereksinimler

- .NET 8 SDK
- SQL Server

---

## ⚠️ Olası Hatalar

| Hata | Çözüm |
|------|------|
| Connection hatası | Server adını kontrol et |
| Migration hatası | DataAccess seç |
| Port hatası | launchSettings.json değiştir |

---

## 📄 Lisans

Portföy amaçlı geliştirilmiştir.
