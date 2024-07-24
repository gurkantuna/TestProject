using System;
using System.Net;
using BlogApp.DataAccess.EntitFrameworkCore.Abstract;
using BlogApp.Entity.Concrete;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.Controllers {
    public class ErrorController : Controller {

        public ErrorController(ILogRepo logRepo) {
            _logRepo = logRepo;
        }

        private readonly ILogRepo _logRepo;

        [Route("error")]
        public IActionResult Error() {
            var exceptionfeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var log = new Log {
                Audit = "ERROR",
                Date = DateTime.Now,
                Detail = $"Path : {exceptionfeature?.Path} - Message : {exceptionfeature?.Error?.Message} - StackTrace : {exceptionfeature?.Error?.StackTrace}",
                User = User.Identity.IsAuthenticated ? User.Identity.Name : "Anonymous",
                Ip = HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logRepo.Add(log);

            return View();
        }

        [Route("error/{statusCode}")]
        public IActionResult Error(int statusCode) {

            var status = (HttpStatusCode)statusCode;

            switch (status) {
                case HttpStatusCode.BadRequest:
                    return View("_400");
                case HttpStatusCode.Unauthorized:
                    return View("_401");
                case HttpStatusCode.PaymentRequired:
                    break;
                case HttpStatusCode.Forbidden:
                    return View("_403");
                case HttpStatusCode.NotFound:
                    return View("_404");
                case HttpStatusCode.MethodNotAllowed:
                    break;
                case HttpStatusCode.RequestTimeout:
                    break;
                case HttpStatusCode.RequestEntityTooLarge:
                    break;
                case HttpStatusCode.RequestUriTooLong:
                    break;
                case HttpStatusCode.UnsupportedMediaType:
                    break;
                case HttpStatusCode.RequestedRangeNotSatisfiable:
                    break;
                case HttpStatusCode.ExpectationFailed:
                    break;
                case HttpStatusCode.TooManyRequests:
                    break;
                case HttpStatusCode.InternalServerError:
                    return View("_500");
                case HttpStatusCode.NotImplemented:
                    break;
                case HttpStatusCode.BadGateway:
                    break;
                case HttpStatusCode.ServiceUnavailable:
                    break;
                case HttpStatusCode.GatewayTimeout:
                    break;
            }

            return View();
        }
    }
}