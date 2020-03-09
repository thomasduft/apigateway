using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApiGateway.TimeApi
{
  public class TimePusherService : BackgroundService
  {
    private readonly ILogger<TimePusherService> logger;
    private readonly IHubContext<TimeHub> hub;

    public TimePusherService(
      ILogger<TimePusherService> logger,
      IHubContext<TimeHub> hub
    )
    {
      this.logger = logger;
      this.hub = hub;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {

        var time = new Time { Now = DateTime.Now };

        this.logger.LogInformation($"Sending time {time.Now}");

        await this.hub.Clients.All.SendAsync("time", time);

        await Task.Delay(1000, stoppingToken);
      }
    }
  }
}