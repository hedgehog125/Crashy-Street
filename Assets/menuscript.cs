using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class menuscript : MonoBehaviour
{
    public bool moveLeft;
    public bool moveRight;
    public bool mainmenu;
    private void OnMoveLeft(InputValue input)
    {
        moveLeft = input.isPressed;
    }
    private void OnMoveRight(InputValue input)
    {
        moveRight = input.isPressed;
    }
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
    void Update()
    {
        if (enabled && mainmenu)
        {
            if (moveLeft)
            {
                LoadScene("Car Select");
            }
            if (moveRight)
            {
                LoadScene("Jacks Generation");
            }
        }
        
    }
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
