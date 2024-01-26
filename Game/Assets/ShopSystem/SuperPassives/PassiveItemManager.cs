using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public class PassiveItemManager : MonoBehaviour
{

    [SerializeField] private List<PassiveItemInfo> passiveItemInfos = new List<PassiveItemInfo>();
    [SerializeField] private List<PassiveItemPresenter> pips = new List<PassiveItemPresenter>();

    private List<PassiveItemInfo> randomPIIs = new List<PassiveItemInfo>();

    private void ShufflePassiceItems(IList<PassiveItemInfo> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            PassiveItemInfo value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void PresentPassives(){
        for (int i = 0; i < pips.Count-1; i++){
            pips[i].SetImageSprite(randomPIIs[i].itemImage);
            pips[i].SetTitle(randomPIIs[i].itemName);
        }
    }

    private void Start(){
        randomPIIs = passiveItemInfos;
        ShufflePassiceItems(randomPIIs);
        PresentPassives();
    }
}
