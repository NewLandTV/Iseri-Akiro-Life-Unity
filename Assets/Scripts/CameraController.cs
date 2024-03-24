using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 움직일 카메라
    [SerializeField]
    private Transform cameraTransform;

    // 바라보는 타겟
    [SerializeField]
    private Transform followTarget;

    // 속도, 민감도, 최대 카메라 X 회전값, 자연스러운 정도
    [SerializeField]
    private float followSpeed;
    [SerializeField]
    private float sensitivity;
    [SerializeField]
    private float cameraRotationXClamp;
    [SerializeField]
    private float smoothness;

    // 카메라와 타겟의 최소 거리와 최대 거리, 현재 거리
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
        // 회전값 초기화
        rotationX = transform.localRotation.eulerAngles.x;
        rotationY = transform.localRotation.eulerAngles.y;

        // 거리값 초기화
        directionNormalized = cameraTransform.localPosition.normalized;
        currentDistance = cameraTransform.localPosition.magnitude;

        // 커서 고정과 숨기기
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
        // 입력 받은 회전값을 적용
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
