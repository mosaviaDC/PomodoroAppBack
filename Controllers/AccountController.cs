using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using PomodoroApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http.Features;
using PomodoroApp.Resources;
using Microsoft.AspNetCore.Authorization;
using MailKit;
using pomodoroapp.Resources;
using pomodoroapp;
using System.Web;
using MimeKit;
using Microsoft.AspNetCore.Cors;

namespace PomodoroApp.Controllers
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController  : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtGenerator _jwtGenerator;


        public AccountController(UserManager<User> userManager,  JwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
        }


        [HttpGet]
        public IActionResult GetCurrentUser()
        {
         
            if (User.Identity.IsAuthenticated)
            {
              
                var user = _userManager.Users.First(u => u.Email == User.Identity.Name);

                return Ok(user);
            }
            else return BadRequest();
            
           
            
            
        }


        [HttpPost("forgot")]
        [AllowAnonymous]
        public async Task <IActionResult> ForgotPassword (ForgotPasswordModel forgotPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
                if (user == null)
                {
                    // Если пользователь отсуствует, то все равно возвращаем OK
                    return Ok();
                }
                var code = HttpUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(user));

               
             

                var callbackUrl = $"https://pomodorotracker.ru/reset?code={code}&email={user.Email}&id={user.Id}";
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = $"<body><div> " +
                    $"<h4> Здравствуйте, {user.firstName}! " + "Для вашего аккаунта зарегистрирована заявка на смену пароля.</h4>" +
                    $"<h3>Для смены пароля перейдите по ссылке ниже</h3>"
                    +$"<p> <link>{callbackUrl} </link> </p>"+
                    "Если это бы ли не вы, просто проигнорируйте данное письмо."+
                    $"</div> " +
                    $"<h4>Good luck! ☺️"+
                    $"<p>С уважением,</p> <p>Администрация сайта pomodorotracker.ru  admin@pomodorotracker.ru </body>";
                bodyBuilder.TextBody = callbackUrl;

                EmailService emailService = new EmailService();
              
             await emailService.SendEmailAsync(forgotPasswordModel.Email, "Сброс пароля", bodyBuilder);
           

                return Ok();
            }
            return BadRequest();


        }






        [HttpPost("reset")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ResetPasswordAsync(user,HttpUtility.UrlDecode( model.Code), model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(result.Errors);



        }





         [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(RegisterModel registerModel)
        {

            var isAlreadyRegisted = await _userManager.FindByEmailAsync(registerModel.Email);
            if (isAlreadyRegisted is null)
            {
                User user = new User() { Email = registerModel.Email, UserName = registerModel.Email, firstName = registerModel.firstName };
                var res = await _userManager.CreateAsync(user, registerModel.Password);
                //Если пользователь уже есть
                if (res.Succeeded)
                {


                    JObject jObject = JObject.FromObject(user);
                    jObject.Add("token", _jwtGenerator.GenerateJwt(user));
                    return CreatedAtAction("signup", jObject);
                }
                else
                    return BadRequest("модель заполнена неверно");
         

            }
            else {

                return Conflict("такой пользователь уже есть");
            }

           
        }
        /// <summary>
        /// Вход
        /// </summary>
        /// <param name="userLoginResource"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(LoginModel userLoginResource)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Email == userLoginResource.Email);
            if (user is null)
            {
                return NotFound("Такого пользователя не существует");
            }
            var userSigninResult = await _userManager.CheckPasswordAsync(user, userLoginResource.Password);
            if (userSigninResult)
            {
                JObject jObject = new JObject();
                jObject = JObject.FromObject(user);
                jObject.Add("token", _jwtGenerator.GenerateJwt(user));

                return Ok(jObject);
            }

            return BadRequest("Логин или пароль неверны");
        }


     



    }



}
