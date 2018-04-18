using UnityEngine;

namespace Kontrol
{
    public class ParallaxBackground : MonoBehaviour
    {
        public bool scrolling, parallax;

        public float backgroundSize;
        public float parallaxSpeed;

        private Transform cameraTransform;
        private Transform[] layers;
        private float viewZone = 10;
        private int leftIndex;
        private int rightIndex;

        private float lastCameraX;

        void Start()
        {
            cameraTransform = Camera.main.transform;
            lastCameraX = cameraTransform.position.x;
            layers = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                layers[i] = transform.GetChild(i);
            }
            leftIndex = 0;
            rightIndex = layers.Length - 1;
        }

        void FixedUpdate()
        {
            if (parallax)
            {
                float deltaX = cameraTransform.position.x - lastCameraX;
                transform.position = new Vector3(transform.position.x + (deltaX * parallaxSpeed), Camera.main.transform.position.y, transform.position.z);
            }
            lastCameraX = cameraTransform.position.x;

            if (scrolling)
            {
                if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
                {
                    ScrollLeft();
                }
                if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
                {
                    ScrollRight();
                }
            }
        }

        private void ScrollLeft()
        {
            int lastRight = rightIndex;
            layers[rightIndex].position = new Vector3(layers[leftIndex].position.x - backgroundSize, Camera.main.transform.position.y, transform.position.z);
            leftIndex = rightIndex;
            rightIndex--;
            if (rightIndex < 0)
            {
                rightIndex = layers.Length - 1;
            }
        }

        private void ScrollRight()
        {
            int lastLeft = leftIndex;
            layers[leftIndex].position = new Vector3(layers[rightIndex].position.x + backgroundSize, Camera.main.transform.position.y, transform.position.z);
            rightIndex = leftIndex;
            leftIndex++;
            if (leftIndex == layers.Length)
            {
                leftIndex = 0;
            }
        }
    }

}