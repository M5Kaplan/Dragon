using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }


    void Update()
    {
        transform.Rotate(0f, 50f * Time.deltaTime, 0);
    }
  
}
