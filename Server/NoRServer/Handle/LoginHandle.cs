using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace NoRServer.Handle
{
    class LoginHandle : BaseHandle
    {
        public LoginHandle()
        {
            eOperationCode = EOperationCode.UserLogin;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {

            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)EUserInfo.Username, out object username);
            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)EUserInfo.Password, out object password);

            bool iVerify = Manager.UserManager.Get.VerifyUser(username.ToString(), password.ToString());

            OperationResponse operation = new OperationResponse((byte)eOperationCode);

            EResponse eResponse = EResponse.False;
            if (iVerify)
            {
                eResponse = EResponse.True;
                peer.Username = username.ToString();
            }

            operation.ReturnCode = (short)eResponse;
            
            peer.SendOperationResponse(operation, sendParameters);

        }
    }
}
