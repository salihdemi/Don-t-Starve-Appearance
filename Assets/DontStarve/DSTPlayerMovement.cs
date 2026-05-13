using UnityEngine;
using UnityEngine.InputSystem;

public class DST_PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public Animator anim;
    public Transform cameraTransform; // Inspector'dan Main Camera'yý sürükle

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (anim == null) anim = GetComponentInChildren<Animator>();
        if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Vector2 input = GetInput();

        // 1. ADIM: Kameranýn bakýþ açýsýna göre hareket yönlerini hesapla
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Y eksenini sýfýrlýyoruz (Karakterin yere gömülmemesi veya uçmamasý için)
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // 2. ADIM: Hedef hareket yönünü oluþtur
        Vector3 moveDirection = (camForward * input.y + camRight * input.x).normalized;

        // 3. ADIM: Fiziksel Hareket
        if (rb != null)
        {
            rb.MovePosition(rb.position + moveDirection * walkSpeed * Time.deltaTime);
        }
        else
        {
            transform.position += moveDirection * walkSpeed * Time.deltaTime;
        }

        // 4. ADIM: Animasyon için Lokal Yön Hesaplama (Ön-Arka-Yan)
        if (anim != null)
        {
            UpdateAnimations(input, moveDirection);
        }
    }

    private Vector2 GetInput()
    {
        if (Keyboard.current == null) return Vector2.zero;

        float x = Keyboard.current.dKey.isPressed ? 1 : (Keyboard.current.aKey.isPressed ? -1 : 0);
        float z = Keyboard.current.wKey.isPressed ? 1 : (Keyboard.current.sKey.isPressed ? -1 : 0);
        return new Vector2(x, z).normalized;
    }

    private void UpdateAnimations(Vector2 input, Vector3 moveDir)
    {
        // Karakter hareket etmiyorsa hýzý sýfýrla ama son bakýþ yönünü koru
        anim.SetFloat("Speed", input.magnitude);

        if (input.magnitude > 0.1f)
        {
            /* 
               DST mantýðýnda animatör parametreleri genellikle þöyledir:
               X: -1 (Sol), 1 (Sað)
               Z: -1 (Ön/Aþaðý), 1 (Arka/Yukarý)
            */
            anim.SetFloat("X", input.x, 0.05f, Time.deltaTime);
            anim.SetFloat("Z", input.y, 0.05f, Time.deltaTime);
        }
    }
}