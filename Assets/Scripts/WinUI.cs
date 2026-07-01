using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour{
    // Fungsi untuk tombol MENU
    public void KeLevelMenu(){
        Time.timeScale = 1f; // PENTING: Kembalikan waktu agar game tidak freeze
        PlayerPrefs.SetInt("LangsungKeLevelMenu", 1); // Catatan rahasia untuk MainMenu.cs
        SceneManager.LoadScene("Menu"); // Pindah ke scene Menu
    }

    // Fungsi untuk tombol CONTINUE/NEXT
    public void KeLevelSelanjutnya(){
        Time.timeScale = 1f; // PENTING: Kembalikan waktu agar game tidak freeze
        
        // Pindah ke level selanjutnya berdasarkan nomor antrean Build Settings
        if(SceneController.instance != null){
            SceneController.instance.NextLevel();
        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}