using System;
using Microsoft.AspNetCore.Mvc;
using NewRelic.Api.Agent;
using ObservabilityDemo.Api.Contracts.Requests;
using ObservabilityDemo.Api.Models;

namespace ObservabilityDemo.Api.Controllers
{
    [ApiController]
    [Route("/v1/users")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Transaction]
        public IActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            if (!IsValidRequest(request))
            {
                SendErrorToNewRelic(new ArgumentException());

                return UnprocessableEntity("Invalid data");
            }

            var user = new User(request.Name, request.Password, request.Email);

            return Accepted(new
            {
                message = "Request recieved with success",
                created_user = user
            });
        }

        private void SendErrorToNewRelic(Exception ex)
        {
            NewRelic.Api.Agent.NewRelic.NoticeError(ex);
        }

        private bool IsValidRequest(CreateUserRequest request) =>
            !String.IsNullOrWhiteSpace(request.Name)
            && !String.IsNullOrWhiteSpace(request.Password)
            && !String.IsNullOrWhiteSpace(request.Name);
    }
}