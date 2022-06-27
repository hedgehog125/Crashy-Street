using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class settingsmenu : MonoBehaviour
{
    private void Start()
    {
        
    }
    // Start is called before the first frame update
    public void good()
    {
        QualitySettings.resolutionScalingFixedDPIFactor = 1;
        PlayerPrefs.SetFloat("targetdpi", 1);
    }

    // Update is called once per frame
    public void bad()
    {
        QualitySettings.resolutionScalingFixedDPIFactor = 0.5f;
        PlayerPrefs.SetFloat("targetdpi", 0.5f);
    }

    public void home()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
