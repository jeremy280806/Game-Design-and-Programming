using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour{
    
    [Header("UI Pemenang")]
    // Variabel untuk menampung panel Win milikmu
    public GameObject winPanel; 

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            
            // Cek apakah musuh sudah habis
            GameObject[] sisaMusuh = GameObject.FindGameObjectsWithTag("Enemy");
            if(sisaMusuh.Length > 0){
                Debug.Log("SISTEM: Masih ada musuh!");
                return; 
            }

            // 1. Simpan progres level
            UnlockNewLevel();

            // 2. Munculkan UI Win dan Hentikan Waktu
            if(winPanel != null){
                winPanel.SetActive(true); // Memunculkan Pop-up
                Time.timeScale = 0f;      // Menghentikan waktu/pause game
            }
        }
    }
    
    void UnlockNewLevel(){
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex")){
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}