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
namespace ClientNetwork
{
	public class LengthFieldPrepender : OutboundHandlerAdapter
	{
		const int MAX_SIZE = 2048 * 2048 * 8;
		int LengthBytes = 4;
		public LengthFieldPrepender()
		{
			tempBuf = ByteBuf.allocOne(MAX_SIZE);
		}
		ByteBuf tempBuf;
		public override void write(ConnectionHandlerContext ctx, object msg)
		{
			if (msg is ByteBuf) {
                tempBuf.Clear();
				ByteBuf buf = (ByteBuf)msg;
				if (LengthBytes == 2) {
					ushort len = (ushort)buf.remainBytes ();
					tempBuf.writeUInt16 (len);
				}
				else if (LengthBytes == 4) {
					uint len = (uint)buf.remainBytes ();
					tempBuf.writeUInt32 (len);

				}
				else if (LengthBytes == 8) {
					ulong len = (ulong)buf.remainBytes ();
					tempBuf.writeUInt64 (len);
				}
				tempBuf.writeBytes(buf);

				ctx.fireWrite(tempBuf);
			}
		}
		
	}
}

