namespace HavaDurumu.Modeller
{
    public class ApiKullanicilari
    {
        public static List<ApiKullanicisi> Kullanıcılar = new()
        {
            new ApiKullanicisi {Id=1,KullanıcıAdi="sinan",Sifre="12345",Rol="Yönetici"},
            new ApiKullanicisi {Id=2,KullanıcıAdi="ilyas",Sifre="12345",Rol="Standart Kullanıcı"}


        };
    }
}
