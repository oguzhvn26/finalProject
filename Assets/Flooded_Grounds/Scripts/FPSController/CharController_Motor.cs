using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Motor : MonoBehaviour {

    [SerializeField] private List<AudioClip> footSteps = new List<AudioClip>();
    [SerializeField] private List<AudioClip> runFootSteps = new List<AudioClip>();
    private AudioSource audioSource; 
    private float footstepTimer;

    // Yürüyüş ve koşma hızı artık Inspector'dan ayarlanabilir
    [SerializeField] private float walkSpeed = 8.0f;  // Yürüyüş hızı
    [SerializeField] private float runSpeed = 16.0f;  // Koşma hızı
    public float acceleration = 5.0f; // Hızlanma oranı
    public float sensitivity = 30.0f;
    public float jumpForce = 18f; 
    public float WaterHeight = 15.5f;
    CharacterController character;
    public GameObject cam;
    float moveFB, moveLR;
    float rotX, rotY;
    public bool webGLRightClickRotation = true;
    float gravity = -19.6f; 
    float verticalVelocity = 0f;
    bool isGrounded = false;

    private bool isRunning;

    void Start() {
        character = GetComponent<CharacterController>();
        if (Application.isEditor) {
            webGLRightClickRotation = false;
            sensitivity = sensitivity * 1.5f;
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void DoFootSteps() {
        // Hareket kontrolü
        if (isGrounded && (moveFB != 0 || moveLR != 0)) {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0) {
                if (!isRunning) {
                    audioSource.PlayOneShot(footSteps[Random.Range(0, footSteps.Count)]);
                    footstepTimer = 1f;
                } else {
                    audioSource.PlayOneShot(runFootSteps[Random.Range(0, runFootSteps.Count)]);
                    footstepTimer = 0.4f;
                }
            }
        }
    }

    void CheckForWaterHeight() {
        if (transform.position.y < WaterHeight) {
            gravity = 0f;
        } else {
            gravity = -19.6f;
        }
    }

    void Update() {
        // Shift tuşuna basılı tutulduğunda hızlanma
        if (Input.GetKey(KeyCode.LeftShift)) {
            isRunning = true; // Shift'e basıldığında koşma durumu aktif
        } else {
            isRunning = false; // Shift'e basılmadığında sadece yürüyüş
        }

        // Koşma veya yürüyüş hızını belirle
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        moveFB = Input.GetAxis("Vertical") * currentSpeed;
        moveLR = Input.GetAxis("Horizontal") * currentSpeed;

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity;

        CheckForWaterHeight();

        isGrounded = character.isGrounded;
        if (isGrounded && verticalVelocity < 0) {
            verticalVelocity = -2f; // Hafif bir negatif değerle yere bağlı kalmasını sağlıyoruz
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            verticalVelocity = jumpForce; // Zıplama kuvveti uygulanıyor
        }

        verticalVelocity += gravity * Time.deltaTime; // Yerçekimi uygulanıyor

        Vector3 movement = new Vector3(moveLR, verticalVelocity, moveFB);

        if (webGLRightClickRotation) {
            if (Input.GetKey(KeyCode.Mouse0)) {
                CameraRotation(cam, rotX, rotY);
            }
        } else {
            CameraRotation(cam, rotX, rotY);
        }

        movement = transform.rotation * movement;
        character.Move(movement * Time.deltaTime);

        // Ayak sesi kontrolü
        DoFootSteps();
    }

    void CameraRotation(GameObject cam, float rotX, float rotY) {
        transform.Rotate(0, rotX * Time.deltaTime, 0);
        cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);
    }
}
