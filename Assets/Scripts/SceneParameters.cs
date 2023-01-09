using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneParameters
{
    public static int ShieldDegrees { get; set; } = 60;

    public static ShieldController.Control control {get; set;} = ShieldController.Control.X_Axis;

}