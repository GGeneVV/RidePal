using Microsoft.AspNetCore.Http;
using RidePal.Services.Exceptions;
using System.Threading.Tasks;

namespace RidePal.Web.Utilities.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

                if (context.Response.StatusCode == 404)
                {
                    context.Response.Redirect("/error/pagenotfound");
                }
            }
            catch (EntityAlreadyExistsException ex)
            {
                context.Response.Redirect($"/error/alreadyexistserror?error={ex.Message}");
            }
            catch (EntityInvalidException ex)
            {
                context.Response.Redirect($"/error/invalid?error={ex.Message}");
            }

        }
    }
}
