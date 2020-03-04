using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.OrdersApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class OrdersController : ControllerBase
  {
    [HttpGet]
    public IEnumerable<Order> Get()
    {
      return new List<Order>
      {
        new Order { Id = 1, Article = "T-Shirt", Quantity = 1, Price = 25 },
        new Order { Id = 2, Article = "Jeans", Quantity = 1, Price = 50 },
        new Order { Id = 3, Article = "Socks", Quantity = 3, Price = 15.05m }
      };
    }
  }

  public class Order
  {
    public int Id { get; set; }
    public string Article { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
  }
}
