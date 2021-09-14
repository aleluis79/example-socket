using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace server
{
    class Program
    {

        static WatsonWsServer server = new WatsonWsServer("localhost", 9000, false);

        static async Task Main(string[] args)
        {
            server.ClientConnected += ClientConnectedAsync;
            server.ClientDisconnected += ClientDisconnected;
            server.MessageReceived += MessageReceived; 
            await server.StartAsync();
            
            Console.WriteLine("Server is listening: " + server.IsListening);

            while (true) {} // Para que no finalice la apliación

            static void ClientConnectedAsync(object sender, ClientConnectedEventArgs args) 
            {
                Console.WriteLine("Client connected: " + args.IpPort);
                
                // Le respondo al cliente que se conecto al servidor
                string data = JsonSerializer.Serialize(new { mensaje = "Conectado al servidor" });
                server.SendAsync(args.IpPort, data);
            }

            static void ClientDisconnected(object sender, ClientDisconnectedEventArgs args) 
            {
                Console.WriteLine("Client disconnected: " + args.IpPort);
            }

            static void MessageReceived(object sender, MessageReceivedEventArgs args) 
            { 
                // Proceso el mensaje recibido
                string mensaje = Encoding.UTF8.GetString(args.Data);
                Console.WriteLine("Message received from " + args.IpPort + ": " + mensaje);

                // Respondo al cliente lo mismo que recibí
                string data = JsonSerializer.Serialize(new { mensaje = "Recibi esto: " + mensaje});
                server.SendAsync(args.IpPort, data);
            }


        }
    }
}
