using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClientNetwork
{
	public class OutboundHandlerAdapter : ConnectionOutboundHandler
	{
		public virtual void active (ConnectionHandlerContext ctx)
		{
            
		}

		public virtual void deactive (ConnectionHandlerContext ctx)
		{
            
		}

		public virtual void handlerAdded (Connection connection)
		{
        
		}

		public virtual void handlerRemoved (Connection connection)
		{
        
		}

        

		public virtual void write (ConnectionHandlerContext ctx, object msg)
		{
			throw new NotImplementedException ();
		}
	}
}
