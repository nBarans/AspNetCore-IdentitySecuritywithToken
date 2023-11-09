using HavaDurumu.Modeller;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HavaDurumu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KimlikDenetimiController : ControllerBase
    {
        private readonly JwtAyarlari _jwtAyarlari;
        public KimlikDenetimiController(IOptions<JwtAyarlari>jwtAyarlari)
        {
            _jwtAyarlari = jwtAyarlari.Value;    
        }
        [HttpPost("Giris")]
        public IActionResult Giris([FromBody] ApiKullanicisi apiKullaniciBilgileri) 
        {
            var apiKullanicisi = KimlikDenetimiYap(apiKullaniciBilgileri);
            if (apiKullanicisi==null)
            {
                return NotFound("Kullanıcı Bulunamadı..");
            }
            var token = TokenOlustur(apiKullanicisi);
            return Ok(token);

        }

        private string TokenOlustur(ApiKullanicisi apiKullanicisi)
        {
            if(_jwtAyarlari.Key==null)
            {
                throw new Exception("Jwt Ayarlarındaki key değeri null olamaz");

            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAyarlari.Key));
            var credentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var claimDizisi = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,apiKullanicisi.KullanıcıAdi ),
                new Claim(ClaimTypes.Role,apiKullanicisi.Rol!)

            };
            var token = new JwtSecurityToken(_jwtAyarlari.Issuer,
                _jwtAyarlari.Audience,
                claimDizisi,
                expires:DateTime.Now.AddHours(1),
                signingCredentials:credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private ApiKullanicisi? KimlikDenetimiYap(ApiKullanicisi apiKullaniciBilgileri)
        {
            return ApiKullanicilari
                .Kullanıcılar
                .FirstOrDefault(x => x.KullanıcıAdi?.ToLower() == apiKullaniciBilgileri.KullanıcıAdi && x.Sifre == apiKullaniciBilgileri.Sifre);
        }
    }
}
