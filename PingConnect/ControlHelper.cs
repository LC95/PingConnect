using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingConnect
{
    internal static class ControlHelper
    {
        public const string SENDER_KEY = "~Aloha___@!";
        public static byte[] SENDER_BYTES = Encoding.UTF8.GetBytes(SENDER_KEY);
       
        public const string RECEIVE_KEY = "~Bonjour___@!";
        public static byte[] RECEIVER_BYTES = Encoding.UTF8.GetBytes(RECEIVE_KEY);
        
        /// <summary>
        /// to judge whether a received msg is a connection request
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsHowl(this byte[] data)
        {
            return SENDER_BYTES.SequenceEqual(data.Skip(28).Take(SENDER_BYTES.Length));
        }

        /// <summary>
        /// to judge whether a received msg is a connection reply
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsEcho(this byte[] data)
        {
            return RECEIVER_BYTES.SequenceEqual(data.Skip(28).Take(RECEIVER_BYTES.Length));
        }
    }
}
