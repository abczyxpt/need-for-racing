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

            LogInit.Log.Info("登录状态" + iVerify);

            OperationResponse operation = new OperationResponse((byte)eOperationCode);

            EResponse eResponse = EResponse.False;
            if (iVerify)
            {
                //判断是否在线
                if (!Manager.UserManager.Get.IsOnline(username.ToString()))
                {
                    eResponse = EResponse.True;
                    peer.Username = username.ToString();
                    Manager.UserManager.Get.ChangeLoading(username.ToString(), true);
                }
                else
                {
                    operation.Parameters = new Dictionary<byte, object>
                    {
                        { (byte)EFeedbackInfo.Info,"账号已登录"},
                    };
                }
            }

            operation.ReturnCode = (short)eResponse;
            
            peer.SendOperationResponse(operation, sendParameters);

        }
    }
}
