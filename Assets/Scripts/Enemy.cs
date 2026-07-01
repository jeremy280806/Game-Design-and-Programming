using UnityEngine;

public class Enemy : MonoBehaviour{
    public int maxHealth = 5;
    public float walkSpeed = 1.5f;
    public float chaseSpeed = 3f;
    
    [Header("Fisika & Komponen")]
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private Animator animator;

    [Header("Deteksi Tanah & Patroli")]
    public Transform groundCheckPoint;
    public float distance = .3f;
    public LayerMask whatIsGround;
    private bool facingLeft = true;
    public GameObject floatingTextPrefab; 
    public Transform textSpawnPoint;      

    [Header("Logika Pengejaran & Serangan")]
    public float attackRangeRadius = 6f;
    public float retrieveDistance = 2.5f; 
    public LayerMask whatIsPlayer;
    public Transform player;
    public Transform attackPoint;
    public float attackRangeRadiusVisual = 1f;

    [Header("Pengaturan Audio SFX")]
    public AudioSource audioSource;
    public AudioClip deathSound;

    void Start(){
        facingLeft = true;  
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        animator = GetComponent<Animator>();
        if (animator == null) {
            animator = GetComponentInChildren<Animator>();
        }
    }

    void Update(){
        if(player == null){
            if (animator != null) animator.SetBool("Attack", false);
        }
        if(maxHealth <= 0){
            Die();
            return; 
        }

        Collider2D collinfo = Physics2D.OverlapCircle(transform.position, attackRangeRadius, whatIsPlayer);

        if(collinfo && player != null){
            if(player.position.x > transform.position.x && facingLeft){
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else if(player.position.x < transform.position.x && !facingLeft){
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;  
            }

            float distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);

            if(distanceToPlayer > retrieveDistance){
                if(animator != null) animator.SetBool("Attack", false);
                
                float moveDir = facingLeft ? -1f : 1f;
                rb.linearVelocity = new Vector2(moveDir * chaseSpeed, rb.linearVelocity.y);
            }
            else{
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                if(animator != null) animator.SetBool("Attack", true); 
            }
        }
        else{
            if(animator != null) animator.SetBool("Attack", false);
            
            float patrolDir = facingLeft ? -1f : 1f;
            rb.linearVelocity = new Vector2(patrolDir * walkSpeed, rb.linearVelocity.y);

            if(groundCheckPoint != null){
                RaycastHit2D hitinfo = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, distance, whatIsGround);

                if(hitinfo == false){
                    if(facingLeft){
                        transform.eulerAngles = new Vector3(0f, -180f, 0f);
                        facingLeft = false;
                    }
                    else{
                        transform.eulerAngles = new Vector3(0f, 0f, 0f);
                        facingLeft = true;
                    }
                }
            }
        }
    }

    public void Attack(){
        if (attackPoint == null) return;

        Collider2D collinfo = Physics2D.OverlapCircle(attackPoint.position, attackRangeRadiusVisual, whatIsPlayer);

        if(collinfo){
           if(collinfo.gameObject.GetComponent<Player>() != null){
                if (Player.instance != null) {
                    Player.instance.TakeDamage(1);
                }
           }
        }
    }

    public void TakeDamage(int damageAmount){
        if(maxHealth <= 0) return;

        maxHealth -= damageAmount;
        if(animator != null) animator.SetTrigger("Hurt");

        if (floatingTextPrefab != null && textSpawnPoint != null) {
            Instantiate(floatingTextPrefab, textSpawnPoint.position, Quaternion.identity);
        }

        if (CameraShake.instance != null) {
            CameraShake.instance.Shake(2f, .12f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Arrow")){
            TakeDamage(1);
            Destroy(other.gameObject);
        }
        // 2. TAMBAHAN: Logika untuk mati jika kena air
        if(other.gameObject.CompareTag("Water")){
            Die(); // Memanggil fungsi Die() yang sudah ada di bawah
        }
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

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;   
        Gizmos.DrawWireSphere(transform.position, attackRangeRadius);
        
        if(groundCheckPoint != null){
            Gizmos.color = Color.yellow;
            // SUDAH FIX: Hanya menggunakan 2 argumen sesuai aturan Unity
            Gizmos.DrawRay(groundCheckPoint.position, Vector2.down * distance);
        }

        if (attackPoint != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRangeRadiusVisual);
        }
    }
}