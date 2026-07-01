using UnityEngine;

public class GameManager : MonoBehaviour{
    public static GameManager instance;
    public GameObject gameOverBackground;
      
      void Start(){
        gameOverBackground.transform.localPosition = new Vector3(0f, -1500f, 0f);

        if(instance == null){
            instance = this;
        }
        else{
            Destroy(this);
        }
    }

    public void TriggerGameOverBackground(){
        gameOverBackground.LeanMoveLocalY(0f, .8f).setEaseOutBack();
    } 
}