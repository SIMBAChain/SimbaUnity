using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RestSharp;


public class GetTransactions : MonoBehaviour
{
  
    public static string GetSimbaTransactions(string endpoint)
    {
        var url = SimbaInfo.Url + endpoint;
        var client = new RestClient(url);
        var request = new RestRequest("");
        request.AddHeader("APIKEY", SimbaInfo.ApiKey);
        var response = client.Get(request);
        var content = response.Content;
        return content;
    }

}
