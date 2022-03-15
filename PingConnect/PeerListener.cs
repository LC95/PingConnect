using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PingConnect
{
    internal class PeerListener
    {
        readonly Ping pingSender = new();
        public string PeerAddr { get; set; } = "";
        private static void Clear(byte[] buffer)
        {
            Array.Clear(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// CTOR
        /// a listener with functions that sending control reply to peer client
        /// and it can output the message to console when received from peer client
        /// </summary>
        /// <param name="peerAddr"></param>
        public PeerListener(string peerAddr = "")
        {
            //Listen all send to local ip
            BeginListen();
            Task.Delay(200).Wait();
            if (peerAddr != "")
            {
                pingSender.Send(peerAddr, 120, ControlHelper.SENDER_BYTES);
            }
            else
            {
                Console.WriteLine("Wait For Connect!");
            }

        }

        private Task BeginListen()
        {
            return Task.Run(() =>
            {
                Socket icmpListener = new(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
                icmpListener.Bind(new IPEndPoint(IPAddress.Parse("192.168.10.128"), 0));
                icmpListener.IOControl(IOControlCode.ReceiveAll, new byte[] { 1, 0, 0, 0 }, null);

                byte[] buffer = new byte[1024 * 1024];
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                while (true)
                {
                    int bytesRead = icmpListener.ReceiveFrom(buffer, ref remoteEndPoint);
                    var remoteAddr = ((IPEndPoint)remoteEndPoint).Address;
                    if (buffer.IsEcho())
                    {
                        PeerAddr = remoteAddr.ToString();
                        Console.WriteLine("Connected to {0}!", remoteAddr);
                        Clear(buffer);
                    }
                    else if (buffer.IsHowl())
                    {
                        PeerAddr = remoteAddr.ToString();
                        Console.WriteLine("Got a new caller: {0}!", remoteAddr);

                        pingSender.Send(remoteAddr, 120, ControlHelper.RECEIVER_BYTES);
                        Clear(buffer);
                    }
                    else
                    {
                        var content = buffer.Skip(28).TakeWhile(x => x != '\0').ToArray();
                        var contentStr = Encoding.UTF8.GetString(content);
                        Console.WriteLine(contentStr);
                        Clear(buffer);
                    }
                }
            });

        }
    }
}
