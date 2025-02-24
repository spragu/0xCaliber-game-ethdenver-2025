using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class LANServerAdvertiser : MonoBehaviour
{
    public int broadcastPort = 47777;           // The UDP port to use for broadcasting
    public float broadcastInterval = 1f;          // Broadcast every 1 second
    public string gameName = "MyFishNetGame";     // Customize this to your game’s name
    public int gamePort = 7777;                   // The port your FishNet server is running on

    private UdpClient udpClient;
    private IPEndPoint broadcastEndPoint;
    private bool broadcasting = false;
    private Thread broadcastThread;

    private void Start()
    {
        StartBroadcast();
    }

    private void OnDestroy()
    {
        StopBroadcast();
    }

    public void StartBroadcast()
    {
        udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;
        broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, broadcastPort);
        broadcasting = true;
        broadcastThread = new Thread(BroadcastLoop);
        broadcastThread.IsBackground = true;
        broadcastThread.Start();
    }

    public void StopBroadcast()
    {
        broadcasting = false;
        if (broadcastThread != null && broadcastThread.IsAlive)
        {
            broadcastThread.Abort();
        }
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }

    private void BroadcastLoop()
    {
        while (broadcasting)
        {
            try
            {
                string localIP = GetLocalIPAddress();
                // Message format: "GameName;IP;Port"
                string message = $"{gameName};{localIP};{gamePort}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                udpClient.Send(data, data.Length, broadcastEndPoint);
            }
            catch (System.Exception ex)
            {
                Debug.Log("Broadcast error: " + ex.Message);
            }
            Thread.Sleep((int)(broadcastInterval * 1000));
        }
    }

    private string GetLocalIPAddress()
    {
        // Simple method to get the local IP address; may need enhancements for multi-homed systems.
        string localIP = "127.0.0.1";
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}
