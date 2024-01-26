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
        if (justTeleport)
            return;
    }

    public void SwitchScene(){
        PlayerInfo.GetPH().immuneSystem.RecycleBacteria();
        if (sceneName != "MainMenu"){
            ScorePopupPool.spp.RetrieveAllPopups();
            PublicPools.RetrieveAllObjectsToPools();
        }
        if (sceneName == "ArenaV2" | sceneName == "MainMenu"){
            EquipmentManager.UpdateRunDataEquipment();
            if (sceneName =="MainMenu"){
                EquipmentManager.RevertAllEquipment();
            }
            RunDataSave.rData.conditionals = playerStats.conditionals;
            RunDataSave.rData.numericals = playerStats.numericals;
            RunDataSave.rData.shields = playerStats.shields;
            RunDataSave.rData.money = PlayerInfo.GetMoney();
            RunDataSave.UpdateJsonData();
        }
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
