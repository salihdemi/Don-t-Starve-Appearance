using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraRotator : MonoBehaviour
{
    public CinemachineCamera virtualCamera;
    public float rotationSpeed = 10f;
    public float distance = 0.5f; // Görseldeki deðerinle eþitledim

    private int currentAngleIndex = 0;
    private Vector3 targetOffset;
    private CinemachineFollow followComponent;

    private Vector2[] offsetPoints = new Vector2[]
    {
        new Vector2(0, -1), new Vector2(1, -1), new Vector2(1, 0), new Vector2(1, 1),
        new Vector2(0, 1), new Vector2(-1, 1), new Vector2(-1, 0), new Vector2(-1, -1)
    };

    void Start()
    {
        if (virtualCamera != null)
        {
            followComponent = virtualCamera.GetComponent<CinemachineFollow>();
            // Baþlangýçta mevcut Y'yi al ve hedefi kur
            float initialY = followComponent.FollowOffset.y;
            UpdateTargetOffset(initialY);
            followComponent.FollowOffset = targetOffset;
        }
    }

    void Update()
    {
        // Sadece tuþa basýldýðýnda yeni hedef belirlenir
        if (Keyboard.current.eKey.wasPressedThisFrame) Rotate(1);
        if (Keyboard.current.qKey.wasPressedThisFrame) Rotate(-1);

        if (followComponent != null)
        {
            // Sadece offseti yumuþatýyoruz, rotasyona müdahale etmiyoruz
            followComponent.FollowOffset = Vector3.Lerp(
                followComponent.FollowOffset,
                targetOffset,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    void Rotate(int dir)
    {
        currentAngleIndex = (currentAngleIndex + dir + 8) % 8;
        UpdateTargetOffset(followComponent.FollowOffset.y);
    }

    void UpdateTargetOffset(float currentY)
    {
        Vector2 dir = offsetPoints[currentAngleIndex].normalized;
        targetOffset = new Vector3(dir.x * distance, currentY, dir.y * distance);
    }
}