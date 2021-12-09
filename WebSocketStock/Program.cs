using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using WebSocketStock.Sockets;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();



var webSocketOptions = new WebSocketOptions()
{
    KeepAliveInterval = TimeSpan.FromSeconds(120),
};

app.UseWebSockets(webSocketOptions);

StockSocket _socket = new StockSocket();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/socket")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
            {
                await _socket.PriceWriter(context, webSocket);
            }
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
    else
    {
        await next();
    }

});
app.UseAuthorization();

app.MapControllers();
app.UseWebSockets();
app.Run();

