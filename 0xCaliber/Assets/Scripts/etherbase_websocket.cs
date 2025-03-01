using UnityEngine;
using Best.WebSockets;
using System;


public class etherbase_websocket : MonoBehaviour
{
    static WebSocket webSocket;
    static string testnetPkey = "0xb353832c074d0910d841a1bc5663f5d07f54987a01879d4328093ec94f551f04";

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    static string wsUri;

   static string jsonString = @"{
        ""type"": ""emit_event"",
        ""data"": {
            ""contractAddress"": ""0x0823C50058A8649637909CE1DceC871d59F323b5"",
            ""name"": ""playerShootEvent"",
            ""args"": {
                ""userId"": ""sprak"",
                ""userAddress"": ""0xcE0318e2848219605590366De9417b0712c25DE1"",
                ""bulletId"": ""sprak-12346"",
                ""timestamp"": 17000000001
            }
        }
    }";


    private void OnWebSocketOpen(WebSocket webSocket)
    {
        Debug.Log("WebSocket is now Open!");
        webSocket.Send(jsonString);
        Debug.Log("WebSocket tried to send the execution test");
    }

    public static void WebShoot()
    {
        webSocket.Send(jsonString);
    }

    void Start()
    {
        // testnetPkey = Environment.GetEnvironmentVariable("testnet_pkey");
        wsUri = $"wss://etherbase-writer-496683047294.europe-west2.run.app/write?privateKey={testnetPkey}";
        Debug.Log("wsUri  is : " + wsUri);
        webSocket = new(new Uri(wsUri));
        //
        webSocket.OnOpen += OnWebSocketOpen;
        Debug.Log("Opening the sock");
        webSocket.Open();
    }


    private void OnWebSocketClosed(WebSocket webSocket, WebSocketStatusCodes code, string message)
    {
        Debug.Log("WebSocket is now Closed!");

        if (code == WebSocketStatusCodes.NormalClosure)
        {
            // Closed by request
        }
        else
        {
            // Error
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
