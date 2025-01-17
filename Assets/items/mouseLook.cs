using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    public Transform playerBody; // Karakterin gövdesini tutacak
    public float lookSpeedX = 2f; // X eksenindeki bakış hızı
    public float lookSpeedY = 2f; // Y eksenindeki bakış hızı
    private float rotationX = 0f; // Y eksenindeki bakış açısı

    void Update()
    {
        // Fare hareketini al
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;

        // Y eksenindeki bakış açısını güncelle
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Kamera rotasını uygula
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Karakterin bakış yönünü uygula
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
