using UnityEngine;

namespace TPSCameraController
{
    [RequireComponent(typeof(CameraController))]
    public class CameraLock : MonoBehaviour
    {
        [SerializeField] Transform targetToLock;
        CameraController cameraController;

        private void Start()
        {
            cameraController = GetComponent<CameraController>();
        }

        void Update()
        {
            transform.LookAt(targetToLock);
        }

        void SetTarget(Transform newTarget)
        {
            cameraController.AllowCameraMovement = newTarget == null;

            targetToLock = newTarget;
        }

        [ContextMenu("Lock")]
        public void SetNextTarget()
        {
            GameObject target = GameObject.Find("Target");
            Transform newTarget = null;

            if (targetToLock != null)
            {
                if (target.name != targetToLock.name)
                {
                    newTarget = target.transform;
                }
            }
            else
            {
                newTarget = target.transform;
            }
            
            SetTarget(newTarget);
        }
    }
}
