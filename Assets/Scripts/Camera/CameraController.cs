using UnityEngine;

namespace TPSCameraController
{
    public class CameraController : MonoBehaviour
    {
        [Header("Configurations")]
        [SerializeField] Vector2 xAxisLimits;
        [SerializeField] float radiusOffset = 0;

        [Header("Sensibility")]
        [SerializeField] float horizontalMouseSensibility = 2;
        [SerializeField] float verticalMouseSensibility = 2;

        public bool AllowCameraMovement { get; set; } = true;

        Vector2 cameraMove;
        Vector2 rotation;

        Transform radius;

        #region Input
        public void ChangeCameraShoulder()
        {
            transform.localPosition = new Vector3(-1 * radiusOffset, 0, 0);
        }
        public void Look(Vector2 _cameraMove)
        {
            cameraMove = _cameraMove.normalized;
        }
        #endregion

        private void Start()
        {
            radius = transform.GetChild(0);
            radius.localPosition = new Vector3(radiusOffset, 0, 0);
            rotation.y = transform.eulerAngles.y;
        }

        private void Update()
        {
           //rotation.y += Input.GetAxis("Mouse X") * horizontalMouseSensibility;
           //rotation.x += -Input.GetAxis("Mouse Y") * verticalMouseSensibility;
           //rotation.x = Mathf.Clamp(rotation.x, -xAxisLimits.x, xAxisLimits.y);
           //transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0);

            if (AllowCameraMovement)
            {
                rotation.x += cameraMove.x * horizontalMouseSensibility * 100 * Time.deltaTime;
                rotation.y -= cameraMove.y * verticalMouseSensibility * 100 * Time.deltaTime;

                rotation.y = Mathf.Clamp(rotation.y, -xAxisLimits.x, xAxisLimits.y);
            
                transform.localRotation = Quaternion.Euler(rotation.y, rotation.x, 0);
            }
        }

        public void ChangeOffset(float lerp)
        {
            radius.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(radiusOffset, 0, 0), lerp);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Mathf.Abs(radiusOffset));
        }
    }
}
