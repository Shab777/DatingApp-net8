using System;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController] //annotation gives extra power to controllers
[Route("api/[controller]")] // EF can direct http request to the appropriate controller & endpoin /api/users
public class BaseApiController:ControllerBase
{

}
