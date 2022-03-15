// See https://aka.ms/new-console-template for more information
using PingConnect;
using System.Net.NetworkInformation;
using System.Text;

//begin to connect peer client | wait some client to connect
PeerListener peerListener = new(args.FirstOrDefault() ?? "");

//message sender
Ping pingSender = new();
while (true)
{
    var msgToSend = Console.ReadLine() ?? "";
    if (!string.IsNullOrEmpty(peerListener.PeerAddr))
    {
        pingSender.Send(peerListener.PeerAddr, 120, Encoding.UTF8.GetBytes(msgToSend));
    }
    else
    {
        Console.WriteLine("No connected peer client!");
    }
}

