using UnityEngine;

public class Enemy_3 : MonoBehaviour {
    [Header("Status & Pergerakan")]
    public int maxHealth = 3;
    public float walkSpeed = 1.5f;
    public float chaseSpeed = 3f;

    [Header("Fisika & Komponen")]
    private Rigidbody2D rb;
    private Animator animator;
    private bool facingLeft = true;

    [Header("Patroli & Jarak")]
    public Transform groundCheckPoint;
    public float distance = 0.3f;
    public LayerMask whatIsGround;
    public float attackRangeRadius = 7f;
    public float retrieveDistance = 4f; // Jarak musuh berhenti untuk membidik
    public LayerMask whatIsPlayer;
    public Transform player;

    [Header("Senjata (Panah)")]
    public Transform firePoint;       // Titik keluar panah
    public GameObject arrowPrefab;    // Objek panah yang akan ditembakkan
    public float arrowSpeed = 6f;     // Kecepatan melesat panah

    [Header("Pengaturan Audio SFX")]
    public AudioSource audioSource;
    public AudioClip deathSound;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update() {
        if (maxHealth <= 0) {
            Die();
            return;
        }

        Collider2D collInfo = Physics2D.OverlapCircle(transform.position, attackRangeRadius, whatIsPlayer);

        if (collInfo && player != null) {
            // Logika balik badan menghadap Player
            if (player.position.x > transform.position.x && facingLeft) {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            } else if (player.position.x < transform.position.x && !facingLeft) {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }

            // Hitung jarak antara musuh dan Player
            float distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);

            if (distanceToPlayer > retrieveDistance) {
                if (animator != null) animator.SetBool("Attack", false);
                float moveDir = facingLeft ? -1f : 1f;
                rb.linearVelocity = new Vector2(moveDir * chaseSpeed, rb.linearVelocity.y);
            } else {
                // Berhenti di jarak aman dan mainkan animasi memanah
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                if (animator != null) animator.SetBool("Attack", true);
            }
        } else {
            if (animator != null) animator.SetBool("Attack", false);
            float patrolDir = facingLeft ? -1f : 1f;
            rb.linearVelocity = new Vector2(patrolDir * walkSpeed, rb.linearVelocity.y);

            if (groundCheckPoint != null) {
                RaycastHit2D hitInfo = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, distance, whatIsGround);
                if (!hitInfo) {
                    facingLeft = !facingLeft;
                    transform.eulerAngles = facingLeft ? new Vector3(0f, 0f, 0f) : new Vector3(0f, -180f, 0f);
                }
            }
        }
    }

    // FUNGSI UTAMA: Memunculkan anak panah (ROTASI DIPERBAIKI)
    public void Attack() {
        if (arrowPrefab != null && firePoint != null) {
            GameObject tempArrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D arrowRb = tempArrow.GetComponent<Rigidbody2D>();
            
            if (arrowRb != null) {
                // 1. Tentukan arah gerak panah secara pasti (kiri = -1, kanan = 1)
                float direction = facingLeft ? -1f : 1f;
                
                // 2. Tembakkan lurus ke arah horizontal (Sumbu X)
                arrowRb.linearVelocity = new Vector2(direction * arrowSpeed, 0f);
                
                // 3. PERBAIKAN ROTASI GAMBAR
                if (facingLeft) {
                    // Karena gambar asli panahmu sudah menghadap kiri, biarkan rotasinya 0
                    tempArrow.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                } else {
                    // Saat musuh menghadap kanan, baru kita putar panahnya 180 derajat
                    tempArrow.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                }
            }
        }
    }

    // TAMBAHKAN FUNGSI INI UNTUK MENERIMA DAMAGE DARI PLAYER
    private void OnTriggerEnter2D(Collider2D other) {
        // Mengecek apakah objek yang menabrak memiliki Tag "Arrow" (Panah Player)
        if (other.gameObject.CompareTag("Arrow")) {
            TakeDamage(1); // Kurangi darah musuh
            
            // Hancurkan panah Player agar tidak tembus ke belakang
            Destroy(other.gameObject); 
        }
        // 2. TAMBAHAN: Logika untuk mati jika kena air
        if(other.gameObject.CompareTag("Water")){
            Die(); // Memanggil fungsi Die() yang sudah ada di bawah
        }
    }

    public void TakeDamage(int damageAmount) {
        if (maxHealth <= 0) return;
        maxHealth -= damageAmount;
        if (animator != null) animator.SetTrigger("Hurt");
    }

    void Die() {
        Debug.Log(this.gameObject.name + " Died!");
        if (animator != null) animator.SetBool("Death", true);
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        
        if (GetComponent<Collider2D>() != null) {
            GetComponent<Collider2D>().enabled = false;
        }

        // <-- TAMBAHKAN KODE INI UNTUK MEMUTAR SUARA MATI -->
        if (audioSource != null && deathSound != null) {
            audioSource.PlayOneShot(deathSound);
        }
        
        this.enabled = false;
        Destroy(this.gameObject, 1f); 
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRangeRadius);
        if (groundCheckPoint != null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(groundCheckPoint.position, Vector2.down * distance);
        }
    }
}