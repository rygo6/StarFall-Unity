using UnityEngine;
using System.Collections;

public class Finger : MonoBehaviour 
{

	#region Properties
	
	public ComponentCache componentCache 
	{ 
		get 
		{ 
			if (m_ComponentCache == null)
			{
				m_ComponentCache = new ComponentCache(gameObject);
			}
			return m_ComponentCache; 
        }
    }
    private ComponentCache m_ComponentCache;
    
    #endregion

}
