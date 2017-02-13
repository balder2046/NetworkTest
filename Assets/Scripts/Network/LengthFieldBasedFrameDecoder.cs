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
	public class LengthFieldBasedFrameDecoder : InboundHandlerAdapter
	{
		const int MAX_SIZE = 2048 * 2048 * 8;
		ByteBuf receivedBuf;
		int targetSize = 0;
		int LengthBytes = 4;

		public LengthFieldBasedFrameDecoder ()
		{
			receivedBuf = ByteBuf.allocOne (MAX_SIZE);
		}

		public override void read (ConnectionHandlerContext ctx, object msg)
		{
            
			ByteBuf inputBuf = (ByteBuf)msg;
			//UnityEngine.Debug.LogError("Read the Raw Message!!!! size = " + inputBuf.remainBytes());
			while (true) {
				//   UnityEngine.Debug.LogError("Loop Read Raw Message!!!! size = " + inputBuf.remainBytes());
				if (targetSize <= 0) {
					targetSize = 0;
					// The target size is not read complete
					int nowsize = receivedBuf.remainBytes ();
					if (nowsize < LengthBytes) {
						int needSizeForLength = LengthBytes - nowsize;
						for (int i = 0; i < needSizeForLength; ++i) {
							if (inputBuf.remainBytes () > 0) {
								receivedBuf.writeByte (inputBuf.readByte ());
							} else {
								break;
							}
						}

					}
					nowsize = receivedBuf.remainBytes ();
					//                  System.Console.WriteLine("nowSize " + nowsize);
					if (nowsize >= LengthBytes) {
						if (LengthBytes == 2)
							targetSize = receivedBuf.readUInt16 ();
						else if (LengthBytes == 4)
							targetSize = (int)receivedBuf.readUInt32 ();
						else if (LengthBytes == 8)
							targetSize = (int)receivedBuf.readUInt64 ();
						else
							throw new Exception ("Big Errror!!!!!!! , not engouh size bytes!!!");
                        
					} else {
						// note read the length complete ,wait for more bytes
						if (inputBuf.remainBytes () > 0) {
							throw new Exception ("Big Errror!!!!!!!");
						}
						return;
					}

				}
				// the targetSize must be ready
				if (targetSize <= 0) {
					throw new Exception ("the target size ERROR,that's must be a bug!");
				}






				//receivedBuf.writeBytes(inputBuf);
				for (int i = 0; i < targetSize && inputBuf.remainBytes () > 0; ++i) {
					receivedBuf.writeByte (inputBuf.readByte ());
				}
				// UnityEngine.Debug.LogError("write the Receive Message!!!! now size " + receivedBuf.remainBytes() + "target Size " + targetSize);
                
				if (receivedBuf.remainBytes () >= targetSize) {
					//    UnityEngine.Debug.LogError("write the Receive Message!!!!    Begin");
                    
					ctx.fireRead (receivedBuf);
					//  UnityEngine.Debug.LogError("write the Receive Message!!!!    End");
					receivedBuf.DiscardReadedBytes ();
					// UnityEngine.Debug.LogError("Deal after  Receive Message!!!! now size " + receivedBuf.remainBytes());
					targetSize = 0;
				}
                
                
				//   System.Console.WriteLine("TargetSize " + targetSize);
				if (inputBuf.remainBytes () == 0) {
					break;
				}
			}
			


		}

	
	}
}

