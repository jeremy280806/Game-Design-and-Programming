using UnityEngine;

public class InstaKillTrap : MonoBehaviour{
    private void OnTriggerEnter2D(Collider2D collision){
        // Cek apakah yang menabrak memiliki Tag "Player"
        if (collision.CompareTag("Player")){
            Player playerScript = collision.GetComponent<Player>();
            
            if (playerScript != null){
                // Memberikan damage maksimum secara paksa (langsung mati)
                playerScript.TakeDamage(999);
            }
        }
    }
}