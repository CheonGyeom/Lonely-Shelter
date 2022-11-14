using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject Option;
    public GameObject DefaultMenu;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void inGame()
    {
        SceneManager.LoadScene("InGame");
    }

    public void OptionMenu(bool active)
    {
        Option.SetActive(active);
        DefaultMenu.SetActive(false);
    }

    public void OptionMenuBack()
    {
        Option.SetActive(false);
        DefaultMenu.SetActive(true);

    }

    public void Quit()
    {

    }

}
