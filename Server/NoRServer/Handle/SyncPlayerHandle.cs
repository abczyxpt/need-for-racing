﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace NoRServer.Handle
{
    public class SyncPlayerHandle : BaseHandle
    {
        public SyncPlayerHandle()
        {
            this.eOperationCode = EOperationCode.SyncPlayerHandle;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {

        }
    }
}
