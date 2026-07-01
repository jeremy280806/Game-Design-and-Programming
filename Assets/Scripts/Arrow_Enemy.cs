using UnityEngine;

public class Arrow_Enemy : MonoBehaviour{
    public float arrowSpeed = 5f;
    public int damage = 1; // Jumlah damage yang diberikan ke Player

    private void Start() {
        // Hancurkan panah otomatis setelah 5 detik agar tidak membuat game lag
        Destroy(this.gameObject, 5f);
    }

    // Fungsi ini akan aktif otomatis saat panah menyentuh objek lain
    private void OnTriggerEnter2D(Collider2D other) {
        // Mengecek apakah objek yang ditabrak memiliki komponen/script "Player"
        Player playerTarget = other.GetComponent<Player>();
        
        // Jika yang ditabrak adalah Player
        if (playerTarget != null) {
            // Berikan damage ke Player
            playerTarget.TakeDamage(damage);
            
            // Hancurkan anak panah setelah berhasil menusuk Player
            Destroy(this.gameObject);
        }
        // (Opsional) Jika panah menabrak tanah/Ground, panah juga hancur
        else if (other.gameObject.CompareTag("Ground")) {
            Destroy(this.gameObject);
        }
    }
}