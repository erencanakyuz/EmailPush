PROJE KAPSAM:
FAZ -1
Projenin Amacı
Bu projedeki temel amaç, basit bir e-posta gönderim API’si geliştirmektir. Çok fazla detaya
girmeden, bir kampanya için temel CRUD işlemlerini gerçekleştirmeniz, ardından bu
kampanyaları bir kuyruğa yazarak gönderime hazır hale getirmeniz beklenmektedir.
Arka planda çalışan bir Worker Service veya Background Service ile kuyruktaki mesajları
tüketin (consume edin) ve e-posta gönderimi simüle edilerek console ekranına gönderildi
bilgisi yazdırılsın.
API üzerinde şu özellikler beklenmektedir:
● Kampanya CRUD işlemleri
● E-posta gönderimini başlatma
● Gönderim istatistikleri (kaç tane gönderildi kaç kampanya var vs)
Araştırmanız Gereken Konular
● Clean Architecture nedir? Proje yapısı için uygun bir mimariye nasıl karar verilir?
● Entity Framework ile MS SQL kullanımı
● Generic Repository Pattern nedir, nasıl uygulanır?
● Dependency Injection nedir, neden kullanılır?
● Worker Service nedir? Nasıl oluşturulur?
● RabbitMQ nedir? MassTransit nedir, ne işe yarar? Kullanılmazsa ne olur?
○ Publisher – Consumer mimarisi nedir?
○ Command ve Event kavramları nelerdir, aralarındaki fark nedir?
● Option Pattern
Beklenen sonuçlar
● Proje GitHub’a yüklenecek.
● Anlaşılır ve detaylı bir README.md dosyası olacak.
● API dökümantasyonu için Swagger veya Postman collection hazırlanmalı.
● UI geliştirmesi beklenmemektedir.
● Options Pattern ile appsettings.json üzerinden konfigürasyon verileri okunmalıdır.
--------------

FAZ -2 TAMAMLANDI
● Exception Handling: Global bir hata yönetimi mekanizması kuruldu
● Logging: Serilog entegre edildi, konsol ve dosya loglama yapılandırıldı
● Middleware: Monitoring middleware eklendi
● Monitoring Tool: Temel sekiyde izleme mekanizması eklendi
● Rate limiting: AspNetCoreRateLimit entegre edildi
● Retry mekanizması: Generic retry servisi oluşturuldu

TAMAMLANANLAR:
✓ Swagger dokümantasyonu eklendi
✓ EmailConsumer'da gerçek email gönderim simülasyonu yapıldı
✓ FluentValidation eklendi campaign validasyon için
✓ API response modelleri eklendi (DTO'lar)
✓ Error handling middleware eklendi
✓ Logging eklendi (Serilog) - FAZ2 kapsamında geliştirildi
✓ appsettings'e email provider configuration eklendi
✓ Exception Handling: Global bir hata yönetimi mekanizması kuruldu - FAZ2
✓ Logging: Konsol haricinde bir log hedefi kullanın (örnek Graylog) - FAZ2
✓ Middleware kullanın - FAZ2
✓ Monitoring Tool: Temel seviyede izleme aracı eklendi - FAZ2
✓ Rate limiting eklendi - FAZ2
✓ Retry mekanizması eklendi - FAZ2

Ekstralar
batch ile gönderme emailleri ya da nasıl kullanabilirsek,(tartışılabilir)
post request kullan get le gondertme mail (tartışılabilir)
default hizli gonderme (tartışılabilir)
api genis olsun, opsiyon bol olsun (tartışılabilir)


TODO: kampanya duzenleme ekranında mevcut verinin otomatik oalrak doldurulmasu gerekiyor bence tartışalım PUT/api/Campaigns/{id} bu kısımda,