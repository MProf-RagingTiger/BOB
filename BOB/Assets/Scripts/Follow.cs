﻿using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour
{

    public GameObject target;
    public float xOffset;
    public float yOffset;


    // Use this for initialization
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x + xOffset, target.transform.position.y + yOffset, transform.position.z);
 

    }
}