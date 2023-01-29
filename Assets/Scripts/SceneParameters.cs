using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneParameters
{
    public static bool isDemo {get; set;} = false;
    public static bool enableMovement {get; set;} = false;
    public static int ShieldDegrees { get; set; } = 45;

    public static ShieldController.Control control {get; set;} = ShieldController.Control.X_Axis;

    public static MenuController.LoadedScene scene {get; set;} = MenuController.LoadedScene.Static;
}