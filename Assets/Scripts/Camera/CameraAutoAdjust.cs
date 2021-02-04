using UnityEngine;

namespace TPSCameraController.Adjust
{
    public class CameraAutoAdjust : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] Transform cameraPos = null;
        [SerializeField] Transform followTransform = null;
        [SerializeField] Transform center = null;

        [Header("Offset")]
        [Min(0)] [SerializeField] float cameraOffset = 1;

        [Header("Configuration")]
        [Min(0)] [SerializeField] float springArmDistance = 4;
        [Min(0)] [SerializeField] float castRadius = 0.5f;
        [Min(1)] [SerializeField] float zoomInLerpSpeed = 100;
        [Min(1)] [SerializeField] float zoomOutLerpSpeed = 100;

        [Header("Layers To Affect")]
        [SerializeField] LayerMask layerMask = 0;

        bool cameraHittedCollider;
        RaycastHit hitInfo;

        Vector3 velocity;

        public float SpringArmDistance { get => springArmDistance - cameraOffset; }

        void Start()
        {
            cameraPos.GetChild(0).localPosition = new Vector3(0, 0, cameraOffset);
            transform.parent = null;
        }

        void Update()
        {
            CheckCollision();
        }

        /// <summary>
        /// Check the colision with other colliders and adjust the camera position
        /// </summary>
        void CheckCollision()
        {
            if (Physics.SphereCast(center.position, castRadius, (transform.position - center.position).normalized, out hitInfo, SpringArmDistance, layerMask))
            {
                cameraHittedCollider = true;

                float distance = Vector3.Distance(center.position, hitInfo.point);
                Vector3 dir = (cameraPos.position - center.position).normalized;
                Vector3 pos = center.position + (dir.normalized * distance);

                cameraPos.position = Vector3.Lerp(cameraPos.position, pos, zoomInLerpSpeed * Time.deltaTime);

                return;
            }

            if (cameraHittedCollider)
            {
                cameraPos.localPosition = Vector3.Lerp(cameraPos.localPosition, Vector3.zero, zoomOutLerpSpeed * Time.deltaTime);
                if (cameraPos.localPosition == Vector3.zero)
                {
                    cameraHittedCollider = false;
                }
                return;
            }

            cameraPos.localPosition = Vector3.zero;
        }

        private void LateUpdate()
        {
            transform.rotation = followTransform.rotation;
            //follow target
            Vector3 pos = followTransform.TransformPoint(new Vector3(0, 0, followTransform.localPosition.z - SpringArmDistance));

            transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, 0);
        }

        private void OnDrawGizmos()
        {
            if (cameraHittedCollider)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            if (followTransform != null)
            {
                Gizmos.DrawLine(transform.position, followTransform.position);

                Gizmos.color = Color.green;
                Gizmos.DrawSphere(transform.position, castRadius);

                Gizmos.DrawLine(followTransform.position, hitInfo.point);

                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(transform.position, (followTransform.position - transform.position).normalized);

                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(hitInfo.point, 0.3f);
            }
        }
    }
}

