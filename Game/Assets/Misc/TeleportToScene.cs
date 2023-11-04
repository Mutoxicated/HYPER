using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TeleportToScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public delegate void Ev();
    public List<Ev> onTeleport = new List<Ev>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        foreach (Ev method in onTeleport){
            method.Invoke();
        }
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
