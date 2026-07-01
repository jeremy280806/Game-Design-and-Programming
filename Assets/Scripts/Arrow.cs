using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed = 5f;
    // Update is called once per frame

    private void Start() {
        Destroy(this.gameObject, 5f);
    }
    
}
