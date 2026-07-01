using UnityEngine;

public class TrapDamage : MonoBehaviour{
    [Header("Pengaturan Perangkap")]
    public int damageAmount = 1; 

    private void OnTriggerEnter2D(Collider2D collision){
        // Mengecek apakah objek yang masuk ke dalam area api memiliki Tag "Player"
        if (collision.CompareTag("Player")){
            // Mengambil komponen Player dan memanggil fungsi TakeDamage milikmu
            Player playerScript = collision.GetComponent<Player>();
            
            if (playerScript != null){
                playerScript.TakeDamage(damageAmount);
            }
        }
    }
}