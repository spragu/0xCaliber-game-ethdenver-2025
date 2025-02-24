using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;

public class LANServerBrowser : MonoBehaviour
{
    public int listenPort = 47777; // The UDP port to listen on
    private UdpClient udpClient;
    private Thread listenThread;
    private bool listening = false;
    private List<DiscoveredServer> discoveredServers = new List<DiscoveredServer>();
    // A thread-safe queue for incoming messages
    private Queue<string> messageQueue = new Queue<string>();

    void Start()
    {
        StartListening();
    }

    void Update()
    {
        ProcessMessages();
        // Clean up servers that haven't been seen recently
        discoveredServers.RemoveAll(s => Time.time - s.lastSeen > 5f);
    }

    void OnDestroy()
    {
        StopListening();
    }

    public void StartListening()
    {
        udpClient = new UdpClient(listenPort);
        listening = true;
        listenThread = new Thread(ListenLoop);
        listenThread.IsBackground = true;
        listenThread.Start();
    }

    public void StopListening()
    {
        listening = false;
        if (listenThread != null && listenThread.IsAlive)
        {
            listenThread.Join(); // Wait for the thread to finish gracefully
        }
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }

    private void ListenLoop()
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, listenPort);
        while (listening)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEP);
                string message = Encoding.UTF8.GetString(data);
                lock (messageQueue)
                {
                    messageQueue.Enqueue(message);
                }
            }
            catch (System.Exception ex)
            {
                // Log errors; note that this still executes on the background thread
                Debug.Log("Listen error: " + ex.Message);
            }
        }
    }

    // Process incoming messages on the main thread
    private void ProcessMessages()
    {
        while (true)
        {
            string message = null;
            lock (messageQueue)
            {
                if (messageQueue.Count > 0)
                    message = messageQueue.Dequeue();
            }
            if (message == null)
                break;

            // Expected message format: "GameName;IP;Port"
            string[] tokens = message.Split(';');
            if (tokens.Length >= 3)
            {
                string gameName = tokens[0];
                string ip = tokens[1];
                int port;
                if (!int.TryParse(tokens[2], out port))
                    continue;

                bool found = false;
                foreach (var server in discoveredServers)
                {
                    if (server.ip == ip && server.port == port)
                    {
                        server.lastSeen = Time.time;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    discoveredServers.Add(new DiscoveredServer(gameName, ip, port, Time.time));
                    Debug.Log($"Discovered server: {gameName} at {ip}:{port}");
                }
            }
        }
    }

    // Call this method from your UI update loop to get current discovered servers.
    public List<DiscoveredServer> GetDiscoveredServers()
    {
        return new List<DiscoveredServer>(discoveredServers);
    }

}
public class DiscoveredServer
{
    public string gameName; public string ip; public int port; public float lastSeen;
    public DiscoveredServer(string gameName, string ip, int port, float lastSeen)
    {
        this.gameName = gameName;
        this.ip = ip;
        this.port = port;
        this.lastSeen = lastSeen;
    }
}
