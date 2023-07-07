using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static float Direction2Angle(Vector2 direction) {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
