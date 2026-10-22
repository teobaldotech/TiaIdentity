```csharp
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TiaIdentity
{
    public sealed class Autenticador
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Autenticador(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task LoginAsync(IUsuario usuario, bool lembrar)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            await LoginAsync(
                usuario.Login,
                usuario.Nome,
                lembrar,
                new List<string> { usuario.Perfil });
        }

        public async Task LoginAsync(
            string login,
            string nome,
            bool lembrar,
            string perfil,
            List<Claim>? outrasClaims = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(perfil);

            await LoginAsync(
                login,
                nome,
                lembrar,
                new List<string> { perfil },
                outrasClaims);
        }

        public async Task LoginAsync(
            string login,
            string nome,
            bool lembrar,
            List<string>? perfis,
            List<Claim>? outrasClaims = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(login);
            ArgumentException.ThrowIfNullOrWhiteSpace(nome);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, login),
                new(ClaimTypes.Name, nome)
            };

            if (perfis != null)
            {
                claims.AddRange(
                    perfis
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .Select(p => new Claim(ClaimTypes.Role, p)));
            }

            if (outrasClaims != null)
            {
                foreach (var claim in outrasClaims)
                {
                    ArgumentNullException.ThrowIfNull(claim);

                    if (claim.Type == ClaimTypes.NameIdentifier ||
                        claim.Type == ClaimTypes.Name ||
                        claim.Type == ClaimTypes.Role)
                    {
                        throw new ArgumentException(
                            "Claims adicionais não podem substituir a identidade ou os perfis.",
                            nameof(outrasClaims));
                    }

                    claims.Add(claim);
                }
            }

            await EfetuarLoginAsync(lembrar, claims);
        }

        private async Task EfetuarLoginAsync(
            bool lembrar,
            IEnumerable<Claim> claims)
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException(
                    "HttpContext não disponível.");

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = lembrar,
                AllowRefresh = true,
                IssuedUtc = DateTimeOffset.UtcNow
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                properties);
        }

        public async Task LogoutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException(
                    "HttpContext não disponível.");

            await httpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public string? LoginUsuario =>
            _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

        public string? NomeUsuario =>
            _httpContextAccessor.HttpContext?
                .User?
                .Identity?
                .Name;

        public bool EstaAutenticado =>
            _httpContextAccessor.HttpContext?
                .User?
                .Identity?
                .IsAuthenticated ?? false;

        public List<string> Perfis =>
            _httpContextAccessor.HttpContext?
                .User?
                .FindAll(ClaimTypes.Role)
                .Select(claim => claim.Value)
                .ToList()
            ?? new List<string>();

        public string? ObterClaim(string tipo)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(tipo);

            return _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(tipo)?
                .Value;
        }

        public bool PossuiPerfil(string perfil)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(perfil);

            return _httpContextAccessor.HttpContext?
                .User?
                .IsInRole(perfil) ?? false;
        }
    }
}
```
