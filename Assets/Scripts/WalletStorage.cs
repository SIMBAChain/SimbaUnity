using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletStorage : MonoBehaviour
{
   public static void SaveWallet()
	{
        Debug.Log("Saving");
        Debug.Log(SimbaInfo.WalletSeed);
        PlayerPrefs.SetString("wallet", SimbaInfo.WalletSeed);
            PlayerPrefs.Save();
        
    }

    public static void LoadWallet()
    {
        Debug.Log("Loading");
        if (PlayerPrefs.HasKey("wallet"))
        {
            string seed = PlayerPrefs.GetString("wallet");
            Debug.Log(seed);
            GenWallet.WalletFromSeed(seed);
        }
    }
}
