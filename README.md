# NOBK klasör seçici

Açılışta oyun klasörü seçtirir; sadece bu klasördeki EXE dosyalarını gösterir. nobk.exe (harf büyüklüğünden bağımsız) ve başlatıcının kendisi listelenmez. Alt klasör taraması varsayılan olarak kapalıdır; kutucuğu işaretleyerek açabilirsiniz. Klasör bağlantıları takip edilmez.

## Kullanım

1. dist/nobk.exe dosyasını açın ve örneğin Sonic 4 klasörünü seçin.
2. EXE'yi seçip çalıştır düğmesine basın veya çift tıklayın.
3. Gerekirse başlatma parametrelerini girin; parametre olarak verilen yolları tırnak içine alın.

Klasör seçimini iptal ederseniz Klasör seç düğmesini kullanabilirsiniz. Oyunlar kendi EXE klasörlerinden çalışır. Başlatıcı oyun açıkken küçülür ve açık kalır. Başlatılan işlem kapandığında geri gelir. Ayrı işlem açıp kapanan oyun başlatıcılarında asıl oyun EXE'sini seçin veya pencereyi açık bırakın. Kurulum ve kaldırma EXE'leri de listelenebilir; oyunun dosyasını seçin. TXT/BAT oyun listesi gerekmez.

## Steam / NOBK kurulumu

Önceki rehber Steam'in NOBK klasöründeki nobk.exe girişini kullanıyor:
https://steamcommunity.com/sharedfiles/filedetails/?id=3765007019

1. Steam → NOBK Overlay → Özellikler → Yüklü dosyalar → Göz at.
2. Steam/NOBK ve oyun kapalıyken orijinal nobk.exe dosyasını kurulum klasörü DIŞINA yedekleyin. Diğer NOBK dosyalarını taşımayın.
3. Paketteki nobk.exe dosyasını NOBK kurulum klasörüne kopyalayın.
4. Steam'den NOBK Overlay'i açın ve istediğiniz oyun klasörünü seçin. Oyunları NOBK klasörüne taşımak gerekmez.
5. Oyundayken Shift+Tab ile overlay'i kontrol edin.

Doğrudan EXE'yi açmak Steam/NOBK oturumunu başlatmaz. Steam girişini değiştirmek için başlatıcı adı nobk.exe olarak kalır; yalnızca oyun listesinden hariç tutulur. Başlatıcı oyunu alt işlem olarak başlatır, ortam değişkenlerini devralır ve açık kalır. Overlay uyumluluğu gerçek Steam/oyun oturumunda kontrol edilmelidir; bu bilgisayarda henüz doğrulanmadı. Yönetici yetkisi isteyen oyunlar için normal başlatma hata gösterebilir. Steam güncellemeleri/dosya doğrulaması orijinal EXE'yi geri getirebilir.

Geri almak için Steam ve oyunu kapatıp orijinal yedeği geri kopyalayın veya Steam dosya doğrulamasını kullanın. Bu paket kurulumunuzu otomatik değiştirmez.

## Dosyalar ve derleme

- Launcher.cs: pencere, tarama ve çalıştırma.
- build.ps1: Windows .NET Framework derleyicisiyle EXE üretir.
- Tests.cs: kapsam, hariç tutma ve gerçek alt işlem çalıştırma kontrolleri.

Windows ve .NET Framework 4.5+ gerekir (Windows 10/11'de 4.8 uygundur). .NET 8 kurulumu gerekmez.
Derleme: powershell -ExecutionPolicy Bypass -File .\build.ps1
