using UnityEngine;
using UnityEngine.InputSystem;

public class DST_DirectionalMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public Animator anim;
    public Transform cameraTransform;

    private Rigidbody rb;
    // Karakterin dünyadaki mutlak bakış yönünü saklar (Başlangıçta ileri baksın)
    private Vector3 worldLookDirection = Vector3.forward; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (anim == null) anim = GetComponentInChildren<Animator>();
        if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Vector2 input = GetInput();
        
        // 1. DÜNYA HAREKETİ
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * input.y + camRight * input.x).normalized;

        if (input.magnitude > 0.1f)
        {
            // Hareket ediyorsak baktığımız yönü güncelle
            worldLookDirection = moveDirection;

            if (rb != null)
                rb.MovePosition(rb.position + moveDirection * walkSpeed * Time.deltaTime);
            else
                transform.position += moveDirection * walkSpeed * Time.deltaTime;
        }

        // 2. HER KAREDE HESAPLA (Hareket etmese bile!)
        // Bu sayede karakter dururken Q-E ile kamera dönerse animasyon değişir.
        UpdateDSTAnimations(input.magnitude);
    }

    void UpdateDSTAnimations(float currentSpeed)
    {
        if (anim == null) return;

        // Kameranın ileri bakış yönü
        Vector3 camDir = cameraTransform.forward;
        camDir.y = 0;
        camDir.Normalize();

        // SignedAngle: Kamera yönü ile karakterin dünya bakış yönü arasındaki açı
        float angle = Vector3.SignedAngle(camDir, worldLookDirection, Vector3.up);

        // Sin/Cos ile Blend Tree parametrelerini bul (-1 ile 1 arası)
        float animX = Mathf.Sin(angle * Mathf.Deg2Rad);
        float animZ = Mathf.Cos(angle * Mathf.Deg2Rad);

        // Değerleri her zaman set ediyoruz
        anim.SetFloat("X", animX);
        anim.SetFloat("Z", animZ);
        anim.SetFloat("Speed", currentSpeed);
    }

    private Vector2 GetInput()
    {
        if (Keyboard.current == null) return Vector2.zero;
        float x = Keyboard.current.dKey.isPressed ? 1 : (Keyboard.current.aKey.isPressed ? -1 : 0);
        float y = Keyboard.current.wKey.isPressed ? 1 : (Keyboard.current.sKey.isPressed ? -1 : 0);
        return new Vector2(x, y).normalized;
    }
}