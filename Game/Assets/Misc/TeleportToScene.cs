using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TeleportToScene : MonoBehaviour
{
    [SerializeField] private bool justTeleport;
    [SerializeField] private string sceneName;
    [SerializeField] private playerLook playerlook;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool platformsState;

    private void Start()
    {
        if (justTeleport)
            return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        if (!justTeleport){
            collision.gameObject.transform.position = spawnPoint.position;
            playerlook.AlterLookRotation(spawnPoint.rotation);
        }
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        PlatformGenerator.PG.ActiveState(platformsState);
        if (rb != null)
            rb.velocity = Vector3.zero;
        PublicPools.RetrieveAllObjectsToPools();
        gameObject.GetComponent<Collider>().enabled = false;
        SceneManager.LoadScene(sceneName);
    }

    public void Teleport()
    {
        SceneManager.LoadScene(sceneName);
    }
}
