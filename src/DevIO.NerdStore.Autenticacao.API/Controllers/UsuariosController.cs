using DevIO.NerdStore.Autenticacao.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevIO.NerdStore.Autenticacao.API.Controllers
{
    [ApiController]
    [Route("api/autenticacao")]
    public class UsuariosController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UsuariosController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistroViewModel usuarioViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = new IdentityUser()
            {
                UserName = usuarioViewModel.Email,
                Email = usuarioViewModel.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, usuarioViewModel.Senha);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Logar(UsuarioLoginViewModel usuarioLoginViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _signInManager.PasswordSignInAsync(usuarioLoginViewModel.Email, usuarioLoginViewModel.Senha, false, true);

            if (result.Succeeded)
                return Ok();

            return BadRequest();
        }
    }
}
