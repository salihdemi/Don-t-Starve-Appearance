using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public Animator anim;

    private Rigidbody rb;

    void Start()
    {
        // Rigidbody'yi almayý dene
        rb = GetComponent<Rigidbody>();

        if (anim == null) anim = GetComponent<Animator>();

        // Eðer Rigidbody yoksa konsola bir uyarý yaz ama oyunu çökertme
        if (rb == null)
        {
            Debug.LogWarning(gameObject.name + " üzerinde Rigidbody bulunamadý! Hareket transform üzerinden yapýlacak.");
        }
    }

    void Update()
    {
        Vector2 input = Vector2.zero;
        if (Keyboard.current != null)
        {
            float x = Keyboard.current.dKey.isPressed ? 1 : (Keyboard.current.aKey.isPressed ? -1 : 0);
            float z = Keyboard.current.wKey.isPressed ? 1 : (Keyboard.current.sKey.isPressed ? -1 : 0);
            input = new Vector2(x, z);
        }

        Vector3 move = transform.right * input.x + transform.forward * input.y;

        // --- HAREKET KONTROLÜ ---
        if (rb != null)
        {
            // Rigidbody varsa fizik motoruyla hareket et (Önerilen)
            rb.MovePosition(rb.position + move * walkSpeed * Time.deltaTime);
        }
        else
        {
            // Rigidbody yoksa basit transform hareketi yap (Hata vermez)
            transform.position += move * walkSpeed * Time.deltaTime;
        }

        // Animatör Parametreleri
        if (anim != null)
        {
            anim.SetFloat("X", input.x, 0.1f, Time.deltaTime);
            anim.SetFloat("Z", input.y, 0.1f, Time.deltaTime);
            anim.SetFloat("Speed", input.magnitude);
        }
    }
}