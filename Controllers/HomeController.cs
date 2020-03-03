using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Shop.Services;
using Shop.Repositories;

namespace Shop.Controllers
{
    [Route("account")]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody]User model)
        {
            var user = UserRepository.Get(model.Username, model.Password);

            if(user == null)
            {
                return NotFound(new { message = "Usuario ou senha invalida" });
            }

            var token = TokenService.GenerateToken(user);

            user.Password = "";

            return new { user = user, token = token };            
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string anonymous() => "Qualquer usuario pode acessar essa rota";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string authenticated() => string.Format("Somente usuarios autenticados - Nome do usuario logado: {0}", User.Identity.Name); 
        //User.Identity.Name = tras o nome do usuario autenticado

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee,manager")]
        public string Employee() => "Qualquer funcionario ou gerente pode acessar essa rota";

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager() => "Somente gerente pode acessa essa rota";
    }
}