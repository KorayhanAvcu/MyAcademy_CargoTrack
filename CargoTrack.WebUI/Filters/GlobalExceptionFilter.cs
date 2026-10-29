using CargoTrack.Business.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CargoTrack.WebUI.Filters
{
    public class GlobalExceptionFilter(ITempDataDictionaryFactory _tempDataFactory,
                                       ILogger<GlobalExceptionFilter> _logger) : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var tempData = _tempDataFactory.GetTempData(context.HttpContext);

            switch (context.Exception)
            {
                case BusinessException ex:
                    tempData["error"] = ex.Message;
                    var referer = context.HttpContext.Request.Headers.Referer.ToString();
                    context.Result = new RedirectResult(string.IsNullOrEmpty(referer) ? "/" : referer);
                    break;

                case UnauthorizedAccessException:
                    context.Result = new RedirectToActionResult("AccessDenied", "ErrorPages", new { area = "" });
                    break;

                default:
                    _logger.LogError(context.Exception, "Beklenmeyen hata");
                    context.Result = new RedirectToActionResult("Error", "ErrorPages", new { area = "" });
                    break;
            }

            context.ExceptionHandled = true;
        }
    }
}
