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
using System.Net;

namespace ClientNetwork
{
	public class ConnectionBuilder
	{
		public Action<ConnectionPipeline> actionAddHandler;
		public ConnectionBuilder()
		{
            _ipAdress = IPAddress.Parse("127.0.0.1");
			port = 8888;
		}

		public ConnectionBuilder socketAddress(string ip, int port)
		{
			this.Ip = ip;
			this.port = port;
			return this;
		}
		public ConnectionBuilder addAddHandlerAction(Action<ConnectionPipeline> actionAdd)
		{
			this.actionAddHandler = actionAdd;
			return this;
		}
		
		public string Ip {
			get
			{
			    if (_ipAdress == null) return null;
			    return _ipAdress.ToString();
			}
		    set
		    {
		        IPAddress[] ipAdresses = null;
		        try
		        {
		            ipAdresses = Dns.GetHostAddresses(value);
		            _ipAdress = ipAdresses[0];
                    
		        }
		        catch (Exception exp)
		        {
		            UnityEngine.Debug.LogException(exp);
		            _ipAdress = null;
		        }
                
		    }
		}
		
	    private IPAddress _ipAdress = null;

		public int Port {
			get {
				return port;
			}
		}

		int port;
	    public bool robotmode = false;
		public Connection build()
		{
			Connection connect = null;
            connect = new Connection(robotmode);
			ConnectionPipeline pipeLine = new ConnectionPipeline(connect);
			connect.setPipeline(pipeLine);
			if (actionAddHandler != null) {
				actionAddHandler(pipeLine);
			}
			connect.Ip = _ipAdress;
			connect.Port = port;
			return connect;
		}

	}
}
