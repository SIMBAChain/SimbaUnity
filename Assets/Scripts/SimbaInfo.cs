using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nethereum.HdWallet;
public class SimbaInfo : MonoBehaviour
{
    public static string Url = "https://api.simbachain.com/v1/highscore";
    public static string ApiKey = "";
    public static string WalletSeed = "";
    public static Wallet Wallet;
    //bool to check if on circle of life network
    //if isCol is true then the code to check and request funds will be used otherwise it will be ignored
    public static bool isCol = false;
}
