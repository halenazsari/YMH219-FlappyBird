# YMH219 - Flappy Bird

Nesne Tabanlı Programlama (YMH 219) dersi ek uygulama ödevi. Windows Forms ile geliştirilmiş,
Timer ve klavye olaylarını kullanan Flappy Bird benzeri bir masaüstü oyunudur. Kuş, yer çekimi
etkisiyle düşer; oyuncu klavyeyle zıplatarak borular arasındaki boşluktan geçirmeye çalışır.

## Kullanılan Teknolojiler

- C# / .NET 8 (`net8.0-windows`)
- Windows Forms
- Harici NuGet paketi kullanılmamıştır.

## Kurulum ve Çalıştırma

1. Depoyu klonlayın:
   ```bash
   git clone https://github.com/halenazsari/YMH219-FlappyBird.git
   ```
2. `FlappyBird.sln` dosyasını Visual Studio 2022 ile açın (".NET masaüstü geliştirme" iş yükü kurulu olmalıdır).
3. `F5` ile derleyip çalıştırın.

Komut satırından çalıştırmak için (Windows):

```bash
dotnet run --project FlappyBird
```

## Kontroller

| Tuş | İşlev |
| --- | --- |
| `Space` / `↑` | Zıpla (ilk basışta oyun başlar) |
| `R` | Game Over ekranında yeniden başlat |
| **Yeni Oyun** butonu | Oyunu sıfırlar |

## Temel Özellikler

- Kuş klavye ile yukarı zıplar, yer çekimiyle aşağı iner.
- İki boru çifti sağdan sola hareket eder; ekrandan çıkan boru rastgele boşluk yüksekliğiyle yeniden konumlanır.
- Boruya veya zemine çarpınca oyun durur ve **GAME OVER** ekranı gösterilir.
- Her geçilen engel skoru 1 artırır.
- **Yeni Oyun** butonu ve `R` tuşu ile yeniden başlatma.
- **İsteğe bağlı:** En yüksek skor `highscore.txt` dosyasında saklanır.
- **İsteğe bağlı:** Her 5 puanda seviye atlanır ve engellerin hızı artar.

## Proje Yapısı ve Sınıf Tasarımı

```
FlappyBird/
├── Program.cs               Uygulama giriş noktası
├── MainForm.cs              Pencere, Timer, klavye olayları, çizim ve arayüz
├── Game/
│   ├── GameEngine.cs        Oyun kuralları: güncelleme, skor, seviye, çarpışma
│   └── GameState.cs         Ready / Running / GameOver durumları
├── Models/
│   ├── Bird.cs              Kuşun konumu, hızı, yer çekimi ve zıplama
│   └── Pipe.cs              Üst/alt boru çifti, hareket ve çarpışma alanı
└── Services/
    └── HighScoreStore.cs    Rekorun dosyaya kaydedilmesi/okunması
```

- **MainForm** yalnızca arayüzle ilgilenir: Timer her 16 ms'de `GameEngine.Update()` çağırır ve `Invalidate()` ile ekranı yeniden çizdirir.
- **GameEngine** formdan bağımsızdır; skor değişimini ve oyunun bitişini `ScoreChanged` ve `GameEnded` olaylarıyla (event) forma bildirir.
- Hız, skor, seviye ve durum bilgileri `GameEngine` sınıfının özellikleri (property) olarak tutulur.
- Çarpışma kontrolü `RectangleF.IntersectsWith` ile dikdörtgenler üzerinden yapılır.

## Ekran Görüntüleri

| Başlangıç | Oyun | Game Over |
| --- | --- | --- |
| ![Başlangıç](docs/screenshots/start.png) | ![Oyun](docs/screenshots/gameplay.png) | ![Game Over](docs/screenshots/gameover.png) |

## Bilinen Sınırlamalar / Geliştirme Notları

- Grafikler GDI+ ile çizilmiştir, resim dosyası kullanılmamıştır.
- Ses efekti eklenmemiştir.
- Oyun sabit 480x640 pencere boyutunda çalışır.

## Öğrenci

- **Ad Soyad:** Halenaz Sarı
- **Öğrenci No:** _(buraya öğrenci numaranızı yazın)_
