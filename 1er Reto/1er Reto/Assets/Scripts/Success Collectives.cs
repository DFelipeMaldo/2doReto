using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuccessCollectives : MonoBehaviour
{
    public int sceneTag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.childCount == 0)
        {
            SceneManager.LoadScene(sceneTag);
        }
    }
}
