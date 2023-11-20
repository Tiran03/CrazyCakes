using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;


public class Victory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Golpe");

    }


    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Debug.Log("Golpe");
            LoadingManager.Instance.LoadScene(8, 9);

        }

    }
}
