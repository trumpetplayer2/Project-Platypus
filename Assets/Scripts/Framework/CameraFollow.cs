using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace tp2
{
    [System.Serializable]
    public class CameraShakeVar
    {
        [Tooltip("Maximum possible location in a circle around camera, in 1/100ths of a unit")]
        public float shakeAmount = 1;
        [Tooltip("Shake time in ms")]
        public float shakeTime = 1;
        public CameraShakeVar(float amount, float time)
        {
            shakeAmount = amount;
            shakeTime = time;
        }
    }

    public class CameraFollow : MonoBehaviour
    {
        //Camera Follow Variables
        public Vector3 maxLocations = new Vector3(10, 10, 10);
        public Vector3 minLocations = new Vector3(-10, -10, -10);
        public float distance;
        public float angle;
        public Transform playerTracker;
        public float smoothSpeed = 0.125f;
        public Vector3 locationOffset;
        public Vector3 rotationOffset;
        //Camera Shake Variables
        public float shakeAmount = 0.25f;
        public float decreaseSpeed = 1.0f;
        public float shakeDuration = 0f;
        public static CameraFollow instance;
        //Cinematic Camera
        public bool cinematicMode = false;
        public int cinematicNumber = 0;
        int sceneNumber = 0;
        int positionNumber = 0;
        float time = 0;
        Vector3 startPos;
        public GameObject Waiting;

        public void Awake()
        {
            instance = this;
        }

        public void Start()
        {
            if (cinematicMode)
            {
                startCutscene(0);
            }
            else
            {
                //paused = false;
            }
        }

        public void startCutscene(int number)
        {
            sceneNumber = number;
            positionNumber = 0;
            time = 0;
            //paused = true;
            startPos = this.transform.position;
        }

        public void endCutscene()
        {
            cinematicMode = false;
            //paused = false;
            if (Waiting != null)
            {
                Waiting.SetActive(false);
            }
        }

        public void FixedUpdate()
        {
            if (cinematicMode)
            {
                cinematicCamUpdate();
            }
            else
            {
                followCamUpdate();
            }
        }

        private void cinematicCamUpdate()
        {
            //paused = true;
        }

        private void followCamUpdate()
        {
            
            if (playerTracker == null) return;
            
            Vector3 tempTracker = new Vector3(playerTracker.position.x, playerTracker.position.y, playerTracker.position.z);

            if (playerTracker.position.x < minLocations.x)
            {
                tempTracker = new Vector3(minLocations.x, playerTracker.position.y, playerTracker.position.z);
            }
            else if (playerTracker.position.x > maxLocations.x)
            {
                tempTracker = new Vector3(maxLocations.x, playerTracker.position.y, playerTracker.position.z);
            }

            if (playerTracker.position.y < minLocations.y)
            {
                tempTracker = new Vector3(tempTracker.x, minLocations.y, playerTracker.position.z);
            }
            else if (playerTracker.position.y > maxLocations.y)
            {
                tempTracker = new Vector3(tempTracker.x, maxLocations.y, playerTracker.position.z);
            }

            if (playerTracker.position.z < minLocations.z)
            {
                tempTracker = new Vector3(tempTracker.x, tempTracker.y, minLocations.z);
            }
            else if (playerTracker.position.y > maxLocations.y)
            {
                tempTracker = new Vector3(tempTracker.x, tempTracker.y, maxLocations.z);
            }

            if(angle > 360)
            {
                angle -= 360;
            }
            if(angle < 0)
            {
                angle += 360;
            }

            locationOffset = new Vector3((Mathf.Sin(angle) * distance), locationOffset.y, (Mathf.Cos(angle) * distance));

            //Check player relation to camera
            Vector3 desiredPosition = tempTracker + playerTracker.rotation * locationOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            //Rotation
            Quaternion desiredrotation = playerTracker.rotation * Quaternion.Euler(rotationOffset);
            Quaternion smoothedrotation = Quaternion.Lerp(transform.rotation, desiredrotation, smoothSpeed);
            transform.rotation = smoothedrotation;
        }

        private void Update()
        {
            if (shakeDuration > 0)
            {
                transform.localPosition = transform.position + Random.insideUnitSphere * shakeAmount;

                shakeDuration -= Time.deltaTime * decreaseSpeed;
            }
        }

        public void shake(float amount = 1f, float time = 1f)
        {
            amount = amount * 0.01f;
            time = time * 0.001f;
            shakeDuration = Mathf.Max(time, shakeDuration);
            shakeAmount = Mathf.Max(amount, shakeAmount);
        }

        public void shake(Vector2 s)
        {
            shake(s.x, s.y);
        }

        public void shake(CameraShakeVar var)
        {
            shake(var.shakeAmount, var.shakeTime);
        }
    }
}
