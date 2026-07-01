using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panel UI")]
    public GameObject mainMenuPanel;
    public GameObject levelMenuPanel;

    private void Start()
    {
        // Membaca catatan rahasia dari FinishPoint
        if(PlayerPrefs.GetInt("LangsungKeLevelMenu", 0) == 1)
        {
            // Jika ada catatan, langsung sembunyikan menu utama & buka level menu
            if(mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if(levelMenuPanel != null) levelMenuPanel.SetActive(true);

            // Penting: Hapus catatan agar saat game baru dibuka (restart), tidak langsung masuk ke level menu
            PlayerPrefs.SetInt("LangsungKeLevelMenu", 0);
        }
    }

    public void PlayGame()
    {
        // Fungsi untuk tombol Play di layar awal (membuka menu level)
        if(mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if(levelMenuPanel != null) levelMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("SISTEM: Game ditutup!");
        Application.Quit();
    }
}