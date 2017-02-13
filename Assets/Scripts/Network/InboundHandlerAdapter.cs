using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClientNetwork
{
	public class InboundHandlerAdapter : ConnectionInboundHandler
	{
		public virtual void active (ConnectionHandlerContext ctx)
		{
            
		}

		public virtual void handlerAdded (Connection connection)
		{
            
		}

		public virtual void handlerRemoved (Connection connection)
		{
            
		}

		public virtual void deactive (ConnectionHandlerContext ctx)
		{
            
		}

		public virtual void read (ConnectionHandlerContext ctx, object msg)
		{
			throw new NotImplementedException ();
		}

		public virtual void readComplete (ConnectionHandlerContext ctx)
		{
            
		}

      
	}
}
