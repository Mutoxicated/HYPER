using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TeleportToScene : MonoBehaviour
{
    [SerializeField] private bool justTeleport;
    [SerializeField] private string sceneName;
    [SerializeField] private Stats playerStats;
    [SerializeField] private playerLook playerlook;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool platformsState;

    private Stats.conditionalDict conds = new Stats.conditionalDict();
    private Stats.numericalDict nums = new Stats.numericalDict();

    private Shield[] shields;

    private void Start()
    {
        if (justTeleport){
            return;
        }
        Application.quitting += UpdateRunDataOnQuit;
    }

    private void OnDestroy(){
        if (justTeleport){
            return;
        }
        Application.quitting -= UpdateRunDataOnQuit;
    }

    private void UpdateMisc(){
        Pickup.ResetMods();
        bullet.ResetBulletEffectiveness();
        SuperPassivePool.Clear();
        PassivePool.ResetEffectiveness();
        PlatformObjective.ResetShieldChance();
        ItemShop.ResetCheapness();
        ItemPool.ResetClassItems();
        ClassSystem.Reset();
        Difficulty.rounds = 0;
    }

    public void SwitchScene(){
        PlayerInfo.SetScore(0);
        PlayerInfo.GetPH().immuneSystem.RecycleBacteria();
        if (sceneName != "MainMenu"){
            ScorePopupPool.spp.RetrieveAllPopups();
            PublicPools.RetrieveAllObjectsToPools();
        }
        PIC.PICVier.GetPIM().SetRandomState(false);
        if (SceneManager.GetActiveScene().name == "ArenaV2" && sceneName == "MainMenu"){
            UpdateMisc();
            return;
        }
        if (SceneManager.GetActiveScene().name == "ArenaV2" && sceneName == "Interoid"){
            MoneyBonus.SetMoneyBonusGot(false);
        }
        EquipmentManager.UpdateRunDataEquipment();
        PIC.PICVier.GetPIM().UpdateFilledSlots(sceneName);
        SuperPassivePool.UpdateRunData();
        if (sceneName =="MainMenu"){
            PIC.PICVier.GetPIM().SetRandomState(false);
            EquipmentManager.RevertAllEquipment();
            UpdateMisc();
        }
        RunDataSave.rData.conditionals = playerStats.conditionals;
        RunDataSave.rData.numericals = playerStats.numericals;
        RunDataSave.rData.shields = playerStats.shields;
        RunDataSave.rData.money = PlayerInfo.GetMoney();
        RunDataSave.rData.moneyBonus = MoneyBonus.GetMoneyBonus();
        PassivePool.UpdateRunDataPassives();
        PIC.LockAllCurrentSlots(sceneName);
        if (sceneName != "Interoid"){
            ItemShop.UpdateCurrentItems();
            PIC.SetCurrentPIIS();
            PIC.UpdateRunDatas();
        }
        RunDataSave.UpdateJsonData();
    }

    private void UpdateRunDataOnQuit(){
        if (SceneManager.GetActiveScene().name == "gas station" || SceneManager.GetActiveScene().name == "ArenaV2") return;
        EquipmentManager.UpdateRunDataEquipment();
        EquipmentManager.RevertAllEquipment();
        RunDataSave.rData.conditionals = playerStats.conditionals;
        RunDataSave.rData.numericals = playerStats.numericals;
        RunDataSave.rData.shields = playerStats.shields;
        RunDataSave.rData.money = PlayerInfo.GetMoney();
        SuperPassivePool.UpdateRunData();
        PassivePool.UpdateRunDataPassives();
        PIC.PICVier.GetPIM().UpdateFilledSlots(sceneName);
        PIC.LockAllCurrentSlots(sceneName);
        ItemShop.UpdateCurrentItems();
        PIC.SetCurrentPIIS();
        PIC.UpdateRunDatas();
        RunDataSave.UpdateJsonData();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        if (!justTeleport){
            collision.gameObject.transform.position = spawnPoint.position;
            playerlook.AlterLookRotation(spawnPoint.rotation);
            SwitchScene();
        }
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        PlatformGenerator.PG.ActiveState(sceneName,platformsState);
        if (rb != null)
            rb.velocity = Vector3.zero;
        gameObject.GetComponent<Collider>().enabled = false;
        SceneManager.LoadScene(sceneName);
    }

    public void Teleport()
    {
        SceneManager.LoadScene(sceneName);
    }
}
