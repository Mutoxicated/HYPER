using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnReload : MonoBehaviour
{
    private static string sceneNotAllowedOn = "MainMenu";
    private static Dictionary<string, DestroyOnReload> dict = new Dictionary<string, DestroyOnReload>();

    private void CheckScene(Scene scene, LoadSceneMode lsm){
        if (sceneNotAllowedOn == scene.name){
            dict = new Dictionary<string, DestroyOnReload>();
            Destroy(gameObject);
        }
    }
    private void Awake() {
        SceneManager.sceneLoaded += CheckScene;
        if (dict.ContainsKey(gameObject.name) && dict[gameObject.name] != this) {
            Destroy(gameObject);
        }
        else {
            if (dict.ContainsKey(gameObject.name))
                dict[gameObject.name] = this;
            else dict.Add(gameObject.name,this);
        }
    }

    private void OnDestroy(){
        SceneManager.sceneLoaded -= CheckScene;
    }
}
