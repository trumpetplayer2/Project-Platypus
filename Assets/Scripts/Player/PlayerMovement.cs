using UnityEngine;
namespace tp2
{
    [System.Serializable]
    public class PlayerCameraSettings
    {
        public bool strictFollow = true;
    }
    [System.Serializable]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerChildrenSetting
    {
        public CharacterController body;
    }
    [System.Serializable]
    public class WalkSpeed
    {
        public float baseSpeed = 5;
        public float runSpeedMult = 1.5f;
        public float waterSpeedMult = 0.5f;
        public float gravity = 9.81f;
        public float terminalVelocity = 10;
        [Tooltip("Slope Min is when slope is at 0")]
        public float slopeMultiplierMin = 0;
        [Tooltip("Slope Max is determined by Character Controller")]
        public float slopeMultiplierMax = 0;
    }
    public class PlayerMovement : MonoBehaviour
    {
        public PlayerCameraSettings cameraSettings;
        public PlayerChildrenSetting childSettings;
        public WalkSpeed speed;
        float lastGravity = 0;
        public static PlayerMovement instance;
        // Start is called before the first frame update
        void Start()
        {
            if(instance == null)
            {
                instance = this;
            }
            if(childSettings.body == null)
            {
                if (this.TryGetComponent<CharacterController>(out CharacterController obj))
                {
                    childSettings.body = obj;
                }
                else
                {
                    childSettings.body = gameObject.AddComponent<CharacterController>();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(!GameManager.instance.isPaused) { unpausedUpdate(); }
        }

        void unpausedUpdate()
        {
            //Get Direction
            float strafe = Input.GetAxis("Horizontal");
            float forward = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(forward, 0, -strafe);
            float angle = 0;
            if(cameraSettings.strictFollow)
            {
                angle = CameraFollow.instance.angle + 90;
            }
            else
            {
                angle = transform.rotation.y;
            }
            movement = Quaternion.AngleAxis(angle, Vector3.up) * movement;
            float mult = 1;
            if(Input.GetButton("Sprint"))
            {
                //Animation Sprint
                mult += speed.runSpeedMult;
            }
            //In Water Check
            if (false)
            {
                mult += speed.waterSpeedMult;
            }
            //Current Slope
            float slope = calculateSlope();
            mult += Mathf.Lerp(speed.slopeMultiplierMin, speed.slopeMultiplierMax, (slope/childSettings.body.slopeLimit));
            movement = movement * speed.baseSpeed * Time.deltaTime * mult;
            //Calculate Gravity if needed
            float gravity = 0;
            if (!childSettings.body.isGrounded)
            {
                //Velocity = Initial Velocity + (Gravity) * Delta time
                gravity = lastGravity + -speed.gravity * Time.deltaTime; 
                if(gravity > speed.terminalVelocity)
                {
                    gravity = speed.terminalVelocity;
                }
            }
            else
            {
                gravity = 0;
            }
            movement.y = gravity;
            childSettings.body.Move(movement);
            lastGravity = gravity;
        }

        public float calculateSlope()
        {
            if(Physics.Raycast(transform.position, -transform.up, out RaycastHit HitInfo))
            {
                //Normal: HitInfo.normal
                //Up: Vector3.up
                return Vector3.Angle(HitInfo.normal, Vector3.up);
            }
            else
            {
                return 0;
            }
        }
    }
}
