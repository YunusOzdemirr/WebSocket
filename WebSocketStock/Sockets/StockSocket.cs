using System;
using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using WebSocketStock.Entities;

namespace WebSocketStock.Sockets
{
    public class StockSocket
    {
        public async Task PriceWriter(HttpContext context, WebSocket wSocket)
        {
            string[] stdcodelist = { "USDTRY", "EURTRY", "EURUSD", "XU100", "XU30", "BRENT", "XGLD", "GLD" };
            ArraySegment<byte> buffer = new ArraySegment<byte>();
            while (true)
            {
                if (wSocket.State == WebSocketState.Open)
                {
                    Random random = new Random();
                    Stock stock = new Stock()
                    {
                        Symbol = stdcodelist[random.Next(0, 7)],
                        Price = random.Next(100, 500)
                    };

                    var jsonViop = JsonConvert.SerializeObject(stock);
                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonViop));
                    await wSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                    Task.Delay(500).Wait();
                }
            }
        }
    }
}

