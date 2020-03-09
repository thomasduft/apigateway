using System;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.TimeApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class TimeController : ControllerBase
  {
    [HttpGet]
    public Time Get()
    {
      return new Time { Now = DateTime.Now };
    }
  }

  public class Time
  {
    public DateTime Now { get; set; }
  }
}
