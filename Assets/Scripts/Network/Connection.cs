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
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.Net;


namespace ClientNetwork
{
	public class Connection
	{
		ConnectionPipeline pipeLine;
		Socket clientSocket;
		Thread readThread;
		Thread writeThread;
	    private ByteBuf recvBuf;
	    private int timeOut;
		bool connected = false;
		Queue dataToWriteList;
	    private float PingTime = 30.0f;
	    private Action PingProc = null;
	    private bool RobotMode = false;
	    public void SetPingProc(Action pingProc)
	    {
	        PingProc = pingProc;
	    }
		public Connection (bool robotmode)
		{
		    RobotMode = robotmode;
			connected = false;
		    timeOut = 8000;
            recvBuf = ByteBuf.allocOne(65536 * 10);
		}
		public void setPipeline (ConnectionPipeline pipe)
		{
			this.pipeLine = pipe;
		}

	    public void setTimeOut(int timeout)
	    {
	        this.timeOut = timeout;
	    }
		public void  writeObject (object message)
		{
			lock (this) {
				dataToWriteList.Enqueue (message);
			}
			
		}
        public bool Connect ()
		{
			if (connected)
				return true;
            if (_ipAddress == null)
            {
                return false;
            }
			
			IPEndPoint tcpaddress = new IPEndPoint (_ipAddress, port);
			clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			clientSocket.ReceiveTimeout = 8000;
            
			//Todo: Add the port ,make the connect valid
			try {
#if UNITY_WEBPLAYER
               clientSocket.Connect (tcpaddress);
#else
			    IAsyncResult result = clientSocket.BeginConnect(tcpaddress, null, null);
			    result.AsyncWaitHandle.WaitOne(timeOut, true);
#endif
			    if (!clientSocket.Connected)
			    {
                    connected = false;
                    clientSocket = null;
			        return false;
			    }


			} catch (SocketException) {
				connected = false;
				clientSocket = null;
				return false;
				
			}

			bool bOK = clientSocket.Connected;

			if (bOK  ) {
				pipeLine.fireActive ();
				connected = true;
                dataToWriteList = new Queue();
			    if (!RobotMode)
			    {
                    
                    //Todo: Add the read and write thread
                    readThread = new Thread(this.readThreadProc);
                    readThread.IsBackground = true;
                    readThread.Start();
                    writeThread = new Thread(this.writeThreadProc);
                    writeThread.Start();
                    writeThread.IsBackground = true;    
			    }
				
			}
			
			return bOK;

		}
		public void Close ()
		{
		
            if (clientSocket != null)
		    {
                connected = false;
		        try
		        {
                    clientSocket.Shutdown(SocketShutdown.Both);
		        }
		        catch (Exception)
		        {
		           
		        }
		        
                clientSocket.Close();
		        clientSocket = null;
		    }
            connected = false;
			

		}
		public bool isConnected {
			get {
				return connected;
			}
		}

	    private IPAddress _ipAddress;
		public IPAddress Ip {
		    get
		    {
		        return _ipAddress;
		    }
		    set
		    {
		        _ipAddress = value;
		    }
		}
		public int Port {
			get {
				return this.port;
			}
			set {
				port = value;
			}
		}
		int port;
		public Socket getClientSocket ()
		{
			return clientSocket;
		}

		// the method must be called at io thread
	    private int PollTimeDuration = 1000*1000;
		void ReadMessage ()
		{

		    ByteBuf buf = recvBuf;
            buf.Clear();
			int bytes = 0;
			try {
                if (clientSocket.Poll(PollTimeDuration, SelectMode.SelectRead))
			    {
			        bytes = clientSocket.Available;
                    if (bytes == 0)
                    {

                        connected = false;
                        UnityEngine.Debug.LogError("the connection disconnected!");
                        pipeLine.fireDeactive();


                    }
                    else
                    {
                        bytes = clientSocket.Receive(buf.getBuffer());
                    }
			    }
                else if (clientSocket.Poll(PollTimeDuration, SelectMode.SelectError))
                {
                    pipeLine.fireDeactive();
                    UnityEngine.Debug.Log("SelectError, the connection disconnected!");
                    

                }
				
    
			} catch (SocketException exp) {
				{

                    UnityEngine.Debug.LogException(exp);
				}


			}
            //UnityEngine.Debug.LogError("Sleep ......");
            Thread.Sleep(100);
			if (bytes > 0) {
				
				buf.setWriteIndex (bytes);
				pipeLine.fireRead (buf);
				pipeLine.readComplete ();
			}


		}

		
		void writeThreadProc ()
		{
		    long lastTicks = DateTime.Now.Ticks;
			while (this.isConnected) {
                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - lastTicks);
                if (timeSpan.TotalSeconds > PingTime)
                {
                    lastTicks = DateTime.Now.Ticks;
			        if (PingProc != null)
			        {
			            PingProc();
			        }
			    }
				object data = null;
				lock (this) {
					if (dataToWriteList.Count > 0) {
						data = dataToWriteList.Dequeue ();
					}
				}
				
				if (data != null) {
					pipeLine.write (data);
				} else {
					Thread.Sleep (100);
				}
                

				//if (nottimeout) {
					
			}

		}
		void readThreadProc ()
		{
			while (this.isConnected) {
				//Todo: Configure the time-out event
				ReadMessage ();

			}
			;
		
		}
        // 机器人模式专用， 所有的为1个线程
	    private long lastWritePingTicks = 0;
	    public bool UpdateLogic()
	    {
	        if (!connected) return false;
            // Check Receive
            ByteBuf buf = recvBuf;
            buf.Clear();
            int bytes = 0;
            try
            {
                if (clientSocket.Poll(0, SelectMode.SelectRead))
                {
                    bytes = clientSocket.Available;
                    if (bytes == 0)
                    {

                        connected = false;
                        pipeLine.fireDeactive();


                    }
                    else
                    {
                        bytes = clientSocket.Receive(buf.getBuffer());
                    }
                }
                


            }
            catch (SocketException exp)
            {
                {

                    UnityEngine.Debug.LogException(exp);
                }


            }
            
            if (bytes > 0)
            {
                buf.setWriteIndex(bytes);
                pipeLine.fireRead(buf);
                pipeLine.readComplete();
            }
	        if (!connected) return false;
            // 写 线程！！
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - lastWritePingTicks);
            if (timeSpan.TotalSeconds > PingTime)
            {
                lastWritePingTicks = DateTime.Now.Ticks;
                if (PingProc != null)
                {
                    PingProc();
                }
            }
            object data = null;
            lock (this)
            {
                if (dataToWriteList.Count > 0)
                {
                    data = dataToWriteList.Dequeue();
                }
            }

            if (data != null)
            {
                pipeLine.write(data);
            }
	        return connected;
	    }
	}
}

