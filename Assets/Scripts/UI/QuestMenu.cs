using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMenu : MonoBehaviour
{
    public bool state = true;
    bool finished = true;
    public float openTime = 1f;
    float timer = 0;
    public Vector2 open = new Vector2 (0, 0);
    public Vector2 closed = new Vector2(-800, 0);
    public static QuestMenu instance;

    private void Start()
    {
        Cursor.lockState = (state) ? CursorLockMode.Locked : CursorLockMode.Confined;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("QuestMenu") && finished)
        {
            finished = false;
            timer = 0;
        }
        if (!finished)
        {
            timer += Time.deltaTime;
            if (state)
            {
                transform.localPosition = Vector3.Lerp(open, closed, timer / openTime);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(closed, open, timer / openTime);
            }
            if(timer >= openTime )
            {
                finished = true;
                state = !state;
                Cursor.lockState = (state) ? CursorLockMode.Locked : CursorLockMode.Confined;
            }
        }
    }
}
