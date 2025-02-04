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

    [System.Serializable]
    public class CameraFollowVariables
    {
        //Camera Follow Variables
        public Vector3 maxLocations = new Vector3(10, 10, 10);
        public Vector3 minLocations = new Vector3(-10, -10, -10);
        public float distance;
        public float startAngle;
        public Transform playerTracker;
        public float smoothSpeed = 0.125f;
        public Vector3 locationOffset;
        public Vector3 rotationOffset;
    }
    [System.Serializable]
    public class CameraCinematicVariables
    {
        //Cinematic Camera
        public bool cinematicMode = false;
        public int cinematicNumber = 0;
        
    }

    [System.Serializable]
    public class CameraMovementSettings
    {
        //Camera rotate speed
        public float rotationSpeed = 1f;
        public float minZoom = 3f;
        public float maxZoom = 20f;
        public float zoomScale = 0.2f;
    }

    public class CameraFollow : MonoBehaviour
    {
        //Instance
        public static CameraFollow instance;
        //Public Settings
        public CameraCinematicVariables CinematicSettings;
        public CameraMovementSettings MovementSettings;
        public CameraFollowVariables Settings;
        ////Private trackers
        //int sceneNumber = 0;
        //int positionNumber = 0;
        //float time = 0;
        //Vector3 startPos;
        float angle;
        public bool freecam = false;
        Vector3 locationOffset;
        float distance;
        //Camera Shake Variables
        float shakeAmount = 0.25f;
        float decreaseSpeed = 1.0f;
        float shakeDuration = 0f;
        
        


        public void Awake()
        {
            instance = this;
            angle = Settings.startAngle;
            locationOffset = Settings.locationOffset;
            distance = Settings.distance;
        }

        public void Start()
        {
            if (CinematicSettings.cinematicMode)
            {
                startCutscene(0);
            }
            else
            {
                GameManager.instance.isPaused = false;
            }
        }

        public void startCutscene(int number)
        {
            //sceneNumber = number;
            //positionNumber = 0;
            //time = 0;
            //GameManager.instance.isPaused = true;
            //startPos = this.transform.position;
        }

        public void endCutscene()
        {
            CinematicSettings.cinematicMode = false;
            GameManager.instance.isPaused = false;
        }

        public void FixedUpdate()
        {
            if (CinematicSettings.cinematicMode)
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
            GameManager.instance.isPaused = true;
        }

        private void followCamUpdate()
        {
            if (Settings.playerTracker == null) return;

            float x = Mathf.Clamp(Settings.playerTracker.position.x, Settings.minLocations.x, Settings.maxLocations.x);
            float y = Mathf.Clamp(Settings.playerTracker.position.y, Settings.minLocations.y, Settings.maxLocations.y);
            float z = Mathf.Clamp(Settings.playerTracker.position.z, Settings.minLocations.z, Settings.maxLocations.z);
            Vector3 tempTracker = new Vector3(x, y, z);
            

            //Update Angle
            angle += Input.GetAxis("RotateCamera") * MovementSettings.rotationSpeed;
            angle = angle % 360;
            
            locationOffset = new Vector3((Mathf.Sin(Mathf.Deg2Rad * angle) * distance), locationOffset.y, (Mathf.Cos(Mathf.Deg2Rad * angle) * distance));
            if (!freecam)
            {
                //Check player relation to camera
                Vector3 desiredPosition = tempTracker + Quaternion.Euler(0, 0, 0) * locationOffset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Settings.smoothSpeed);
                transform.position = smoothedPosition;
            }
            Settings.rotationOffset.y = angle-180;

            //Rotation
            Quaternion desiredrotation = Quaternion.Euler(0,0,0) * Quaternion.Euler(Settings.rotationOffset);
            Quaternion smoothedrotation = Quaternion.Lerp(transform.rotation, desiredrotation, Settings.smoothSpeed);
            transform.rotation = smoothedrotation;
        }

        private void Update()
        {
            if (GameManager.instance.isPaused) return;
            if (shakeDuration > 0)
            {
                transform.localPosition = transform.position + Random.insideUnitSphere * shakeAmount;

                shakeDuration -= Time.deltaTime * decreaseSpeed;
            }

            float cameraZoomChange = Input.GetAxis("Mouse ScrollWheel");
            distance -= cameraZoomChange * MovementSettings.zoomScale;
            distance = Mathf.Clamp(distance, MovementSettings.minZoom, MovementSettings.maxZoom);
            //Scale current y by the initial settings
            locationOffset.y = distance / Settings.distance * Settings.locationOffset.y;

            if (Input.GetButton("MoveCamera"))
            {
                angle += Input.GetAxis("Mouse X");
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (Input.GetButtonUp("MoveCamera"))
            {
                Cursor.lockState = CursorLockMode.None;
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

        public void freecamUpdate(Vector3 movement)
        {
            freecam = true;
            transform.position += movement;
        }

        public float getAngle()
        {
            return angle;
        }
    }
}
