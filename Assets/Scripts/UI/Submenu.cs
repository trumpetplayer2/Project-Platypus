using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submenu : MonoBehaviour
{
    public void toggleMenu(GameObject otherMenu)
    {
        otherMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
