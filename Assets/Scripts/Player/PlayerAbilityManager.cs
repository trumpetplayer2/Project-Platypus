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
    }
    [Serializable]
    public class DigSettings
    {
        public float Cooldown;
        public GameObject digTip;
        public string DigInputName = "Dig";
        public UnityEvent dig = new UnityEvent();
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
            if (Input.GetButtonDown(dig.DigInputName))
            {
                if(digCooldown <= 0) Dig();
            }
            if (Input.GetButtonDown("Gravel"))
            {
                if(gravelCooldown <= 0) Gravel();
            }
        }

        void DecrementCooldown()
        {
            senseCooldown = Mathf.Max(0, senseCooldown - Time.deltaTime);
            digCooldown = Mathf.Max(0, senseCooldown - Time.deltaTime);
            gravelCooldown = Mathf.Max(0, senseCooldown - Time.deltaTime);
            canSense = senseCooldown <= 0;

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
        void CollectGravel()
        {
            Debug.Log("Collect");
            gravelCount = gravel.Max;
            updateGravelUI();
        }

        void SpitGravel()
        {
            if(gravelCount <= 0)
            {
                return;
            }
            Instantiate(gravel.gravelEntity, gravel.gravelSpawnLocation.position, gravel.gravelSpawnLocation.rotation);
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
            dig.dig?.Invoke();
        }
    }
}
