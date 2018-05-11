using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace Automation101
{
    public class ConnectTool : PlayerTool
    {
        public override string animToolName => "flashlight";

        public bool IsConnecting;
        public Connectable Connecting;

        public override bool OnLeftHandDown()
        {
            var gameObject = default(GameObject);
            var dist = 0f;

            if(Targeting.GetTarget(100f, out gameObject, out dist))
            {
                ErrorMessage.AddMessage("Hit Object: " + gameObject.name);

                var connectable = gameObject.GetComponentInParent<Connectable>();
                if(connectable != null)
                {
                    IsConnecting = true;
                    Connecting = connectable;

                    Connecting.OnConnectStart();

                    ErrorMessage.AddMessage("Connecting: " + Connecting.transform.name);
                }
            }
            else
            {
                ErrorMessage.AddMessage("Did not hit Object!");
            }

            return base.OnLeftHandDown();
        }

        public override bool OnLeftHandUp()
        {
            var gameObject = default(GameObject);
            var dist = 0f;

            if (Targeting.GetTarget(100f, out gameObject, out dist))
            {
                var connectable = gameObject.GetComponentInParent<Connectable>();
                if (connectable != null && IsConnecting)
                {
                    Connecting.OnConnectEnd(connectable);
                    ConnectablesController.Connected.Add(Connecting.Identifier.Id, Connecting);

                    ErrorMessage.AddMessage("Connected: " + Connecting.transform.name + " to: " + connectable.transform.name);

                    return base.OnLeftHandUp();
                }
            }

            if (ConnectablesController.Connected.ContainsKey(Connecting.Identifier.Id))
                ConnectablesController.Connected.Remove(Connecting.Identifier.Id);

            Connecting.ConnectedTo = null;
            Connecting = null;
            IsConnecting = false;

            return base.OnLeftHandUp();
        }
    }
}
