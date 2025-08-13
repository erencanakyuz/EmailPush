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

TODO: Swagger dokümantasyonu ekle - API endpoints'lere açıklama ve örnek ekle
TODO: EmailConsumer'da gerçek email gönderim implementasyonu yap
TODO: FluentValidation ekle campaign validasyon için
TODO: API response modelleri ekle (DTO'lar)
TODO: Error handling middleware ekle
TODO: Logging ekle (Serilog)
TODO: appsettings'e email provider configuration ekle
Eklenebilir...

Ekstralar
batch ile gönderme emailleri ya da nasıl kullanabilirsek,(tartışılabilir)
post request kullan get le gondertme mail (tartışılabilir)
default hizli gonderme (tartışılabilir)
api genis olsun, opsiyon bol olsun (tartışılabilir)
