using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Client
{
    static async Task Main()
    {
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        int port = 8888;
        TcpClient client = new TcpClient();

        try
        {
            await client.ConnectAsync(ip, port);
            NetworkStream stream = client.GetStream();
            IPEndPoint serverEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            string serverIP = serverEndPoint.Address.ToString();

            Console.WriteLine("Type 'date' to get the date or 'time' to get the time:");
            string userInput = Console.ReadLine();
            byte[] messageBytes = Encoding.ASCII.GetBytes(userInput);
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);

            byte[] buffer = new byte[1024];
            int receivedLength = await stream.ReadAsync(buffer, 0, buffer.Length);
            string serverResponse = Encoding.ASCII.GetString(buffer, 0, receivedLength);

            Console.WriteLine($"о {DateTime.Now:HH:mm} від [{serverIP}] {serverResponse}");

            client.Close();
        }
        catch (SocketException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
