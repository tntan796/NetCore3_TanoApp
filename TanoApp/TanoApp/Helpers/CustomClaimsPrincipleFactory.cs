using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using TanoApp.Data.Entities;
using TanoApp.Data.Enums;

namespace TanoApp.Helpers
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        private UserManager<AppUser> _userManger;

        public CustomClaimsPrincipalFactory(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
            _userManger = userManager;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(AppUser user)
        {
            var principal = await base.CreateAsync(user);
            var roles = await _userManger.GetRolesAsync(user);
            ((ClaimsIdentity)principal.Identity).AddClaims(new[]
            {
                new Claim("UserName",user.UserName),
                new Claim("Avatar",user.Avatar ?? string.Empty),
                new Claim("FullName",user.FullName ?? string.Empty),
                new Claim("Email",user.Email ?? string.Empty),
                new Claim("PhoneNumber",user.PhoneNumber ?? string.Empty),
                new Claim("Status", user.Status.ToString()),
                new Claim("Roles",string.Join(";",roles))
            });
            return principal;
        }
    }
}