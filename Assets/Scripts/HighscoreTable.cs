using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Linq;
using Nethereum.Hex.HexConvertors;
using Nethereum.Hex.HexTypes;
using NBitcoin;
public class HighscoreTable : MonoBehaviour
{
    public Text Count;
    public Text TopTen;
    public Text Balance;
    public Text PublicKey;
    public Text PrivateKey;
    public Text Seed;
    public Text Url;
    public InputField SeedInput;
    public InputField ScoreInput;
    public static bool getTransactions = false;
    public bool TriggerGet;
    public bool TriggerPost;
    public int PostScore;

    // Start is called before the first frame update
    void Start()
    {
        Get();
      
       
       
        Url.text = "URL: " + SimbaInfo.Url;
        RandomWallet();

    }


 
    void Update()
    {

        if (getTransactions || TriggerGet)
        {
            Get();
            getTransactions = false;
            TriggerGet = false;
        }
        if (TriggerPost)
        {
            PostScore = int.Parse(ScoreInput.text);
            TriggerPost = false;
            Post();
          
        }
    }
   

    void Get()
    {
        var content = GetTransactions.GetSimbaTransactions("/highscore/");
        JObject json = JObject.Parse(content);
        JArray results = JArray.Parse(json["results"].ToString());
  

        Count.text = "Count: " + json["count"].ToString();
        string topTen = "Last Ten";
        var i = 0;
        Dictionary<string, List<int>> scoreDict = new Dictionary<string, List<int>>();
    
        foreach (JObject transaction in results)
        {
           
            
      
            
                
            if (i < 10)
            {
                var from = transaction["payload"]["raw"]["from"].ToString();
                var score = transaction["payload"]["inputs"]["score"].ToString();
                topTen += "\n" + from.ToString() + " : " + score.ToString();
                i++;
            }

        }
      
        TopTen.text = topTen;
        Debug.Log(results);
    }

    void Post()
    {
        var request = new RestRequest("");
        

        request.AddParameter("from", SimbaInfo.Wallet.GetAccount(0).Address.ToString());
        request.AddParameter("score", PostScore);
        var content = PostandSign.PostSimbaTransaction("/highscore/", request);
        JObject json = JObject.Parse(content);
        JObject raw = JObject.Parse(json["payload"]["raw"].ToString());
        Debug.Log(content);
        Sign(json,raw);

    }
    string Sign(JObject json, JObject raw)
    {
        var i = 0;
        while (i < 5)
        {
            var payload = PostandSign.SignSimbaTransaction(raw);

            string txnId = json["id"].ToString();
            Debug.Log("TXN ID BELOW");
            Debug.Log(txnId);

            var signRequest = new RestRequest("");
            signRequest.AddParameter("payload", "0x" + payload);

            var submitsigned = PostandSign.PostSimbaTransaction("/transaction/" + txnId + "/", signRequest);
            var submitJson = JObject.Parse(submitsigned);
            Debug.Log(submitsigned);

            
            try
            {
                var errors = submitJson["errors"];
                var error = errors[0];
                var code = error["code"].ToString();
                if (code == "15001")
                {
                    var newNonce = error["meta"]["suggested_nonce"].ToObject<int>();
                
                    raw["nonce"] = newNonce.ToString("X");

                    Debug.Log(newNonce);
                }
            }
            catch
            {
                TriggerGet = true;
                return submitsigned;

            }
            
            i++;
        }
        return "";
    }

    //Button Handling
    public void GetClicked()
    {
        TriggerGet = true;
    }

    public void PostClicked()
    {
       
        TriggerPost = true;
    }

    public void RandomWallet()
    {
        GenWallet.RandomWallet();
        Balance.text = WalletBalance.GetBalance(SimbaInfo.Wallet.GetAccount(0).Address) + " ether";
        PublicKey.text = "Public Key: " + SimbaInfo.Wallet.GetAccount(0).Address;
        PrivateKey.text = "Private Key:" + SimbaInfo.Wallet.GetAccount(0).PrivateKey;
        Seed.text = "Seed: " + SimbaInfo.WalletSeed;
    }

    public void WalletFromSeed()
    {
        GenWallet.WalletFromSeed(SeedInput.text);
        Balance.text = WalletBalance.GetBalance(SimbaInfo.Wallet.GetAccount(0).Address) + " ether";
        PublicKey.text = "Public Key: " + SimbaInfo.Wallet.GetAccount(0).Address;
        PrivateKey.text = "Private Key:" + SimbaInfo.Wallet.GetAccount(0).PrivateKey;
        Seed.text = "Seed: " + SimbaInfo.WalletSeed;
    }

    public void SaveWallet()
	{
        WalletStorage.SaveWallet();
	}

    public void LoadWallet()
	{
        WalletStorage.LoadWallet();
        Balance.text = WalletBalance.GetBalance(SimbaInfo.Wallet.GetAccount(0).Address) + " ether";
        PublicKey.text = "Public Key: " + SimbaInfo.Wallet.GetAccount(0).Address;
        PrivateKey.text = "Private Key:" + SimbaInfo.Wallet.GetAccount(0).PrivateKey;
        Seed.text = "Seed: " + SimbaInfo.WalletSeed;
    }

}
