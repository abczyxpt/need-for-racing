using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoRServer.Handle
{
    public abstract class BaseHandle
    {
        public EOperationCode eOperationCode;
        public abstract void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters,ClientPeer peer);
    }
}
