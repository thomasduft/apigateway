using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.CatalogApi.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class CatalogsController : ControllerBase
  {
    [HttpGet]
    public IEnumerable<CatalogItem> Get()
    {
      return new List<CatalogItem>
      {
        new CatalogItem { Id = 1, Name = "T-Shirt" },
        new CatalogItem { Id = 2, Name = "Jeans" },
        new CatalogItem { Id = 3, Name = "Socks" }
      };
    }

    public class CatalogItem
    {
      public int Id { get; set; }
      public string Name { get; set; }
    }
  }
}
