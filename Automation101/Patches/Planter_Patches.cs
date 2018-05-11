using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace Automation101.Patches
{
    [HarmonyPatch(typeof(Planter))]
    [HarmonyPatch("Start")]
    public class Planter_Start_Patch
    {
        static void Postfix(Planter __instance)
        {
            if (__instance.GetComponent<StorageConnectable>() != null)
                MonoBehaviour.DestroyImmediate(__instance.GetComponent<StorageConnectable>());

            __instance.gameObject.AddComponent<PlanterConnectable>();
        }
    }
}
