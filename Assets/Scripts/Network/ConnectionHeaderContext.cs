//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Net.Sockets;

namespace ClientNetwork
{
	public class ConnectionHeaderContext : ConnectionHandlerContext
	{
		const string HEADER_NAME = "headercontext";
		public ConnectionHeaderContext(Connection connection):base(HEADER_NAME,null,false,true)
		{

			this.connection = connection;
		}

		public override void invokeWrite(object message)
		{

			ByteBuf buf = (ByteBuf)message;

            /*
            var output = new System.IO.FileStream(Application.dataPath + "\\buf.txt", System.IO.FileMode.Append);
            var writer = new System.IO.StreamWriter(output, System.Text.Encoding.UTF8);
            writer.WriteLine("buf.remainBytes() = " + buf.remainBytes().ToString());
            writer.Close();
            output.Close();
            */

			

			Socket clientSocket = this.connection.getClientSocket();
			clientSocket.Send(buf.getBuffer(), buf.getReaderIndex(), buf.remainBytes(), SocketFlags.None);
		}

		public override void invokeRead(object message)
		{
			throw new NotImplementedException();
		}

		public override void invokeReadComplete()
		{
			throw new NotImplementedException();
		}

		Connection connection;
	}
}
