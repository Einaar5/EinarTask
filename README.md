<!DOCTYPE html>
<html lang="tr">
<head>
  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>EinarTask Management</title>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
  <style>
    body {
      background-color: #0f172a;
      color: #f1f5f9;
      font-family: "Segoe UI", sans-serif;
    }
    h1, h2, h3 {
      color: #38bdf8;
      font-weight: 700;
    }
    .section {
      padding: 60px 0;
    }
    .feature-box {
      background: #1e293b;
      border-radius: 10px;
      padding: 25px;
      transition: 0.3s;
    }
    .feature-box:hover {
      transform: translateY(-5px);
      background: #334155;
    }
    .code-box {
      background: #1e293b;
      border-radius: 8px;
      padding: 15px;
      color: #93c5fd;
      font-family: monospace;
    }
    footer {
      background: #0f172a;
      padding: 20px 0;
      color: #94a3b8;
      text-align: center;
    }
    .badge {
      margin: 3px;
    }
  </style>
</head>
<body>
  <div class="container text-center py-5">
    <h1>🧭 EinarTask Management</h1>
    <p class="lead">.NET Core MVC & EF Core tabanlı bir görev yönetim sistemi</p>
    <a href="https://github.com/kullaniciadi/EinarTask-Management" class="btn btn-primary mt-3" target="_blank">GitHub Reposuna Git</a>
  </div>

  <section class="section text-center">
    <div class="container">
      <h2>🚀 Özellikler</h2>
      <div class="row mt-4">
        <div class="col-md-4 mb-3">
          <div class="feature-box">
            <h4>🔐 Kimlik Doğrulama</h4>
            <p>Kullanıcı kayıt, giriş ve profil yönetimi ASP.NET Core Identity ile sağlanır.</p>
          </div>
        </div>
        <div class="col-md-4 mb-3">
          <div class="feature-box">
            <h4>📦 Code-First</h4>
            <p>Veritabanı EF Core Code-First yaklaşımıyla oluşturulur ve yönetilir.</p>
          </div>
        </div>
        <div class="col-md-4 mb-3">
          <div class="feature-box">
            <h4>✅ Görev Yönetimi</h4>
            <p>Görev ekleme, düzenleme, silme ve durum takibi özellikleri içerir.</p>
          </div>
        </div>
      </div>
    </div>
  </section>

  <section class="section bg-dark text-center">
    <div class="container">
      <h2>⚙️ Kurulum</h2>
      <p>Projeyi kendi bilgisayarında çalıştırmak için aşağıdaki adımları izle:</p>

      <div class="text-start mt-4 mx-auto" style="max-width: 700px;">
        <div class="code-box mb-3">
          git clone https://github.com/kullaniciadi/EinarTask-Management.git<br>
          cd EinarTask-Management
        </div>
        <p><strong>1️⃣</strong> <code>appsettings.json</code> dosyasını aç ve veritabanı bağlantı bilgisini gir:</p>
        <div class="code-box mb-3">
          "ConnectionStrings": {<br>
          &nbsp;&nbsp;"DefaultConnection": "Server=.;Database=EinarTaskDB;Trusted_Connection=True;TrustServerCertificate=True;"<br>
          }
        </div>

        <p><strong>2️⃣</strong> Visual Studio veya terminalde şu komutu çalıştır:</p>
        <div class="code-box mb-3">update-database</div>

        <p><strong>3️⃣</strong> Uygulamayı başlat:</p>
        <div class="code-box">dotnet run</div>

        <p><strong>4️⃣</strong> Tarayıcıdan <code>https://localhost:5001</code> adresine git, kayıt ol ve görevlerini yönet!</p>
      </div>
    </div>
  </section>

  <section class="section text-center">
    <div class="container">
      <h2>🧩 Kullanılan Teknolojiler</h2>
      <span class="badge bg-primary">.NET 6+</span>
      <span class="badge bg-success">Entity Framework Core</span>
      <span class="badge bg-info text-dark">MVC Pattern</span>
      <span class="badge bg-secondary">MSSQL</span>
      <span class="badge bg-warning text-dark">Bootstrap 5</span>
    </div>
  </section>

  <footer>
    <p>Made with ❤️ by <strong>Can</strong> | EinarTask Management © 2025</p>
  </footer>
</body>
</html>
