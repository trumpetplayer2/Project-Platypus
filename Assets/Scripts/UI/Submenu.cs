using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submenu : MonoBehaviour
{
    public int layer = 0;
    public PauseScript pause;
    public void toggleMenu(GameObject otherMenu)
    {
        otherMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void togglePauseMenu(Submenu otherMenu)
    {
        if(pause != null)
        {
            pause.currentMenu = otherMenu;
        }
        otherMenu.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        
    }
}
