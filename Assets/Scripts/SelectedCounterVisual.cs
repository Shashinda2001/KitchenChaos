using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private GameObject counterSelectedVisual;
    [SerializeField] private CleanCounter cleanCounter;
    // Start is called before the first frame update
    void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
      //  Debug.Log("Selected Counter Changed: " + e.selectedCounter);
        if (e.selectedCounter == cleanCounter)
        {
            show();
        }
        else
        {
            hide();
        }
    }

    private void hide()
    {
        counterSelectedVisual.SetActive(false);
    }

    private void show() {  
        counterSelectedVisual.SetActive(true);
    }

    // Update is called once per frame

}
