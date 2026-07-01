using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour{

    public Button[] buttons;
    public GameObject levelButtons; // WAJIB ditambahkan agar fungsi ButtonsToArray tidak error

    private void Awake(){
        ButtonsToArray();
        // Mengambil data level yang sudah terbuka, default-nya level 1
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        
        // Mengunci semua tombol level terlebih dahulu
        for(int i = 0; i < buttons.Length; i++){
            buttons[i].interactable = false;
        }
        
        // Membuka tombol hanya untuk level yang sudah terbuka
        for(int i = 0; i < unlockedLevel; i++){
            buttons[i].interactable = true;
        }
    }

    public void LoadLevel(int index){
        SceneManager.LoadScene(index);
    }

   void ButtonsToArray(){
    // Tambahkan pengecekan null di sini agar tidak error
        if(levelButtons != null){
            int childCount = levelButtons.transform.childCount;
            buttons = new Button[childCount];
            for(int i = 0; i < childCount; i++){
                buttons[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
            }
        } else {
            Debug.LogError("Variabel Level Buttons belum diisi di Inspector LevelManager!");
        }
    }
}