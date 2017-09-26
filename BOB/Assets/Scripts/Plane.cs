using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlaneState { IDLE, MOVING, CHASING};

public class Plane : MonoBehaviour
{

    public bool britishPlane;
    public Vector3 targetLocation;
    public Plane targetPlane;
    public float speed;
    public float accelration;
    public float rotationSpeed;
    public PlaneState planeState;

    Rigidbody rigBod;

    public float angle;
    public float targetAngle;

    public float shotDistance;
    public bool ready2Fire = true;
    public float shotSpeed;
    private float shotTimer = 0;
    public GameObject bullet;

    HealthBar health;

    AudioSource audioSource;


    // Use this for initialization
    void Start() {

        rigBod = GetComponent<Rigidbody>();
        health = GetComponent<HealthBar>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update() {

        bool flyLeft = false;
        bool flyRight = false;
        bool flyStraight = false;


        switch (planeState)
        {
            case PlaneState.IDLE:
                {

                    transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                    rigBod.AddForce(transform.forward * accelration * Time.deltaTime);

                    break;
                }
            case PlaneState.CHASING:
                {

                    if (targetPlane == null)
                    {
                        planeState = PlaneState.IDLE;
                    }

                    //find the direction of the plane
                    Vector3 targetDir = targetPlane.transform.position - transform.position;

                    //find the angle 
                    angle = Vector3.Angle(transform.forward, targetDir);

                    //find if the angle is negtive or postive
                    int sign = Vector3.Cross(transform.forward, targetDir).y < 0 ? -1 : 1;

                    //make the angle postive or negtive
                    angle *= sign;

                    float targetDistance = Vector3.Distance(transform.position, targetPlane.transform.position);

                    if (targetAngle == 0 && targetDistance < shotDistance)
                    {
                        FireBullet();

                        flyStraight = true;
                    }

                    //rotate the plane
                    if (angle < targetAngle)
                    {
                        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                        flyRight = true;
                    }
                    else if (angle > targetAngle)
                    {
                        transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
                        flyLeft = true;
                    }

                    rigBod.AddForce(transform.forward * accelration * Time.deltaTime);


                    break;
                }
            case PlaneState.MOVING:
                {
                    //find the direction of the plane
                    Vector3 targetDir = targetLocation - transform.position;

                    //find the angle 
                    angle = Vector3.Angle(transform.forward, targetDir);

                    //find if the angle is negtive or postive
                    int sign = Vector3.Cross(transform.forward, targetDir).y < 0 ? -1 : 1;

                    //make the angle postive or negtive
                    angle *= sign;


                    //rotate the plane
                    if (angle < targetAngle)
                    {
                        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

                        flyRight = true;

                    }
                    else if (angle > targetAngle)
                    {
                        transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);

                        flyLeft = true;
                    }
                    else
                    {
                        flyStraight = true;
                    }

                    rigBod.AddForce(transform.forward * accelration * Time.deltaTime);

                    if (Vector3.Distance(targetLocation, transform.position) < 2)
                    {
                        planeState = PlaneState.IDLE;

                    }

                    break;
                }
        }

        if(flyLeft)
        {
            Vector3 planeRot = transform.rotation.eulerAngles;
            float newAngle = Mathf.Lerp(planeRot.z, 45, 0.1f);
            planeRot.z = newAngle;
            transform.rotation = Quaternion.Euler(planeRot);
        }
        if(flyRight)
        {
            Vector3 planeRot = transform.rotation.eulerAngles;
            float newAngle = Mathf.Lerp(planeRot.z, -45, 0.1f);
            planeRot.z = newAngle;
            transform.rotation = Quaternion.Euler(planeRot);
        }

        BulletUpdate();



        rigBod.velocity = Vector3.ClampMagnitude(rigBod.velocity, speed);

    }

  

    private void BulletUpdate()
    {
        //shot
        if (!ready2Fire)
        {
            shotTimer += Time.deltaTime;

            if (shotTimer > shotSpeed)
            {
                ready2Fire = true;

                shotTimer -= shotSpeed;
            }
        }
    }

    private void FireBullet()
    {
        if(ready2Fire)
        {
            GameObject newBullet = Instantiate(bullet, transform.position,transform.rotation);
            newBullet.GetComponent<BulletScript>().britishBullet = britishPlane;

            Debug.Log("Audio Played");
            audioSource.Play();

            ready2Fire = false;
        }
    }

    public void SetTargetPostion(Vector3 newTarget)
    {
        targetLocation = newTarget;
        planeState = PlaneState.MOVING;
    }

    public void SetTargetPlane(Plane newTarget)
    {
        targetPlane = newTarget;
        planeState = PlaneState.CHASING;


    }

    void OnTriggerEnter(Collider col)
    {

        if(col.GetComponent<BulletScript>())
        {
            if(britishPlane != col.GetComponent<BulletScript>().britishBullet)
            health.TakeDamage(4);
            Destroy(col.gameObject);
        }
    }
}
