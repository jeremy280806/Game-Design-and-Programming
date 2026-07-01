using UnityEngine;

public class Floating_Text : MonoBehaviour{
    public TextMesh textMesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
       int randomNumber = Random.Range(1, 101);
       textMesh.text = randomNumber.ToString();    
       Destroy(this.gameObject, 1.01f);    
    }

    // Update is called once per frame
    void Update(){
        
    }
}
