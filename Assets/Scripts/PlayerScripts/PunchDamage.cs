using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchDamage : MonoBehaviour
{
    public int punchDamage = 33; // damage to apply when punching
                                 // Start is called before the first frame update

    public bool isPunching = false; // Flag to track if the player is currently punching

    // This method will be called by PlayerAttackController when the punch starts
    public void StartPunch()
    {
        isPunching = true;
    }

    // This method will be called by PlayerAttackController when the punch ends
    public void EndPunch()
    {
        isPunching = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Detected with: " + collision.gameObject.name);
        if (isPunching)
            if (collision.gameObject.CompareTag("Bat"))
            {
                Debug.Log("Bat hit");
                BatDamage batDamage = collision.gameObject.GetComponent<BatDamage>();
                if (batDamage != null)
                {
                    batDamage.ApplyDamage(punchDamage);
                }
            }
            else if (collision.gameObject.CompareTag("Slime"))
            {
                Debug.Log("Slime hit");
                SlimeDamage slimeDamage = collision.gameObject.GetComponent<SlimeDamage>();
                if (slimeDamage != null)
                {
                    slimeDamage.ApplyDamage(punchDamage);
                }
            }
        }
    }
