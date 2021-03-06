using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerShooting _playerShooting;
    private float shootingDelay = 0;
    private bool isShooting = false;
    private void Awake() {
        _playerShooting = GetComponent<PlayerShooting>();
    }
    // Update is called once per frame
    void Update()
    {
        Stun();
        Suction();
    }

    void Stun ()
    {
        float mouseLeftButton = Input.GetAxis("Fire1");
        if (mouseLeftButton != 0)
        {
            if (shootingDelay == 0)
            {
                GetComponent<Animator>().SetTrigger("Stun");
                shootingDelay = Time.time+1.2f;
                isShooting = true;
            }
        }

        if (isShooting)
        {
            if (Time.time > shootingDelay)
            {
                _playerShooting.createBullet();
                isShooting = false;
                shootingDelay = 0;
            }
        }
    }

    void Suction()
    {
        float mouseRightButton = Input.GetAxis("Fire2");
        GetComponent<Animator>().SetFloat("Suction", mouseRightButton);
    }
}
