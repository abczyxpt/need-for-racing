using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace NoRServer.Handle
{
    class RegisterHandle : BaseHandle
    {
        public RegisterHandle()
        {
            eOperationCode = EOperationCode.UserRegister;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            operationRequest.Parameters.TryGetValue((byte)EUserInfo.Username, out object username); 
            operationRequest.Parameters.TryGetValue((byte)EUserInfo.Password, out object password);

            //判断数据库中是否有同名账号
            Model.User user = Manager.UserManager.Get.GetInfoByName(username.ToString());
            bool isRegiste = false;
            //数据库插入操作
            if(user == null)
            {
                user = new Model.User { Username = username.ToString(), Password = password.ToString(), Registerdate = DateTime.Now.ToString("yyyy-MM-dd") };
                Manager.UserManager.Get.Add(user);
                isRegiste = true;
            }
            //反馈给客户端
            OperationResponse operation = new OperationResponse((byte)eOperationCode);
            EResponse eResponse = EResponse.False;
            if (isRegiste) eResponse = EResponse.True;
            operation.ReturnCode = (short)eResponse;
            peer.SendOperationResponse(operation, sendParameters);

        }
    }
}
