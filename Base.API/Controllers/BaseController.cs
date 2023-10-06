using Base.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using OkResult = Base.API.Models.OkResult;

namespace Base.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public int GetUserId()
        {
            return UserData.UserId;
        }
        public void AuditInsert(AuditCreate model)
        {
            model.CreatedBy = GetUserId();
            model.CreatedAt = DateTime.Now;
            model.IsActive = true;
        }
        public void AuditUpdate(AuditCreateUpdate model)
        {
            model.UpdatedBy = GetUserId();
            model.UpdatedAt = DateTime.Now;
        }
        public void AuditDelete(FullAudit model)
        {
            model.DeletedBy = GetUserId();
            model.DeletedAt = DateTime.Now;
            model.IsActive = false;
        }
        public void ReActive(AuditCreate baseClass)
        {
            baseClass.IsActive = true;
        }
        protected ObjectResult OkResult(string message)
        {
            return CreateResult(StatusCodes.Status200OK, "OK", message);
        }

        protected ObjectResult OkResult(object result = null)
        {
            return CreateResult(StatusCodes.Status200OK, "OK", "The request was successful.", result);
        }
        protected ObjectResult OkResult(object result = null, string message= "The request was successful")
        {
            return CreateResult(StatusCodes.Status200OK, "OK", message, result);
        }

        protected ObjectResult CreatedResult(object result = null, string message = "The request was successful and a resource was created.")
        {
            return CreateResult(StatusCodes.Status201Created, "CREATED", message, result);
        }

        protected ObjectResult UpdatedResult(object result = null, string message = "The request was successful and a resource was updated.")
        {
            return CreateResult(StatusCodes.Status205ResetContent, "UPDATED", message, result);
        }

        protected ObjectResult DeletedResult(string message = "The request was successful and a resource was deleted.")
        {
            return CreateResult(StatusCodes.Status204NoContent, "DELETED", message);
        }

        protected ObjectResult NoContentResult(string message = "The request was successful but there is no representation to return.")
        {
            return CreateResult(StatusCodes.Status204NoContent, "NO_CONTENT", message);
        }

        protected ObjectResult BadRequestResult(string message = "The request could not be understood or was missing required parameters.")
        {
            return CreateResult(StatusCodes.Status400BadRequest, "BAD_REQUEST", message);
        }

        protected ObjectResult NotFoundResult(string message = "Resource was not found.")
        {
            return CreateResult(StatusCodes.Status404NotFound, "NOT_FOUND", message);
        }

        protected ObjectResult UnauthorizedResult(string message = "Authentication failed or user doesn't have permissions for requested operation.")
        {
            return CreateResult(StatusCodes.Status401Unauthorized, "UNAUTHORIZED", message);
        }

        protected ObjectResult ValidationResult(string message = "")
        {
            return CreateResult(StatusCodes.Status422UnprocessableEntity, "VALIDATION", message);
        }

        protected ObjectResult ServerErrorResult(string message = "")
        {
            return CreateResult(StatusCodes.Status500InternalServerError, "INTERNAL_ERROR", message);
        }

        protected ObjectResult ConnectErrorResult(string message = "")
        {
            return CreateResult(StatusCodes.Status502BadGateway, "CONNECT_ERROR", message);
        }

        protected ObjectResult FailureResult(string message = "")
        {
            return CreateResult(StatusCodes.Status502BadGateway, "FAILURE", message);
        }

        protected ObjectResult ServiceUnavailableResult(string message = "")
        {
            return CreateResult(StatusCodes.Status503ServiceUnavailable, "SERVICE_UNAVAILABLE", message);
        }

        protected ObjectResult ConnectTimeoutResult(string message = "")
        {
            return CreateResult(StatusCodes.Status504GatewayTimeout, "CONNECT_TIMEOUT", message);
        }

        private ObjectResult CreateResult(int statusCode, string responseCode, string message = "", object result = null)
        {
            if (result==null && message.IsNullOrEmpty())
                return new ObjectResult(new BaseResult { Code = statusCode, Status = responseCode });
            if (result == null)
                return new ObjectResult(new MessageResult { Code = statusCode, Status = responseCode, Message = message });
            if (message.IsNullOrEmpty())
                return new ObjectResult(new OnlyResult { Code = statusCode, Status = responseCode, Result = result });
            else
                return new ObjectResult(new OkResult { Code = statusCode, Status = responseCode, Result = result, Message = message });
        }




    }
}
