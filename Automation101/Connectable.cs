using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace Automation101
{
    public enum ConnectableType
    {
        Storage
    }

    public abstract class Connectable : MonoBehaviour
    {
        public Connectable ConnectedTo { get; set; }
        public PrefabIdentifier Identifier { get; set; }

        public bool IsBeingConnected { get; private set; }

        public abstract ConnectableType Type { get; }

        public Transform ConnectVisual;

        public LineRenderer line;

        private void Start()
        {
            var connectingStuff = Utils.SpawnFromPrefab(Main.ConnectingStuff, transform);
            connectingStuff.transform.position = transform.position;

            ConnectVisual = connectingStuff.transform;

            line = connectingStuff.GetComponentInChildren<LineRenderer>();
        }

        private void OnDestroy()
        {
            DestroyImmediate(ConnectVisual);
        }

        private void Update()
        {
            if(IsBeingConnected)
            {
                //line.SetPositions(new Vector3[]
                //{
                //    transform.position,
                 //   Inventory.main.GetHeldObject().transform.position
                //});
            }
        }

        public virtual void OnConnectStart()
        {
            ConnectedTo = null;
            IsBeingConnected = true;
        }

        public virtual void OnConnectEnd(Connectable connectedTo = null)
        {
            IsBeingConnected = false;

            //line.SetPositions(new Vector3[]
            //{
            //    Vector3.zero,
            ///    Vector3.zero
            //});

            if (connectedTo != null)
            {
                ConnectedTo = connectedTo;
                //line.SetPositions(new Vector3[]
                //{
                //    transform.position, 
                //    ConnectedTo.transform.position
                //});
            }
        }
    }
}
