using UnityEngine;
using System.Collections;

public class ComponentCache  
{

	#region Properties

	private readonly GameObject m_GameObject;
	
	public Renderer renderer 
	{ 
		get 
		{ 
			if (m_Renderer == null)
			{
				m_Renderer = m_GameObject.GetComponent<Renderer>();
			}
			return m_Renderer; 
		}
	}
	private Renderer m_Renderer;
	
	public SpriteRenderer spriteRenderer 
	{ 
		get 
		{ 
			if (m_SpriteRenderer == null)
			{
				m_SpriteRenderer = m_GameObject.GetComponent<SpriteRenderer>();
			}
			return m_SpriteRenderer; 
		}
	}
	private SpriteRenderer m_SpriteRenderer;
	
	public Camera camera 
	{ 
		get 
		{ 
			if (m_Camera == null)
			{
				m_Camera = m_GameObject.GetComponent<Camera>();
			}
			return m_Camera; 
		}
	}
	private Camera m_Camera;	
	
	public RectTransform rectTransform 
	{ 
		get 
		{ 
			if (m_RectTransform == null)
			{
				m_RectTransform = m_GameObject.GetComponent<RectTransform>();
			}
			return m_RectTransform; 
		}
	}
	private RectTransform m_RectTransform;	

	public CircleCollider2D circleCollider2D 
	{ 
		get 
		{ 
			if (m_CircleCollider2D == null)
			{
				m_CircleCollider2D = m_GameObject.GetComponent<CircleCollider2D>();
			}
			return m_CircleCollider2D; 
		}
	}
	private CircleCollider2D m_CircleCollider2D;

	public BoxCollider2D boxCollider2D 
	{ 
		get 
		{ 
			if (m_BoxCollider2D == null)
			{
				m_BoxCollider2D = m_GameObject.GetComponent<BoxCollider2D>();
			}
			return m_BoxCollider2D; 
		}
	}
	private BoxCollider2D m_BoxCollider2D;			
	
	public Rigidbody2D rigidBody2D 
	{ 
		get 
		{ 
			if (m_RigidBody2D == null)
			{
				m_RigidBody2D = m_GameObject.GetComponent<Rigidbody2D>();
			}
			return m_RigidBody2D; 
		}
	}
	private Rigidbody2D m_RigidBody2D;	
	
	#endregion
	
	#region LifeCycle
	
	public ComponentCache(GameObject sourceGameobject)
	{
		m_GameObject = sourceGameobject;
	}
	
	#endregion
	
}
