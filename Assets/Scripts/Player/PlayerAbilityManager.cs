using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace tp2
{
    [Serializable]
    public class GravelSettings
    {
        public int Max;
        public float Cooldown;
        public Image countIndicator;
        public Sprite[] sprites;
        public GameObject gravelEntity;
        public Transform gravelSpawnLocation;
        public AudioClip collectClip;
        public AudioClip spitClip;
        public ParticleSystem gravelParticle;
    }
    [Serializable]
    public class DigSettings
    {
        public float Cooldown;
        public GameObject digTip;
        public string DigInputName = "Dig";
        public UnityEvent dig = new UnityEvent();
        public AudioClip clip;
        public bool triggered = false;
        public ParticleSystem digParticle;
        public List<GameObject> DigObjects = new List<GameObject>();
    }
    [Serializable]
    public class SixthSenseSettings
    {
        public float Cooldown = 1f;
        public float length = 10f;
        public SenseSphere sphere;
        public string SenseInputName = "SixthSense";
        public AudioClip clip;
    }
    [Serializable]
    public class  HoldSettings
    {
        public Transform holdLocation;
        public ItemScript heldObject;
        public string grabInput = "Grab";
        public float Cooldown = 1f;
        public UnityEvent grabEvent = new UnityEvent();
        public AudioClip grabClip;
        public AudioClip dropClip;
    }
    [Serializable]
    public class NoiseMaker
    {
        public Clip[] Clips;
        public string Input = "Noise";
        public float noiseCooldown = .5f;
    }
    public class PlayerAbilityManager : MonoBehaviour
    {
        public static PlayerAbilityManager instance;
        //Gravel Stuff
        public GravelSettings gravel;
        public bool canCollectGravel = false;
        private int gravelCount;
        private float gravelCooldown = 0;
        //Dig Stuff
        public DigSettings dig;
        private float digCooldown = 0;
        //Sixth Sense
        public SixthSenseSettings sense;
        private float senseCooldown = 0;
        bool canSense = true;
        //Grab settings
        public HoldSettings grab;
        float grabCooldown = 0f;
        AudioSettings audioSettings;
        public Animator animator;
        public NoiseMaker noiseController;
        float noiseCooldown = 0;

        private void Start()
        {
            audioSettings = new AudioSettings(gravel.collectClip, gravel.spitClip, dig.clip, sense.clip, grab.grabClip, grab.dropClip);
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.instance.isPaused) return;
            DecrementCooldown();
            if (Input.GetButtonDown(noiseController.Input) && noiseCooldown <= 0)
            {
                System.Random rand = new System.Random();
                AudioHandler.instance.queueClip(noiseController.Clips[rand.Next(noiseController.Clips.Length)]);
                noiseCooldown = noiseController.noiseCooldown;
            }
            if (PlayerMovement.instance.held) return;
            if (Input.GetButton(sense.SenseInputName))
            {
                Sense();
            }
            if (Input.GetButtonUp(sense.SenseInputName))
            {
                SenseFade();
            }
            if (Input.GetButtonDown(grab.grabInput))
            {
                if (grabCooldown <= 0)
                {
                    if (Grab()) return;
                }
            }
            if (Input.GetButtonDown(dig.DigInputName))
            {
                if(digCooldown <= 0) Dig();
                if (dig.triggered) return;
            }
            if (Input.GetButtonDown("Gravel"))
            {
                if(gravelCooldown <= 0) Gravel();
            }
        }

        void DecrementCooldown()
        {
            senseCooldown = Mathf.Max(0, senseCooldown - Time.deltaTime);
            digCooldown = Mathf.Max(0, digCooldown - Time.deltaTime);
            gravelCooldown = Mathf.Max(0, gravelCooldown - Time.deltaTime);
            grabCooldown = Mathf.Max(0, grabCooldown - Time.deltaTime);
            noiseCooldown = Mathf.Max(0, noiseCooldown - Time.deltaTime);
            canSense = senseCooldown <= 0;

        }
        void Sense()
        {
            sense.sphere.showDetect();
            queueClip(3);
            //Invoke("SenseFade", sense.length);
        }

        void SenseFade()
        {
            sense.sphere.hideDetect();
        }

        void Gravel()
        {
            if (grab.heldObject != null) return;
            gravelCooldown = gravel.Cooldown;
            if (canCollectGravel && gravelCount < gravel.Max)
            {
                CollectGravel();
                return;
            }
            else
            {
                SpitGravel();
            }
        }

        void queueClip(int slot)
        {
            AudioHandler.instance.queueClip(audioSettings.toClip(slot));
        }
        void CollectGravel()
        {
            gravelCount = gravel.Max;
            AudioHandler.instance.queueClip(gravel.collectClip);
            updateGravelUI();
        }

        void SpitGravel()
        {
            if(gravelCount <= 0)
            {
                return;
            }
            Instantiate(gravel.gravelEntity, gravel.gravelSpawnLocation.position, gravel.gravelSpawnLocation.rotation);
            AudioHandler.instance.queueClip(gravel.spitClip);
            gravelCount -= 1;
            updateGravelUI();
        }

        void updateGravelUI()
        {
            switch(gravelCount)
            {
                case 0:
                    gravel.countIndicator.sprite = gravel.sprites[0];
                    return;
                case 1:
                    gravel.countIndicator.sprite = gravel.sprites[1];
                    return;
                case 2:
                    gravel.countIndicator.sprite = gravel.sprites[2];
                    return;
                case 3:
                    gravel.countIndicator.sprite = gravel.sprites[3];
                    return;
            }
        }

        void Dig()
        {
            dig.triggered = false;
            dig.dig?.Invoke();
            if(dig.digParticle != null)
            {
                foreach(GameObject obj in dig.DigObjects)
                {
                    SpawnParticle(dig.digParticle, obj);
                }
            }
            digCooldown = dig.Cooldown;
            if (dig.triggered)
            {
                queueClip(2);
                Invoke("DigClip", 1);
                Invoke("DigClip", 2);
            }
        }
        void DigClip()
        {
            queueClip(2);
        }

        void SpawnParticle(ParticleSystem Particle, GameObject target)
        {
            if(Physics.Raycast(this.transform.position, target.transform.position - this.transform.position, out RaycastHit hitInfo))
            {
                Instantiate(Particle, hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
            }
        }

        bool Grab()
        {
            if(grab.heldObject != null) {
                Release();
                return true;
            }
            grab.grabEvent?.Invoke();
            grabCooldown = grab.Cooldown;
            if (grab.heldObject == null) return false;
            queueClip(4);
            return false;
        }
        public void Release()
        {
            if (grab.heldObject != null)
            {
                grab.heldObject.release();
                grabCooldown = grab.Cooldown;
                queueClip(5);
                return;
            }
        }
    }
}
