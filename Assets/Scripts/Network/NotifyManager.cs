using UnityEngine;
using System.IO;
using ClientNetwork;
using autodemo;
using System;
using System.Collections.Generic;



public class NotifyManager : MonoBehaviour {


	SafeQueue<ImageMsg> mEventQueue = new SafeQueue<ImageMsg>();
    MessageActionTable messageActionTable = new MessageActionTable();
    
    void Start()
    {
        
    }

    

    public void clearForTriggers()
    {
        messageActionTable.Clear();   
    }
    public void initForTriggers()
    {
        clearForTriggers();
        
    }
    public void RegisterMessageTrigger<T>(Action<T> action,bool once)
    {
        messageActionTable.RegisterMessageTrigger(action, once);
    }
	// Update is called once per frame
	void Update () {

		ImageMsg info = null;
        info = mEventQueue.popObject();
        
        while (info != null)
        {
            onNotify(info);
            info = mEventQueue.popObject();
        }
	    
	}
	public void PushMessage(ImageMsg msg)
    {
        if (msg != null)
        {
            mEventQueue.pushObject(msg);
        }
        
    }
	void onNotify(ImageMsg msg)
    {
        if (msg != null)
        {
			messageActionTable.fireMessageTrigger(msg);     
            
            
        }
    }
}
