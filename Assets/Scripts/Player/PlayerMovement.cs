using UnityEngine;
namespace tp2
{
    /// <summary>
    /// Camera Modes:
    /// Follow - Player and Camera rotate together
    /// Free Cam - Camera moves, but player doesn't. Screenshots!
    /// Independent - Camera and player have seperate rotations
    /// </summary>
    public enum cameraMode { Assist, Locked, Unlocked, FreeCam }
    [System.Serializable]
    public class PlayerCameraSettings
    {
        public cameraMode followMode = cameraMode.Assist;
    }
    [System.Serializable]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerChildrenSetting
    {
        public CharacterController body;
        public Rigidbody rb;
        public Animator animator;
    }
    public enum SlopeMode{None, Linear}
    [System.Serializable]
    public class WalkSpeed
    {
        public float baseSpeed = 5;
        public float runSpeedMult = 1.5f;
        public float waterSpeedMult = 0.5f;
        public float gravity = 9.81f;
        public float slideSpeed = 10f;
        public float terminalVelocity = 10;
        public SlopeMode slopeMode = SlopeMode.Linear;
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
        public bool swimming = false;
        public bool held = false;
        float spinTimer = 0;
        public float spinWaitTime = 10f;
        bool spinning = false;
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
                childSettings.rb = this.GetComponent<Rigidbody>();
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(!GameManager.instance.isPaused) { unpausedUpdate(); }
        }
        public void warp(Vector3 position)
        {
            childSettings.body.enabled = false;
            transform.position = position;
            childSettings.body.enabled = true;
        }
        void unpausedUpdate()
        {
            if (Input.GetButtonDown("SwitchCameraMode"))
            {
                updateCameraMode();
            }
            if (Input.GetButtonDown("CameraModeAssist"))
            {
                cameraSettings.followMode = cameraMode.Assist;
            } else if (Input.GetButtonDown("CameraModeStrict"))
            {
                cameraSettings.followMode = cameraMode.Locked;
            }
            else if (Input.GetButtonDown("CameraModeUnlocked"))
            {
                cameraSettings.followMode = cameraMode.Unlocked;
            }
            else if (Input.GetButtonDown("Freecam"))
            {
                cameraSettings.followMode = cameraMode.FreeCam;
            }
            if(!spinning && spinTimer > spinWaitTime)
            {
                childSettings.animator.SetTrigger("Spin");
            }
            spinTimer += Time.deltaTime;
            //Get Direction
            float strafe = Input.GetAxisRaw("Horizontal");
            float forward = Input.GetAxisRaw("Vertical");
            //Calculate base movement
            Vector3 movement = new Vector3(forward, 0, -strafe).normalized;
            //Create a variable angle for later use
            float angle = 0;
            if (held)
            {
                childSettings.animator.SetBool("Grabbed", true);
                return;
            }
            else
            {
                childSettings.animator.SetBool("Grabbed", false);
            }
            //Different Camera Modes!
            switch (cameraSettings.followMode)
            {
                case cameraMode.Assist:
                    //Forward facing matters, Get Cam Forward, disregard y rotation
                    Vector3 cameraForward = new Vector3(CameraFollow.instance.transform.right.x, 0, CameraFollow.instance.transform.right.z);
                    cameraForward.Normalize();
                    //Update Angle
                    angle = CameraFollow.instance.getAngle() + 90;
                    //Update movement
                    movement = Quaternion.AngleAxis(angle, Vector3.up) * movement;
                    if (movement.magnitude > 0)
                    {
                        //Face direction of movement
                        transform.rotation = Quaternion.FromToRotation(Vector3.right, Quaternion.Euler(0, -90, 0) * movement);
                    }
                    break;
                case cameraMode.Locked:
                    angle = CameraFollow.instance.getAngle() + 90;
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, CameraFollow.instance.transform.eulerAngles.y + 180, transform.eulerAngles.z);
                    movement = Quaternion.AngleAxis(angle, Vector3.up) * movement;
                    break;
                case cameraMode.Unlocked:
                    if (movement.magnitude > 0)
                    {
                        transform.rotation = Quaternion.FromToRotation(Vector3.right, movement);
                    }
                    movement = Quaternion.AngleAxis(90, Vector3.up) * movement;
                    break;
                case cameraMode.FreeCam:
                    angle = CameraFollow.instance.getAngle() + 90;
                    movement = Quaternion.AngleAxis(angle, Vector3.up) * movement;
                    break;
            }
            childSettings.animator.SetBool("Walking", movement.magnitude > 0);
            if(movement.magnitude > 0)
            {
                spinTimer = 0;
                spinning = false;
                childSettings.animator.ResetTrigger("Spin");
            }
            float mult = 1;
            if (Input.GetButton("Sprint"))
            {
                //Animation Sprint
                mult += speed.runSpeedMult;
                childSettings.animator.SetBool("Sprinting", true);
            }
            else
            {
                childSettings.animator.SetBool("Sprinting", false);
            }
            //In Water Check
            if (swimming)
            {
                mult += speed.waterSpeedMult;
                childSettings.animator.SetBool("Swimming", true);
            }
            else
            {
                childSettings.animator.SetBool("Swimming", false);
            }
            //Current Slope
            float slope = calculateSlope(out Vector3 normal);
            switch (speed.slopeMode)
            {
                case SlopeMode.Linear:
                    mult += Mathf.Lerp(speed.slopeMultiplierMin, speed.slopeMultiplierMax, (slope / childSettings.body.slopeLimit));
                    break;
                case SlopeMode.None:
                    
                    break;
            }
            //Finalize horizontal plane movement
            movement = movement * speed.baseSpeed * mult;
            if (float.IsNaN(movement.x) || float.IsNaN(movement.y) || float.IsNaN(movement.z)) return;
            //Freecam check! This movement goes to freecam if in freecam mode
            if (cameraSettings.followMode == cameraMode.FreeCam)
            {
                //Send movement to camera;
                CameraFollow.instance.freecamUpdate(movement);
                //Set movement here to zero
                movement = Vector3.zero;
                childSettings.animator.SetBool("Walking", false);
                childSettings.animator.SetBool("Sprinting", false);
            }else if (CameraFollow.instance.freecam){
                CameraFollow.instance.freecam = false;
            }
            //Calculate Gravity if needed
            float gravity = 0;
            if (!childSettings.body.isGrounded)
            {
                //Velocity = Initial Velocity + (Gravity) * Delta time
                gravity = lastGravity + -speed.gravity; 
                if(gravity > speed.terminalVelocity)
                {
                    gravity = speed.terminalVelocity;
                }
            }
            else
            {
                gravity = 0;
            }
            if(slope > childSettings.body.slopeLimit)
            {
                Vector3 temp = normal.normalized;
                movement = new Vector3(temp.x,-.5f,temp.z) * speed.slideSpeed;
            }
            movement.y = gravity;
            childSettings.body.Move(movement*Time.fixedDeltaTime);
            lastGravity = gravity;
        }

        public float calculateSlope(out Vector3 normal)
        {
            if(Physics.Raycast(transform.position, -transform.up, out RaycastHit HitInfo))
            {
                //Normal: HitInfo.normal
                //Up: Vector3.up
                normal = HitInfo.normal;
                return Vector3.Angle(HitInfo.normal, Vector3.up);
            }
            else
            {
                normal = Vector3.zero;
                return 0;
            }
        }

        public void updateCameraMode()
        {
            switch (cameraSettings.followMode)
            {
                case cameraMode.Assist:
                    cameraSettings.followMode = cameraMode.Locked;
                    break;
                case cameraMode.Locked:
                    cameraSettings.followMode = cameraMode.FreeCam;
                    break;
                case cameraMode.Unlocked:
                    cameraSettings.followMode = cameraMode.FreeCam;
                    break;                
                case cameraMode.FreeCam:
                    cameraSettings.followMode = cameraMode.Assist;
                    break;
            }
        }
    }
}
