using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class degen_mode_checker : MonoBehaviour
{
 private string canisterId = "be2us-64aaa-aaaaa-qaabq-cai"; // Replace with your actual canister ID
    private string icNetwork = "127.0.0.1:4943"; // Replace with local network if testing locally

    IEnumerator GetDegenMode()
    {
        string url = $"http://127.0.0.1:4943/api/v2/canister/be2us-64aaa-aaaaa-qaabq-cai/query";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("isDegenMode: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }

    void Start()
    {
        StartCoroutine(GetDegenMode());
    }
}
