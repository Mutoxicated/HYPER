using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public static class MoveAbilities
{
    private static int t;

    public static void Jump(Rigidbody rb, Vector3 pointNormal, float jumpForce)
    {
        pointNormal.y = 1f;
        rb.AddForce(pointNormal.normalized * jumpForce, ForceMode.Impulse);
    }

    public static void Slide(Rigidbody rb, Vector3 direction, float slideSpeed)
    {
        direction.y = 0f;
        rb.velocity = direction * slideSpeed;
    }

    public static void GroundSlam(Rigidbody rb, float slamSpeed)
    {
        rb.velocity = Vector3.down * slamSpeed;
    }

    public static void Dash(Rigidbody rb, Vector3 direction, float dashSpeed)
    {
        rb.AddForce(direction * dashSpeed, ForceMode.Impulse);
    }

    public static bool Lock(Rigidbody rb, int duration)
    {
        t++;
        if (t > duration)
        {
            t = 0;
            return true;
        }
        return false;
    }
}