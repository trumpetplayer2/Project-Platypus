using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        public float rotationSmoothSpeed = 0.1f;
        public LayerMask blocked;
        public float WallJitterTolerance = 0.25f;
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
        public Vector3 FreecamCap = new Vector3(20, 20, 20);
    }

    public class CameraFollow : MonoBehaviour
    {
        //Instance
        public static CameraFollow instance;
        //Public Settings
        public CameraCinematicVariables CinematicSettings;
        public CameraMovementSettings MovementSettings;
        public CameraFollowVariables cSettings;
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
        Rigidbody body;
        


        public void Awake()
        {
            instance = this;
            angle = cSettings.startAngle;
            locationOffset = cSettings.locationOffset;
            distance = cSettings.distance;
            body = this.GetComponent<Rigidbody>();
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
                Time.timeScale = 1f;
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
            if (cSettings.playerTracker == null) return;
            float x = Mathf.Clamp(cSettings.playerTracker.position.x, cSettings.minLocations.x, cSettings.maxLocations.x);
            float y = Mathf.Clamp(cSettings.playerTracker.position.y, cSettings.minLocations.y, cSettings.maxLocations.y);
            float z = Mathf.Clamp(cSettings.playerTracker.position.z, cSettings.minLocations.z, cSettings.maxLocations.z);
            Vector3 tempTracker = new Vector3(x, y, z);
            

            //Update Angle
            angle += Input.GetAxis("RotateCamera") * MovementSettings.rotationSpeed * Settings.cameraSensitivity;
            angle = angle % 360;
            
            locationOffset = new Vector3((Mathf.Sin(Mathf.Deg2Rad * angle) * distance), locationOffset.y, (Mathf.Cos(Mathf.Deg2Rad * angle) * distance));
            if (!freecam)
            {
                //Check player relation to camera
                Vector3 desiredPosition = tempTracker + Quaternion.Euler(0, 0, 0) * locationOffset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, cSettings.smoothSpeed);
                //transform.position = smoothedPosition;
                body.velocity = (smoothedPosition - transform.position)/Time.deltaTime;
            }
            cSettings.rotationOffset.y = angle-180;

            //Rotation
            Quaternion desiredrotation = Quaternion.Euler(0,0,0) * Quaternion.Euler(cSettings.rotationOffset);
            Quaternion smoothedrotation = Quaternion.Lerp(transform.rotation, desiredrotation, cSettings.rotationSmoothSpeed);
            transform.rotation = smoothedrotation;
        }
        private void Update()
        {
            if (GameManager.instance.isPaused) return;
            Ray ray = new Ray(PlayerMovement.instance.transform.position, transform.position - PlayerMovement.instance.transform.position);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, distance, cSettings.blocked))
            {
                if (!(hitInfo.collider.gameObject.tag.ToLower().Equals("player") || hitInfo.collider.gameObject.tag.ToLower().Equals("maincamera")))
                {
                    Vector3 loc = new Vector3(hitInfo.point.x, locationOffset.y, hitInfo.point.z);
                    //If camera is below player, and player is below max height, tp to y of player
                    if (loc.y < cSettings.playerTracker.position.y && cSettings.playerTracker.position.y < cSettings.maxLocations.y)
                    {
                        loc = new Vector3(loc.x, cSettings.playerTracker.position.y, loc.z);
                    }
                    //Only move camera if loc is further than .1 units away. This prevents camera vibration on certain walls
                    if(Vector3.Distance(loc, transform.position) > cSettings.WallJitterTolerance)
                    {
                        transform.position = loc;
                    }
                    body.velocity = Vector3.zero;
                }
            }
            if (shakeDuration > 0)
            {
                transform.localPosition = transform.position + Random.insideUnitSphere * shakeAmount;

                shakeDuration -= Time.deltaTime * decreaseSpeed;
            }

            float cameraZoomChange = Input.GetAxis("Mouse ScrollWheel");
            distance -= cameraZoomChange * MovementSettings.zoomScale;
            distance = Mathf.Clamp(distance, MovementSettings.minZoom, MovementSettings.maxZoom);
            //Scale current y by the initial settings
            locationOffset.y = distance / cSettings.distance * cSettings.locationOffset.y;

            //If camera lock is disabled, input is not needed
            if (Input.GetButton("MoveCamera") || !Settings.cameraLock)
            {
                angle += Input.GetAxis("Mouse X") * Settings.cameraSensitivity;
                //Cursor.lockState = CursorLockMode.Locked;
            }
            if (Input.GetButtonUp("MoveCamera"))
            {
                //Cursor.lockState = CursorLockMode.None;
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
            //Get bounds
            Vector3 pos = PlayerMovement.instance.gameObject.transform.position;
            Vector3 max = new Vector3(pos.x + MovementSettings.FreecamCap.x, pos.y + MovementSettings.FreecamCap.y, pos.z + MovementSettings.FreecamCap.z);
            Vector3 min = new Vector3(pos.x - MovementSettings.FreecamCap.x, pos.y - MovementSettings.FreecamCap.y, pos.z - MovementSettings.FreecamCap.z);
            float x = Mathf.Clamp(movement.x + transform.position.x, min.x, max.x);
            x = x - transform.position.x;
            float y = Mathf.Clamp(movement.y + transform.position.y, min.y, max.y);
            y = y - transform.position.y;
            float z = Mathf.Clamp(movement.z + transform.position.z, min.z, max.z);
            z = z - transform.position.z;

            body.velocity = (new Vector3(x, y, z)) / Time.deltaTime;
        }

        public float getAngle()
        {
            return angle;
        }
    }
}
