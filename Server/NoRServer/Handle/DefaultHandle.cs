﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace NoRServer.Handle
{
    class DefaultHandle : BaseHandle
    {
        public DefaultHandle()
        {
            this.eOperationCode = EOperationCode.DefaultHandle;
        }
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {

        }
    }
}
