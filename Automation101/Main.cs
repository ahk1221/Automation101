using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using SMLHelper;
using SMLHelper.Patchers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Harmony;

namespace Automation101
{
    public class Main
    {
        public static TechType ConnectToolTechType;

        public static GameObject ConnectingStuff;

        public static void Patch()
        {
            var harmony = HarmonyInstance.Create("com.ahk1221.automation101");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            var assetBundle = AssetBundle.LoadFromFile("./QMods/Automation101/connectingassets.assets");
            ConnectingStuff = assetBundle.LoadAsset<GameObject>("ConnectStuff");

            ConnectToolTechType = TechTypePatcher.AddTechType("ConnectTool", "Connect Tool", "Connect tool.");

            CraftDataPatcher.customEquipmentTypes[ConnectToolTechType] = EquipmentType.Hand;

            CustomPrefabHandler.customPrefabs.Add(new CustomPrefab("ConnectTool", "WorldEntities/Tools/ConnectTool", ConnectToolTechType, GetConnectToolPrefab));
        }

        private static void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(scene.name == "Main")
            {
                ConnectablesController.Load();
            }
        }

        public static GameObject GetConnectToolPrefab()
        {
            var obj = Resources.Load<GameObject>("WorldEntities/Tools/FlashLight");
            var prefab = GameObject.Instantiate(obj);

            var techTag = prefab.GetComponent<TechTag>();
            var prefabIdentifier = prefab.GetComponent<PrefabIdentifier>();

            techTag.type = ConnectToolTechType;
            prefabIdentifier.ClassId = "ConnectTool";

            MonoBehaviour.DestroyImmediate(prefab.GetComponent<FlashLight>());

            var connectTool = prefab.AddComponent<ConnectTool>();
            connectTool.ikAimRightArm = true;
            connectTool.ikAimLeftArm = false;
            connectTool.useLeftAimTargetOnPlayer = false;

            return prefab;
        }

        public static string GetSavePathDir()
        {
            var savePathDir = Path.Combine(@".\SNAppData\SavedGames\", Utils.GetSavegameDir());
            return Path.Combine(savePathDir, "Automation101");
        }
    }
}
