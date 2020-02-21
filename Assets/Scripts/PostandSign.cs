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
public class PostandSign : MonoBehaviour
{
    public static string PostSimbaTransaction(string endpoint,RestRequest request)
    {
        var url = SimbaInfo.Url + endpoint;
        var client = new RestClient(url);
     
        request.AddHeader("APIKEY", SimbaInfo.ApiKey);
        var response = client.Post(request);
        
        var content = response.Content;
        return content;
    }

    public static string SignSimbaTransaction(JObject json)
    {
        if (SimbaInfo.isCol && double.Parse(WalletBalance.GetBalance(SimbaInfo.Wallet.GetAccount(0).Address)) < 1.0)
		{
            WalletBalance.RequestFunds(SimbaInfo.Wallet.GetAccount(0).Address, "1");
        }
        AccountSignerTransactionManager TransactionManager = new AccountSignerTransactionManager(null, SimbaInfo.Wallet.GetAccount(0), null);
        
         
          var transaction = new Nethereum.RPC.Eth.DTOs.TransactionInput();
          transaction.Data = json["data"].ToString();
          transaction.From = json["from"].ToString();
          transaction.Gas =  new Nethereum.Hex.HexTypes.HexBigInteger(json["gas"].ToString()) ;
          transaction.GasPrice = new Nethereum.Hex.HexTypes.HexBigInteger(json["gasPrice"].ToString());
          transaction.Nonce = new Nethereum.Hex.HexTypes.HexBigInteger(json["nonce"].ToString());
            
      
          transaction.To = json["to"].ToString();


        var signed = TransactionManager.SignTransaction(transaction);
       
        Debug.Log(signed);
          Debug.Log(signed.ToString());
          return signed.ToString();
      


    }
}
