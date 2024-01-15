using UnityEngine;

public class LookTo : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private float lerpSpeed;
    private Quaternion lookRotation;
    private Vector3 toEntity = Vector3.zero;

    private void OnEnable(){
        stats.FindEntity();
        //Debug.Log("enabled on: "+gameObject.name);
        GetRotation();
        if (stats.entity != null)
            transform.rotation = lookRotation;
    }

    private void GetRotation(){
        if (stats.entity == null){
            stats.DecideObjective();
            stats.FindEntity();
        }
        if (stats.entity == null){
            return;
        }
        toEntity = stats.entity.position - transform.position;
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
        if (stats.entity == null){
            return;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * lerpSpeed);
    }
}
