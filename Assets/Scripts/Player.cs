using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera mainCamera;

    // �ȴ� �ӵ��� �ٴ� �ӵ�, ���� �ӵ�
    [SerializeField]
    private float walkingSpeed;
    [SerializeField]
    private float runningSpeed;
    private float currentSpeed;

    // �ڿ������� ī�޶� ������ ����
    [SerializeField]
    private float smoothnes;

    // ���� ����
    private bool toggleCameraRotation;
    private bool isRunning;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private CharacterController characterController;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            CameraRotationControl();
            Move();

            yield return null;
        }
    }

    private void CameraRotationControl()
    {
        toggleCameraRotation = Input.GetKey(KeyCode.LeftAlt);

        if (!toggleCameraRotation)
        {
            Vector3 playerRotate = Vector3.Scale(mainCamera.transform.forward, Vector3.right + Vector3.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), smoothnes * Time.deltaTime);
        }
    }

    private void Move()
    {
        isRunning = Input.GetKey(KeyCode.LeftControl);

        currentSpeed = isRunning ? runningSpeed : walkingSpeed;

        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 direction = right * Input.GetAxisRaw("Horizontal") + forward * Input.GetAxisRaw("Vertical");

        float percent = (isRunning ? 1f : 0.5f) * direction.magnitude;

        animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
        characterController.Move(direction.normalized * currentSpeed * Time.deltaTime);
    }
}
