using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelper : MonoBehaviour
{
    public static Vector2 RandomVector2(float angle, float angleMin){
        float random = Random.value * angle + angleMin;
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }
    
    public static Vector2 RandomVector2BetweenAngle(float angleMax, float angleMin)
    {
        float random = Random.Range(angleMin, angleMax);
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }
    
    public static Vector2 DegreeToVector2(float degree)
    {  
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
}
