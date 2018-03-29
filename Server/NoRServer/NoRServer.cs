using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace NoRServer
{

    public class NoRServer : ApplicationBase
    {

        /// <summary>
        /// 当一个客户端请求连接
        /// </summary>
        /// <param name="initRequest"></param>
        /// <returns></returns>
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new ClientPeer(initRequest);
        }


        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Setup()
        {

        }


        /// <summary>
        /// server关闭
        /// </summary>
        protected override void TearDown()
        {
            throw new NotImplementedException();
        }
    }
}
