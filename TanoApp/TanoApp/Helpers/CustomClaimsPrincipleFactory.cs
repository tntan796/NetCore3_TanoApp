using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TanoApp.Data.Entities;

namespace TanoApp.Helpers
{
    public class CustomClaimsPrincipleFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        UserManager<AppUser> _userManager;
        public CustomClaimsPrincipleFactory(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
            _userManager = userManager;
        }


        public async override Task<ClaimsPrincipal> CreateAsync(AppUser user)
        {
            var principal = await base.CreateAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Email", user.Email != null ? user.Email : string.Empty));
            claims.Add(new Claim("UserName", user.UserName ?? string.Empty));
            claims.Add(new Claim("FullName", user.FullName ?? string.Empty));
            claims.Add(new Claim("Avatar", user.Avatar ?? string.Empty));
            claims.Add(new Claim("Role", string.Join(";", roles)));
            ((ClaimsIdentity)principal.Identity).AddClaims(claims);
            System.Console.WriteLine(principal);
            return principal;
        }
    }
}