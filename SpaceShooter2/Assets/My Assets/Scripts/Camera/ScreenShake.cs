using UnityEngine;

namespace Kontrol
{
    public class ScreenShake : MonoBehaviour
    {
        Vector3 originPosition;
        Quaternion originRotation;

        public float shakeDecay;
        public float shakeIntensity;

        private float _shakeDecay;
        private float _shakeIntensity;
        private Vector3 shakeMovement;
        private bool addShake;

        private void FixedUpdate()
        {
            if (_shakeIntensity > 0)
            {
                if (addShake)
                {
                    shakeMovement = Random.insideUnitSphere * _shakeIntensity;
                    transform.Translate(shakeMovement);

                    transform.rotation = Quaternion.Euler(
                                    (float)(originRotation.x + Random.Range(-_shakeIntensity, _shakeIntensity) * .2),
                                    (float)(originRotation.y + Random.Range(-_shakeIntensity, _shakeIntensity) * .2),
                                    (float)(originRotation.z + Random.Range(-_shakeIntensity, _shakeIntensity) * .2));

                    _shakeIntensity -= _shakeDecay;
                    addShake = false;
                }
                else
                {
                    transform.Translate(-shakeMovement);
                    transform.rotation = originRotation;

                    addShake = true;
                }
            }
        }

        public void Shake()
        {
            originPosition = transform.position;
            originRotation = transform.rotation;

            _shakeIntensity = shakeIntensity;
            _shakeDecay = shakeDecay;
            addShake = true;
        }

        public void Shake(float iShakeIntensity, float iShakeDecay)
        {
            originPosition = transform.position;
            originRotation = transform.rotation;

            _shakeIntensity = iShakeIntensity;
            _shakeDecay = iShakeDecay;
        }
    }

}