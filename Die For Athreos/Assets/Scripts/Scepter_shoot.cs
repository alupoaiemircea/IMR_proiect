using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scepter_shoot : MonoBehaviour
{
    public Camera cam;
    public Transform attackPoint;
    public GameObject projectile;
    public float shootForce;
    public float timeBetweenShooting;
    private bool allowInvoke = true;
    public void ShootProjectile()
    {

        //GameObject attack = new GameObject();
        //attack.AddComponent<Rigidbody>();
        //attack.AddComponent<BoxCollider>();
        //attack.tag = "attack";
        //attack.AddComponent<Collide>();
        //attack.transform.position = Vector3.MoveTowards(transform.position, middle of screen, 100f * Time.deltaTime);
        float x = Screen.width / 2f;
        float y = Screen.height / 2f;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(50);

        Vector3 direction = targetPoint - attackPoint.position;
        GameObject currentBullet = Instantiate(projectile, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = direction.normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }
    private void ResetShot()
    {
        allowInvoke = true;
    }
}
