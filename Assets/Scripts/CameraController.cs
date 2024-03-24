using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ������ ī�޶�
    [SerializeField]
    private Transform cameraTransform;

    // �ٶ󺸴� Ÿ��
    [SerializeField]
    private Transform followTarget;

    // �ӵ�, �ΰ���, �ִ� ī�޶� X ȸ����, �ڿ������� ����
    [SerializeField]
    private float followSpeed;
    [SerializeField]
    private float sensitivity;
    [SerializeField]
    private float cameraRotationXClamp;
    [SerializeField]
    private float smoothness;

    // ī�޶�� Ÿ���� �ּ� �Ÿ��� �ִ� �Ÿ�, ���� �Ÿ�
    [SerializeField]
    private float minDistance;
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private float currentDistance;

    private float rotationX;
    private float rotationY;

    private Vector3 directionNormalized;
    private Vector3 currentDirection;

    private void Awake()
    {
        // ȸ���� �ʱ�ȭ
        rotationX = transform.localRotation.eulerAngles.x;
        rotationY = transform.localRotation.eulerAngles.y;

        // �Ÿ��� �ʱ�ȭ
        directionNormalized = cameraTransform.localPosition.normalized;
        currentDistance = cameraTransform.localPosition.magnitude;

        // Ŀ�� ������ �����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            CameraRotationControl();
            SmoothFollow();

            yield return null;
        }
    }

    private void CameraRotationControl()
    {
        // �Է� ���� ȸ������ ����
        rotationX += -Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -cameraRotationXClamp, cameraRotationXClamp);

        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);

        transform.rotation = rotation;
    }

    private void SmoothFollow()
    {
        transform.position = Vector3.MoveTowards(transform.position, followTarget.position, followSpeed * Time.deltaTime);

        currentDirection = transform.TransformPoint(directionNormalized * maxDistance);

        RaycastHit hit;

        if (Physics.Linecast(transform.position, currentDirection, out hit))
        {
            currentDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            currentDistance = maxDistance;
        }

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, directionNormalized * currentDistance, smoothness * Time.deltaTime);
    }
}
