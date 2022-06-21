using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CarChange : MonoBehaviour
{
    public GameObject carhold;
    private bool moveLeft;
    private bool moveRight;
    public int car;
    public Text txt;
    public bool menu;
    int childcount;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in carhold.transform)
        {
            childcount += 1;
        }
        car = PlayerPrefs.GetInt("SelectedCar", 0);
        ChangeCar();
    }

    private void OnMoveLeft(InputValue input)
    {
        moveLeft = input.isPressed;
    }
    private void OnMoveRight(InputValue input)
    {
        moveRight = input.isPressed;
    }
    bool prevleft;
    bool prevright;
    private void Update()
    {
        if (menu)
        {
            if (moveLeft && prevleft != moveLeft)
            {
                if (car == 0) car = childcount - 1;
                else car--;
                PlayerPrefs.SetInt("SelectedCar", car);
                ChangeCar();
            }
            if (moveRight && moveRight != prevright)
            {
                if (car == childcount - 1) car = 0;
                else car++;
                PlayerPrefs.SetInt("SelectedCar", car);
                ChangeCar();
            }
            prevleft = moveLeft;
            prevright = moveRight;
        }
    }
    private void ChangeCar()
    {
        int i = 0;
        foreach (Transform child in carhold.transform)
        {
            if (i == car)
            {
                if (menu)
                {
                    txt.text = child.name;
                }
                child.gameObject.SetActive(true);
            }
            else child.gameObject.SetActive(false);
            i++;
        }
    }
}
