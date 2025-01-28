using System;
using System.Collections;
using System.Collections.Generic;
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
    }
    [Serializable]
    public class SixthSenseSettings
    {
        public float Cooldown;
        public SenseSphere sense;
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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
