using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

using ClientNetwork;
using autodemo;
public class ClientConnection
{
    


    public ClientConnection(NotifyManager notify)
    {
        notifyManager = notify;
    }
		
    
    public bool Connect(String ip, int port)
    {
        return Connect(ip, port, false);
    }
    public bool Connect(String ip, int port,bool robot)
    {
     

        
        ConnectionBuilder builder = new ConnectionBuilder();
        builder.robotmode = robot;
        builder.socketAddress(ip, port);

        builder.addAddHandlerAction((ConnectionPipeline pipeLine) =>
        {
            pipeLine.addLast("prelinedecoder", new LengthFieldBasedFrameDecoder());
            pipeLine.addLast("protobufdecoder",
					new ProtobufDecoder<ImageMsg>((ImageMsg msg) =>
                {
                    


                    notifyManager.PushMessage(msg);
                }));
            pipeLine.addLast("prelineencoder", new LengthFieldPrepender());
				pipeLine.addLast("probufencoder", new ProtobufEncoder<ImageMsg>());
        });
        connect = builder.build();
        
        return connect.Connect();
    }





    Connection connect;
    NotifyManager notifyManager;
    string username;

    public NotifyManager getNotifyManager()
    {
        return notifyManager;
    }

    public Connection getConnection()
    {
        return connect;
    }
  


	public void SendImage(ImageMsg msg)
	{
		if (connect == null || !connect.isConnected)
			return;
		connect.writeObject(msg);

	}
    public void Disconnect()
    {
        if (connect != null)
            connect.Close();
    }
}