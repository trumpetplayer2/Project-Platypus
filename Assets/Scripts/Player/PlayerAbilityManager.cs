using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace tp2
{
    [Serializable]
    public class GravelSettings
    {
        public int Max;
        public float Cooldown;
        public GameObject countIndicator;
        public GameObject gravelEntity;
    }
    [Serializable]
    public class DigSettings
    {
        public float Cooldown;
        public GameObject digTip;
        public string DigInputName = "Dig";
    }
    [Serializable]
    public class SixthSenseSettings
    {
        public float Cooldown = 1f;
        public float length = 10f;
        public SenseSphere sphere;
        public string SenseInputName = "SixthSense";
    }
    public class PlayerAbilityManager : MonoBehaviour
    {
        //Gravel Stuff
        public GravelSettings gravel;
        private int gravelCount;
        private float gravelCooldown = 0;
        //Dig Stuff
        public DigSettings dig;
        private float digCooldown = 0;
        //Sixth Sense
        public SixthSenseSettings sense;
        private float senseCooldown = 0;
        bool canSense = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.instance.isPaused) return;
            DecrementCooldown();
            if (Input.GetButtonDown(sense.SenseInputName))
            {
                if (canSense)
                {
                    Sense();
                }
            }
            if (Input.GetButton(dig.DigInputName))
            {
                Dig();
            }
            Gravel();

        }

        void DecrementCooldown()
        {
            senseCooldown -= Mathf.Max(0, senseCooldown - Time.deltaTime);
            digCooldown -= Mathf.Max(0, senseCooldown - Time.deltaTime);
            gravelCooldown -= Mathf.Max(0, senseCooldown - Time.deltaTime);
        }
        void Sense()
        {
            sense.sphere.showDetect();
            Invoke("SenseFade", sense.length);
        }

        void SenseFade()
        {
            sense.sphere.hideDetect();
        }

        void Gravel()
        {
            CollectGravel();
            SpitGravel();
        }
        void CollectGravel()
        {

        }

        void SpitGravel()
        {

        }

        void Dig()
        {

        }
    }
}
