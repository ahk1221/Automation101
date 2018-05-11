using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace Automation101.Patches
{
    [HarmonyPatch(typeof(StorageContainer))]
    [HarmonyPatch("Awake")]
    public class StorageContainer_Patches
    {
        static void Postfix(StorageContainer __instance)
        {
            var connectable = __instance.gameObject.AddComponent<StorageConnectable>();
            connectable.container = __instance;
            connectable.Identifier = __instance.GetComponentInParent<PrefabIdentifier>();
        }
    }
}
