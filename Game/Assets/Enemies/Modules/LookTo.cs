using UnityEngine;

public class LookTo : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private float lerpSpeed;
    private Quaternion lookRotation = Quaternion.identity;
    private Vector3 toEntity = Vector3.zero;

    private void Awake(){
        stats.FindEntity();
        //Debug.Log("enabled on: "+gameObject.name);
        GetRotation();
        if (stats.entities[0] != null)
            transform.rotation = lookRotation;
    }

    private void GetRotation(){
        if (stats.entities[0] == null){
            stats.FindEntity();
        }
        if (stats.entities[0] == null){
            return;
        }
        toEntity = stats.entities[0].position - transform.position;
        if (toEntity == Vector3.zero) {
            lookRotation = Quaternion.identity;
            return;
        }
        lookRotation = Quaternion.LookRotation(toEntity,Vector3.up);
    }

    public void ResetLocalRotation(){
        transform.localRotation = Quaternion.identity;
        lookRotation = Quaternion.identity;
    }

    public void ChangeLerpSpeed(float speed){
        lerpSpeed = speed;
    }

    private void Update()
    {
        GetRotation();
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * lerpSpeed);
    }
}
