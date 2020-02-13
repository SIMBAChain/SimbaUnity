using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NBitcoin;
using Nethereum.HdWallet;
using UnityEngine.UI;


public class GenWallet : MonoBehaviour
{
   
    public static void RandomWallet()
	{
        Mnemonic mnemo = new Mnemonic(Wordlist.English);
        string mnemoString = "";
        var i = 0;
        while (i < 12)
        {
            if (i != 11)
            {
                mnemoString += mnemo.Words[i] + " ";
            }
            else
            {
                mnemoString += mnemo.Words[i];
            }
            i++;
        }
        string Password = "";
        SimbaInfo.Wallet = new Wallet(mnemoString, Password);

        string myWordList = "";
        foreach (string word in SimbaInfo.Wallet.Words)
        {
            myWordList += word + " ";
        }

        SimbaInfo.WalletSeed = myWordList;
    }

    public static void WalletFromSeed(string seed)
    {
        string Password = "";
        SimbaInfo.Wallet = new Wallet(seed, Password);

        string myWordList = "";
        foreach (string word in SimbaInfo.Wallet.Words)
        {
            myWordList += word + " ";
        }

        SimbaInfo.WalletSeed = myWordList;
    }

}
