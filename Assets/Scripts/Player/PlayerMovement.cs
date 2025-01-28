using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace tp2
{
    [System.Serializable]
    public class PlayerCameraSettings
    {
        public bool strictFollow = true;
    }
    [System.Serializable]
    public class ChildrenSetting
    {
        public Rigidbody body;
    }
    [System.Serializable]
    public class WalkSpeed
    {
        public float baseSpeed = 5;
        public float runSpeedMult = 1.5f;
        public float waterSpeedMult = 0.5f;
    }
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        public PlayerCameraSettings cameraSettings;
        public ChildrenSetting childSettings;
        public WalkSpeed speed;
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
                if (this.TryGetComponent<Rigidbody>(out Rigidbody obj))
                {
                    childSettings.body = obj;
                }
                else
                {
                    childSettings.body = gameObject.AddComponent<Rigidbody>();
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
            Vector3 movement = new Vector3(forward, 0, strafe);
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
            childSettings.body.velocity = movement * speed.baseSpeed * Time.deltaTime * mult * 50;

        }
    }
}
