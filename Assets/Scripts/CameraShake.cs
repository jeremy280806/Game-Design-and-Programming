using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour{
    public static CameraShake instance; 
    public CinemachineCamera cam;
    private float shakeTime;

    void Start(){
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(this.gameObject);
        }
    }

    void Update(){
        if(shakeTime > 0f){
            shakeTime -= Time.deltaTime;

            if(shakeTime <= 0f){
                // PERBAIKAN: Kembali menggunakan GetComponent standar Unity 6 (Cinemachine v3) dengan null check
                CinemachineBasicMultiChannelPerlin perlin = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
                if (perlin != null) {
                    perlin.AmplitudeGain = 0f;
                }
            }
        }
    }

    public void Shake(float intensity, float duration){
        if (cam == null) return;

        shakeTime = duration;
        // PERBAIKAN: Kembali menggunakan GetComponent standar Unity 6 (Cinemachine v3) dengan null check
        CinemachineBasicMultiChannelPerlin perlin = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (perlin != null) {
            perlin.AmplitudeGain = intensity;
        }
    }
}