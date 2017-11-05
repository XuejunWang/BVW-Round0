using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCollision : MonoBehaviour {
    public PlayerScript player;
    private const string PLATFORM_TAG = "Platform";

    private void OnStart()
    {
        player.AllowJump = false; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Collision Detected");

        if (collision.collider.gameObject.CompareTag(PLATFORM_TAG))
        {
            player.AllowJump = true;
        }
    }
}
