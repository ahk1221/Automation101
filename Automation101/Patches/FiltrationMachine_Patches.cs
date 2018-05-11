using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using Harmony;

namespace Automation101.Patches
{
    [HarmonyPatch(typeof(FiltrationMachine))]
    [HarmonyPatch("Start")]
    public class FiltrationMachine_Start_Patch
    {
        static void Postfix(FiltrationMachine __instance)
        {
            if (__instance.GetComponent<StorageConnectable>() != null)
                MonoBehaviour.DestroyImmediate(__instance.GetComponent<StorageConnectable>());
        }
    }

    [HarmonyPatch(typeof(BaseFiltrationMachineGeometry))]
    [HarmonyPatch("Start")]
    public class BaseFiltrationMachineGeometry_Start_Patch
    {
        static MethodInfo GetModuleMethod =
            typeof(BaseFiltrationMachineGeometry).
            GetMethod("GetModule", BindingFlags.NonPublic | BindingFlags.Instance);

        static void Postfix(BaseFiltrationMachineGeometry __instance)
        {
            foreach (var t in __instance.GetComponentsInChildren<Transform>())
            {
                if (t.name == "HandTarget")
                {
                    if (t.gameObject.GetComponent<StorageConnectable>() == null)
                    {
                        var connectable = t.gameObject.AddComponent<StorageConnectable>();
                        var module = (FiltrationMachine)GetModuleMethod.Invoke(__instance, new object[] { });
                        connectable.container = module.storageContainer;
                        connectable.Identifier = module.GetComponentInParent<PrefabIdentifier>();
                    }
                }
            }
        }
    }
}
