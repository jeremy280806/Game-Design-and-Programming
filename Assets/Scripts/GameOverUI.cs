using UnityEngine;
using UnityEngine.SceneManagement;

// Nama class sekarang sudah sama dengan nama file (GameOverUI)
public class GameOverUI : MonoBehaviour {
    
    public void RetryLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu() {
        // Ini adalah perintah wajib untuk pindah ke scene Menu
        SceneManager.LoadScene("Menu"); 
    }
}