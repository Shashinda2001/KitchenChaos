using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanCounter : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform TomatoPrefab;
    public void Interact() {         
        Debug.Log("Interacting with clean counter");
        Transform tomatoTransform = Instantiate(TomatoPrefab, counterTopPoint);
         tomatoTransform.localPosition = Vector3.zero;
    }
}
