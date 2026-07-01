using UnityEngine;
using UnityEngine.SceneManagement; // <-- Sudah diperbaiki di sini

public class SceneController : MonoBehaviour{
    public static SceneController instance;

    private void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public void NextLevel(){
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName){
        SceneManager.LoadSceneAsync(sceneName);
    }
}