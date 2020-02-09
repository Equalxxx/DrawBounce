using UnityEngine;
using System.Collections;

namespace MLFramework
{
    public class ShakeCamera : Singleton<ShakeCamera>
    {
        [Header("Default Info")]
        public float defaultPower;
        public float defaultTime;

        [Header("ShakePosInfo")]
        public bool shakePosX = true;
        public bool shakePosY = true;
        public bool shakePosZ = true;

        [Header("ShakeRotInfo")]
        public bool shakeRotX = true;
        public bool shakeRotY = true;
        public bool shakeRotZ = true;

        private bool useShake = false;

        public static void ShakePosOrder(float shakePower = 0f, float shakeTime = 0f)
        {
            if (shakePower == 0f)
                shakePower = Instance.defaultPower;

            if (shakeTime == 0f)
                shakeTime = Instance.defaultTime;

            float shakeRelease = shakePower / shakeTime;

            Instance.StartCoroutine(Instance.ShakePos(shakePower, shakeRelease));
        }

        public static void ShakeRotOrder(float shakePower = 0f, float shakeTime = 0f)
        {
            if (shakePower == 0f)
                shakePower = Instance.defaultPower;

            if (shakeTime == 0f)
                shakeTime = Instance.defaultTime;

            float shakeRelease = shakePower / shakeTime;

            Instance.StartCoroutine(Instance.ShakeRot(shakePower, shakeRelease));
        }

        IEnumerator ShakePos(float shakePosPower, float shakePosRelease)
        {
            if (useShake)
                yield break;

            useShake = true;
            Vector3 originPosition = transform.localPosition;

            while (shakePosPower > 0)
            {
                Vector3 randDir;
                randDir.x = ((shakePosX) ? Random.Range(-1.0f, 1.0f) : 0);
                randDir.y = ((shakePosY) ? Random.Range(-1.0f, 1.0f) : 0);
                randDir.z = ((shakePosZ) ? Random.Range(-1.0f, 1.0f) : 0);
                randDir.Normalize();

                Vector3 localPostion = originPosition + randDir * shakePosPower;
                transform.localPosition = localPostion;

                shakePosPower -= shakePosRelease * Time.deltaTime;

                if (shakePosPower < 0.0f)
                {
                    transform.localPosition = originPosition;
                    shakePosPower = 0.0f;
                }

                yield return null;
            }

            useShake = false;
        }

        IEnumerator ShakeRot(float shakeRotPower, float shakeRotRelease)
        {
            if (useShake)
                yield break;

            useShake = true;
            Quaternion originRotation = transform.localRotation;

            while (shakeRotPower > 0)
            {
                Vector3 randDir;
                randDir.x = ((shakeRotX) ? Random.Range(-1.0f, 1.0f) : 0);
                randDir.y = ((shakeRotY) ? Random.Range(-1.0f, 1.0f) : 0);
                randDir.z = ((shakeRotZ) ? Random.Range(-1.0f, 1.0f) : 0);
                randDir.Normalize();
            
                Vector3 localAngle = originRotation.eulerAngles + randDir * shakeRotPower;
                transform.localRotation = Quaternion.Euler(localAngle);
            
                shakeRotPower -= shakeRotRelease * Time.deltaTime;

                if (shakeRotPower < 0.0f)
                {
                    transform.localRotation = originRotation;
                    shakeRotPower = 0.0f;
                }

                yield return null;
            }

            useShake = false;
        }
    }
}
