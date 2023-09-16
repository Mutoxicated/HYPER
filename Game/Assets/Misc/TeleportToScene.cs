using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = Vector3.zero;
        PublicPools.RetrieveAllObjectsToPools();
        gameObject.GetComponent<Collider>().enabled = false;
        SceneManager.LoadScene(sceneName);
    }
}
