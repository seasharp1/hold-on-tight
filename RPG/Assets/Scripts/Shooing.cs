using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooing : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bullet;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}
