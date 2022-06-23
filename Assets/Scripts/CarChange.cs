using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CarChange : MonoBehaviour
{
    public GameObject carhold;
    public bool moveLeft;
    public bool moveRight;
    public int car;
    public Text txt;
    public bool menu;
    public int offset;
    int childcount;
    // Start is called before the first frame update
    void Start()
    {
        car = PlayerPrefs.GetInt("SelectedCar", 0);
        foreach (Transform child in carhold.transform)
        {
            childcount += 1;
        }
        if (offset == -1)
        {
            if (car == 0) car = childcount - 1;
            else car--;
        }
        if (offset == 1)
        {
            if (car == childcount - 1) car = 0;
            else car++;
        }
        ChangeCar();
    }

    public void MoveLeft()
    {
        moveLeft = true;
    }
    public void NotMoveLeft()
    {
        moveLeft = false;
    }
    public void MoveRight()
    {
        moveRight = true;
    }
    public void NotMoveRight()
    {
        moveRight = false;
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
                if (offset == 0) PlayerPrefs.SetInt("SelectedCar", car);
                ChangeCar();
            }
            if (moveRight && moveRight != prevright)
            {
                if (car == childcount - 1) car = 0;
                else car++;
                if (offset == 0) PlayerPrefs.SetInt("SelectedCar", car);
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
