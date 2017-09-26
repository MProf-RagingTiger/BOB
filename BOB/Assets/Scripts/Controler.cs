using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour {


    public Plane selectedPlane;

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
      

        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                
                if (hit.collider.GetComponent<Plane>())
                {
                    selectedPlane = hit.collider.GetComponent<Plane>();
                }
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                //if hit a unit attack that unit
                if (hit.collider.GetComponent<Plane>())
                {
                    selectedPlane.SetTargetPlane(hit.collider.GetComponent<Plane>());
                }
                else if (hit.collider.name == "Floor")
                {
                    selectedPlane.SetTargetPostion(hit.point);
                }
              
            }
        }
    }
}
