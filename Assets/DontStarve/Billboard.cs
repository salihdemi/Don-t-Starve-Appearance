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

        // Kameranýn rotasyonuna eþitle (Bu bizi arkadan baktýrýyor olabilir)
        transform.rotation = targetCamera.rotation;
    }
}