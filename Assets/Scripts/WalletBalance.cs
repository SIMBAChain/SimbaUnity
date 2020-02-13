using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using RestSharp;
using Newtonsoft.Json.Linq;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.HdWallet;
using Nethereum.Signer;
using Nethereum.KeyStore;
using Nethereum.Web3.Accounts;
public class WalletBalance : MonoBehaviour
{
   public static string GetBalance(string account)
	{
        JObject json = JObject.Parse(GetTransactions.GetSimbaTransactions("/balance/" + account));
        Debug.Log(json);
        var balance = json["amount"].ToObject<Double>() / 1000000000000000000.0;
        return balance.ToString();
    }
       
    public static string RequestFunds(string account, string amount)
	{
        var request = new RestRequest("");


        request.AddParameter("currency", "ether");
        request.AddParameter("value", amount);
        //   var content = PostandSign.PostSimbaTransaction(("/balance/" + account).ToString(), request);

        var url = SimbaInfo.Url + "/balance/" + account + "/";
        var client = new RestClient(url);

        request.AddHeader("APIKEY", SimbaInfo.ApiKey);
        var response = client.Post(request);

        var content = response.Content;
        Debug.Log(request);
        Debug.Log(response.StatusCode);
        Debug.Log(content);
        return content;
	}
}
