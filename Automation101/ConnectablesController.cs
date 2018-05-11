using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Automation101
{
    public class ConnectablesController : MonoBehaviour, IProtoEventListener
    {
        private static List<UnityEngine.Object> destroyOnUnload = new List<UnityEngine.Object>();

        public static Dictionary<string, Connectable> Connected = new Dictionary<string, Connectable>();

        public static void Load()
        {
            Unload();
            destroyOnUnload.Add(new GameObject("ConnectablesController").AddComponent<ConnectablesController>().gameObject);
        }

        public static void Unload()
        {
            while (destroyOnUnload.Count > 0)
            {
                if (destroyOnUnload[0])
                    UnityEngine.Object.Destroy(destroyOnUnload[0]);
                destroyOnUnload.RemoveAt(0);
            }
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            while (!uGUI_SceneLoading.IsLoadingScreenFinished || !uGUI.main || uGUI.main.loading.IsLoading)
            {
                yield return null;
            }
            try
            {
                Run();
                yield break;
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                Unload();
                yield break;
            }
        }

        private void Run()
        {
            // Load Connectables
            LoadConnectables();
        }

        public void SaveConnectables()
        {
            var savePathDir = Path.Combine(Main.GetSavePathDir(), "Connectables");

            if (!Directory.Exists(savePathDir))
                Directory.CreateDirectory(savePathDir);
            
            foreach(var connectable in Connected)
            {
                var id = connectable.Key;
                var path = Path.Combine(savePathDir, id + ".txt");

                if (connectable.Value.ConnectedTo != null)
                {
                    var connectedId = connectable.Value.ConnectedTo.Identifier.Id;

                    File.WriteAllText(path, connectedId);
                }
                else
                {
                    Console.WriteLine("ConnectedTo Null!");
                }
            }
        }

        public void LoadConnectables()
        {
            var savePathDir = Path.Combine(Main.GetSavePathDir(), "Connectables");

            if (Directory.Exists(savePathDir))
            {
                var files = new DirectoryInfo(savePathDir).GetFiles();

                foreach (var file in files)
                {
                    var id = Path.GetFileNameWithoutExtension(file.Name);
                    var connectedToId = File.ReadAllText(file.FullName);

                    var main = UniqueIdentifierHelper.GetByName(id);
                    var connectedTo = UniqueIdentifierHelper.GetByName(connectedToId);

                    if (main && connectedTo)
                        main.GetComponent<Connectable>().OnConnectEnd(connectedTo.GetComponent<Connectable>());
                    else
                        Console.WriteLine("Nullll!");
                }
            }
        }

        public void OnProtoSerialize(ProtobufSerializer serializer)
        {
            SaveConnectables();
        }

        public void OnProtoDeserialize(ProtobufSerializer serializer)
        {
            LoadConnectables();
        }
    }
}
