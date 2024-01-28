using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Linq;

public class PassiveItemManager : MonoBehaviour
{

    [SerializeField] private List<PassiveItemInfo> passiveItemInfos = new List<PassiveItemInfo>();
    [SerializeField] private List<PassiveItemPresenter> pips = new List<PassiveItemPresenter>();

    private List<PassiveItemInfo> TEMPpassiveItemInfos;

    public List<PassiveItemInfo> GetPassiveInfos(){
        return passiveItemInfos;
    }

    private void PresentPassives(){
        TEMPpassiveItemInfos = passiveItemInfos.ToList();
        for (int i = 0; i < pips.Count; i++){
            int num = SeedGenerator.random.Next(0,TEMPpassiveItemInfos.Count);
            pips[i].SetImageSprite(TEMPpassiveItemInfos[num].itemImage);
            pips[i].SetTitle(TEMPpassiveItemInfos[num].itemName);
            pips[i].SetCurrentPassive(TEMPpassiveItemInfos[num].item);
            TEMPpassiveItemInfos.Remove(TEMPpassiveItemInfos[num]);
        }
    }

    private void Start(){
        PresentPassives();
    }
}
