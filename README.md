Selling Project
------------------------------

• Bu proje, modern microservis mimarisi üzerine kurulu olup, her servis kendi veritabanına ve iş mantığına sahip olacak şekilde izole edildi. 

• .Net 8, C# dili ile geliştirildi. Her servis için bağımsız yapı kuruldu. 

• Frontend tarafında Blazor kullanılarak kullanıcı dostu ve interaktif bir arayüz oluşturuldu. 

• Api Gateway: Ocelot kullanılarak mikroservislere gelen istekler yönetildi.

• Service Discovery: Consul kullanılarak dinamik servis keşfi ve sağlık kontrolleri sağlandı.

• CQRS, MediatR ve Generic Repository tasarım desenleri kullanıldı.

• EventBus altyapısı geliştirildi ve RabbitMQ ile servisler arası haberleşme sağlandı. Alternatif olarak Azure Service Bus desteği de entegre edildi.

• JWT ile güvenli authentication yapısı kuruldu. 

• Redis kullanılarak özellikle sepet işlemlerinde yüksek performans elde edildi.

• Tüm servisler Docker ile container haline getirildi. Geliştirme ve test süreçleri için docker-compose kullanıldı. 

• Domain-Driven Design(DDD) prensiplerine uygun olarak servisler katmanlı ve test edilebilir biçimde kurgulandı. 
