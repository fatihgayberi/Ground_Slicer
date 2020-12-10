using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet 
{
    GameObject myBullet;
    Rigidbody rb;

    float m2;
    float vX1;
    float vY1;


    public Bullet(Vector3 playerPos, Vector3 shoterPos, GameObject newBullet, float bulletSpeed)
    {
        myBullet = newBullet;
        m2 = (playerPos.z - shoterPos.z) / (playerPos.x - shoterPos.x);
        vX1 = bulletSpeed / Mathf.Sqrt(1 + m2 * m2);
        vX1 = Mathf.Abs(vX1);
        if (playerPos.x < shoterPos.x)
        {
            vX1 *= -1;
        }
        vY1 = m2 * vX1;

        Vector3 bulletPosition = new Vector3(shoterPos.x + vX1 * 0.3f, shoterPos.y + 1.2f, shoterPos.z + vY1 * 0.3f);

        myBullet.transform.position = bulletPosition;
        rb = myBullet.GetComponent<Rigidbody>();
    }


    public void BulletPositionUpdate()
    {
        rb.velocity = new Vector3(vX1, 0, vY1);
    }

    public GameObject GetBulletObject()
    {
        return myBullet;
    }

}
