using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrollingScript : MonoBehaviour
{

    public float scrollSpeed;

    public float timeTilMainM;

  

    RectTransform uITransform;

    // Start is called before the first frame update
    void Start()
    {
        uITransform = GetComponent<RectTransform>();

        StartCoroutine(ReturnToMainMenu());
    }

    // Update is called once per frame
    void Update()
    {

        uITransform.transform.position += new Vector3(0, scrollSpeed * Time.deltaTime, 0);

        
    }

    IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(timeTilMainM);
        SceneManager.LoadScene(0);
    }
}
