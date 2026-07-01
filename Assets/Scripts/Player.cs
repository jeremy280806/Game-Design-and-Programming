using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour{   
    public static Player instance;

    private Animator animator;
    public Rigidbody2D rb;
    public int maxHealth = 5; 
    public float jumpHeight = 7f;
    public float moveSpeed = 5f;
    private float movement; 
    private bool isGround; 
    private bool facingRight;

    public Transform groundCheckPoint;
    public float groundCheckRadius = .2f;
    public LayerMask whatIsGround;

    public GameObject arrowPrefab;
    public Transform spawnPosition;
    public float arrowSpeed = 7f;
    public GameObject explosionPrefab;
    public Transform explosionSpawnPoint;

    private int currentDiamonds;
    public GameObject collect_EffectPrefab;
    public Text currentHeart_Text;
    public Text currentDiamond_Text;

    [Header("Pengaturan Audio SFX")]
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip hitSound;
    public AudioClip runSound; // <-- DITAMBAHKAN: Untuk suara jalan
    public AudioClip collectSound; // <-- DITAMBAHKAN: Untuk suara collectible (Heart & Diamond)
    public AudioClip explosionSound;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start(){
        isGround = true;
        facingRight = true;
        animator = this.gameObject.GetComponent<Animator>();
        currentDiamonds = 0; 
    }

    void Update(){
       movement = Input.GetAxis("Horizontal");

       if(Input.GetKeyDown(KeyCode.Space)){
            Jump();
       }
       
       Collider2D collInfo = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);

       if(collInfo){
            isGround = true;
       }

       Flip();
       PlayRunAnimation();   

       if(Input.GetMouseButtonDown(0)){
            animator.SetTrigger("Fire");
       }
    }

    private void FixedUpdate(){
        transform.position += new Vector3(movement * moveSpeed, 0f, 0f) * Time.fixedDeltaTime;
    }

    public void FireArrow(){
       GameObject tempArrowPrefab =  Instantiate(arrowPrefab, spawnPosition.position, spawnPosition.rotation);
       tempArrowPrefab.GetComponent<Rigidbody2D>().linearVelocity = spawnPosition.right * arrowSpeed;

       // <-- DITAMBAHKAN: Bunyikan suara panah/menyerang
       if (audioSource != null && attackSound != null) {
           audioSource.PlayOneShot(attackSound);
       }
    }

    void PlayRunAnimation(){
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        if (stateInfo.IsName("Fire")) {
            return; 
        }

        if(Mathf.Abs(movement) > 0.05f){
            animator.SetFloat("Run", 1f);   
        }
        else {
            animator.SetFloat("Run", 0f);
        }
    } 

    void Flip(){
        if(movement < 0f && facingRight == true){
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;    
        }
        else if(movement > 0f && facingRight == false){
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true; 
        }
    }

    void Jump() {
        if(isGround == true) {
            Vector2 velocity = rb.linearVelocity;
            velocity.y = jumpHeight;
            rb.linearVelocity = velocity;
            isGround = false;
            animator.SetBool("Jump", true);

            // <-- DITAMBAHKAN: Bunyikan suara melompat
            if (audioSource != null && jumpSound != null) {
                audioSource.PlayOneShot(jumpSound);
            }
        }   
    }

    public void TakeDamage(int damageAmount){
        if(maxHealth <= 0){
            return;
        }
        
        maxHealth -= damageAmount;

        // Kode ini memastikan teks di UI selalu sama dengan nilai maxHealth yang baru
        if (currentHeart_Text != null) {
            currentHeart_Text.text = maxHealth.ToString();
        }

        if (maxHealth <= 0) {
            Die();
            return;
        }

        animator.SetTrigger("Hurt");

        if (audioSource != null && hitSound != null) {
            audioSource.PlayOneShot(hitSound);
        }
           
        if (CameraShake.instance != null) {
            CameraShake.instance.Shake(2f, 0.12f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ground")) {
            animator.SetBool("Jump", false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D coll){
        if(coll.gameObject.tag == "Heart"){
            maxHealth++;
            currentHeart_Text.text = maxHealth.ToString();
            
            if (audioSource != null && collectSound != null) {
                audioSource.PlayOneShot(collectSound);
            }

            GameObject tempCollect_Effect = Instantiate(collect_EffectPrefab, coll.gameObject.transform.position, Quaternion.identity);
            Destroy(tempCollect_Effect, .401f);
            Destroy(coll.gameObject);
        }

        if(coll.gameObject.CompareTag("Arrow_Enemy")){
            TakeDamage(1);
            Destroy(coll.gameObject);
        }
        
        if(coll.gameObject.CompareTag("Diamond")){
            currentDiamonds++;
            currentDiamond_Text.text = currentDiamonds.ToString();
            Debug.Log("Diamond terkumpul: " + currentDiamonds);
            
            if (audioSource != null && collectSound != null) {
                audioSource.PlayOneShot(collectSound);
            }

            GameObject tempCollect_Effect = Instantiate(collect_EffectPrefab, coll.gameObject.transform.position, Quaternion.identity);
            Destroy(tempCollect_Effect, .401f);
            Destroy(coll.gameObject);
        }

        // <-- TAMBAHKAN KODE INI UNTUK DETEKSI AIR -->
        if(coll.gameObject.CompareTag("Water")){
            // Mengurangi nyawa sesuai jumlah maxHealth saat ini 
            // agar angka Heart di UI menjadi 0, lalu memicu fungsi Die()
            TakeDamage(maxHealth); 
        }
    }

    private void OnDrawGizmosSelected() {
        if(groundCheckPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }

    void Die(){
        Debug.Log(this.gameObject.name + " Died!");
        
        if (CameraShake.instance != null) {
            CameraShake.instance.Shake(4f, .18f);
        }
        
        if (GameManager.instance != null) {
            GameManager.instance.TriggerGameOverBackground();
        }
        
        if (explosionPrefab != null && explosionSpawnPoint != null) {
            GameObject tempExplosion = Instantiate(explosionPrefab, explosionSpawnPoint.position, explosionSpawnPoint.rotation);
            Destroy(tempExplosion, .901f);
        }

        // <-- 2. TAMBAHKAN KODE INI UNTUK MEMUTAR SUARA LEDAKAN -->
        if (explosionSound != null) {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }
        
        Destroy(this.gameObject);
    }

    // Fungsi khusus untuk dipanggil dari Animation Event (Suara Lari)
    public void PlayStepSound() {
        if (audioSource != null && runSound != null) {
            audioSource.PlayOneShot(runSound);
        }
    }
}