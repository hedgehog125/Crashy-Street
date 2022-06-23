using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class menuscript : MonoBehaviour
{
    public bool mainmenu;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("enable", 0.2f);
    }
    bool enabled = false;
    void enable()
    {
        enabled = true;
    }
    // Update is called once per frame
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
