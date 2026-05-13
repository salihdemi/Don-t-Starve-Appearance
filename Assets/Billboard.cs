using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform targetCamera;

    void Start()
    {
        if (targetCamera == null && Camera.main != null)
            targetCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (targetCamera == null) return;

        // 1. Adým: Kameranýn mevcut rotasyonunu al
        Quaternion camRotation = targetCamera.rotation;

        // 2. Adým: Sadece Y ekseninde 180 derece döndürülmüþ yeni bir rotasyon oluþtur
        // Bu iþlem karakterin 'forward' yönünü düzeltir ama koordinatlarý senkronize eder
        Quaternion correctedRotation = camRotation * Quaternion.Euler(0, 180, 0);

        // 3. Adým: Rotasyonu uygula
        transform.rotation = correctedRotation;

        // Not: Eðer hala ýþýk sorunu (siyahlýk) varsa, Material'dan "Render Face: Both" 
        // seçeneðinin açýk olduðundan emin ol.
    }
}