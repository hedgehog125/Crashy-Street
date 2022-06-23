using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CarChange : MonoBehaviour
{
    public Slider[] turnaccs;
    public Slider[] accs;
    public Slider[] speeds;
    public int unlockcost;
    public int turncost;
    public int speedcost;
    public int acccost;
    public int turnacc;
    public int acc;
    public int speed;
    public int maxturnacc;
    public int maxacc;
    public int maxspeed;
    public int unlocked;
    public GameObject box;
    public GameObject carhold;
    public GameObject maincar;
    public Text elmoney;
    public bool moveLeft;
    public bool moveRight;
    public int car;
    public Text txt;
    public bool menu;
    public int offset;
    public bool reset;
    int childcount;
    // Start is called before the first frame update
    void Start()
    {
        car = PlayerPrefs.GetInt("SelectedCar", 0);
        turnacc = PlayerPrefs.GetInt("turnacc", 4);
        acc = PlayerPrefs.GetInt("acc", 4);
        speed = PlayerPrefs.GetInt("speed", 8);
        maxturnacc = PlayerPrefs.GetInt("maxturnacc", 4);
        maxacc = PlayerPrefs.GetInt("maxacc", 4);
        maxspeed = PlayerPrefs.GetInt("maxspeed", 8);
        unlocked = PlayerPrefs.GetInt("unlocked", 4);
        PlayerPrefs.SetInt("SelectedCar", car);
        PlayerPrefs.SetInt("turnacc", turnacc);
        PlayerPrefs.SetInt("acc", acc);
        PlayerPrefs.SetInt("speed", speed);
        PlayerPrefs.SetInt("maxturnacc", maxturnacc);
        PlayerPrefs.SetInt("maxacc", maxacc);
        PlayerPrefs.SetInt("maxspeed", maxspeed);
        PlayerPrefs.SetInt("unlocked", unlocked);
        foreach (Transform child in carhold.transform)
        {
            childcount += 1;
        }
        ChangeCar();
        GetComponent<CarMovement>().maxSpeed *= speed;
        GetComponent<CarMovement>().acceleration *= acc;
        GetComponent<CarMovement>().turnAcceleration *= turnacc;
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
    public void increaseturnmaxacc()
    {
        if (PlayerPrefs.GetInt("money", 0) >= turncost)
        {
            maxturnacc += 1;
            if (maxturnacc > 4) maxturnacc = 4;
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0)-turncost);
        }
        PlayerPrefs.SetInt("maxturnacc", maxturnacc);
    }
    public void increasemaxspeed()
    {
        if (PlayerPrefs.GetInt("money", 0) >= speedcost)
        {
            maxspeed += 1;
            if (maxspeed > 4) maxspeed = 4;
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) - speedcost);
        }
        PlayerPrefs.SetInt("maxspeed", maxspeed);
    }
    public void increasemaxacc()
    {
        if (PlayerPrefs.GetInt("money", 0) >= acccost)
        {
            maxacc += 1;
            if (maxacc > 4) maxacc = 4;
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) - acccost);
        }
        PlayerPrefs.SetInt("maxacc", maxacc);
    }
    public void changeturnacc(int amount)
    {
        if (turnacc + amount != maxturnacc + 1 && turnacc + amount > 0)
        {
            turnacc += amount;
            if (turnacc > 4) turnacc = 4;
        }
        PlayerPrefs.SetInt("turnacc", turnacc);
    }
    public void changespeed(int amount)
    {
        if (speed+amount != maxspeed+1&&speed+amount>0)
        {
            speed += amount;
            if (speed > 4) speed = 4;
        }
        PlayerPrefs.SetInt("speed", speed);
    }
    public void changeacc(int amount)
    {
        if (acc + amount != maxacc + 1 && acc + amount > 0)
        {
            acc += amount;
            if (acc > 4) acc = 4;
        }
        PlayerPrefs.SetInt("acc", acc);
    }
    public void nextcar()
    {
        if (PlayerPrefs.GetInt("unlocked", 1) + 1 <= childcount)
        {
            PlayerPrefs.SetInt("unlocked", PlayerPrefs.GetInt("unlocked", 1) + 1);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) - unlockcost);
        }
            
        unlocked = PlayerPrefs.GetInt("unlocked", 1);
    }
    bool prevleft;
    bool prevright;
    private void Update()
    {
        if (reset)
        {
            PlayerPrefs.SetInt("speed", 8);
            PlayerPrefs.SetInt("turnacc", 4);
            PlayerPrefs.SetInt("acc", 4);
            PlayerPrefs.SetInt("maxspeed", 8);
            PlayerPrefs.SetInt("maxturnacc", 4);
            PlayerPrefs.SetInt("maxacc", 4);
            PlayerPrefs.SetInt("unlocked", 1);
        }
        
        if (menu)
        {
            
            try
            {
                speeds[1].value = speed;
                speeds[0].value = 4 - maxspeed;
                turnaccs[1].value = turnacc;
                turnaccs[0].value = 4 - maxturnacc;
                accs[1].value = acc;
                accs[0].value = 4 - maxacc;
                elmoney.text = $"{PlayerPrefs.GetInt("money", 0)}";
            }
            catch {
                unlocked = PlayerPrefs.GetInt("unlocked", 1);
                    }
            if (maincar == null)
            {
                if (moveLeft && prevleft != moveLeft)
                {
                    if (car == 0) car = 0;
                    else car--;
                    if (offset == 0 && car < unlocked) PlayerPrefs.SetInt("SelectedCar", car);
                    
                    ChangeCar();
                }
                if (moveRight && moveRight != prevright)
                {
                    if (car < childcount-1) car++;
                    if (offset == 0 && car < unlocked) PlayerPrefs.SetInt("SelectedCar", car);
                    ChangeCar();
                }
            }
            else
            {
                car = maincar.GetComponent<CarChange>().car + offset;
                ChangeCar();
            }
            prevleft = moveLeft;
            prevright = moveRight;
        }
        if (car < unlocked)
        {
            box.SetActive(false);
            carhold.SetActive(true);
        }
        else
        {
            box.SetActive(true);
            carhold.SetActive(false);
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
                    if(txt != null) txt.text = child.name;
                }
                child.gameObject.SetActive(true);
            }
            else child.gameObject.SetActive(false);
            i++;
        }
    }
}
