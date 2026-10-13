# GameSelector

Oyun klasörünü seçin; klasörün tamamındaki ve bütün alt klasörlerdeki EXE dosyaları otomatik listelenir. Alt klasör taraması her zaman açıktır. Klasör bağlantıları takip edilmez; erişilemeyen klasörlerin sayısı gösterilir. nobk.exe, gameselector.exe ve çalışmakta olan başlatıcının kendi dosyası listelenmez.

## Kullanım ve hatırlama

1. gameselector.exe dosyasını açın. İlk kullanımda oyun klasörünü seçin.
2. EXE'yi seçip çalıştır düğmesine basın veya çift tıklayın.
3. Aynı adla tekrar açtığınızda kayıtlı klasör otomatik taranır; tekrar seçmeniz gerekmez.
4. Klasörü değiştirmek için Klasör seç düğmesine basın. Yeni seçim aynı EXE adının kaydını günceller.

Hatırlama başlatıcının dosya adına bağlıdır:

- SonicMania.exe → o adla seçtiğiniz klasör.
- nobk.exe → bağımsız bir klasör seçimi.
- Yeni, daha önce kullanılmamış bir ad → yeniden klasör seçimi.
- Eski ada geri dönmek → o adın eski seçimini geri getirir.

Aynı adı taşıyan kopyalar, konumları farklı olsa da aynı Windows kullanıcısında aynı kaydı kullanır. Sadece büyük/küçük harf değişikliği yeni kayıt oluşturmaz. Kayıtlı klasör artık yoksa tekrar klasör seçtirilir. Seçimi iptal etmek önceki kaydı değiştirmez.

Kayıtlar kullanıcının %LOCALAPPDATA%\GameSelector\Folders klasöründedir. EXE yanında ayar dosyası gerekmez; başka bir Windows kullanıcısına veya bilgisayara kopyalandığında yeniden seçim yapılır. Buradaki kayıt dosyaları otomatik klasör tercihleridir; elle düzenlenen oyun listeleri değildir.

Oyunlar kendi EXE klasörlerinden başlatılır. Başlatıcı oyun açıkken küçülür ve açık kalır. Başlatılan işlem kapandığında geri gelir. Ayrı işlem açıp kapanan oyun başlatıcılarında asıl oyun EXE'sini seçin veya pencereyi açık bırakın. Başlatma parametreleri isteğe bağlıdır; parametre olarak verilen yolları tırnak içine alın. Kurulum/kaldırma EXE'leri de listelenebilir; oyunun dosyasını seçin. TXT/BAT oyun listesi gerekmez.

## Steam ve diğer kullanım

GameSelector bağımsız olarak veya başka bir başlatıcı üzerinden kullanılabilir. NOBK kullanmak zorunlu değildir. NOBK/Steam girişinde kullanacaksanız gameselector.exe dosyasının bir kopyasını nobk.exe olarak adlandırın. Orijinal NOBK EXE'sini kurulum klasörü dışına yedekleyip bu kopyayı onun yerine koyun, sonra Steam'den NOBK'yi başlatın. Diğer NOBK dosyalarını taşımayın. nobk.exe adına ait klasör seçimi diğer adlardan ayrıdır.

Kullanıcı önceki sürümde Steam overlay'in çalıştığını doğruladı. Bu sürümde oyunu alt işlem olarak açma, çalışma klasörü, ortam değişkenlerini devralma ve oyun açıkken başlatıcıyı açık tutma davranışı korunur. Güncellenen sürüm gerçek Steam/oyun oturumunda yeniden test edilmedi. Steam güncellemeleri/dosya doğrulaması orijinal EXE'yi geri getirebilir. Geri almak için orijinal yedeği geri kopyalayın veya Steam dosya doğrulamasını kullanın. Bu paket kurulumunuzu otomatik değiştirmez.

## Dosyalar ve derleme

- Launcher.cs: pencere, otomatik alt klasör taraması, dosya adına göre hatırlama ve çalıştırma.
- build.ps1: Windows .NET Framework derleyicisiyle gameselector.exe üretir.
- Tests.cs: tarama, adlara göre ayrı kayıtlar, yeniden açma ve gerçek alt işlem çalıştırma kontrolleri.

Windows ve .NET Framework 4.5+ gerekir (Windows 10/11'de 4.8 uygundur). .NET 8 kurulumu gerekmez.
Derleme: powershell -ExecutionPolicy Bypass -File .\build.ps1
