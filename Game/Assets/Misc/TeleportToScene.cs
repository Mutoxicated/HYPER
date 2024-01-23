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

    private void UpdatePlayerStatsTransfer(){
        conds = PlayerInfo.GetConditionals();
        nums = PlayerInfo.GetNumericals();

        var pconds = playerStats.GetConditionals();
        var pnums = playerStats.GetNumericals();

        foreach (string key in pconds.Keys){
            conds[key] = pconds[key];
        }

        foreach (string key in pnums.Keys){
            nums[key] = pnums[key];
        }

        PlayerInfo.SetShields(playerStats.shields.ToArray());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        if (!justTeleport){
            PlayerInfo.GetPH().immuneSystem.RecycleBacteria();
            ScorePopupPool.spp.RetrieveAllPopups();
            PublicPools.RetrieveAllObjectsToPools();
            collision.gameObject.transform.position = spawnPoint.position;
            playerlook.AlterLookRotation(spawnPoint.rotation);
            UpdatePlayerStatsTransfer();
        }
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        PlatformGenerator.PG.ActiveState(platformsState);
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
