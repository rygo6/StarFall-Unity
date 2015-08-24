using UnityEngine;
using System.Collections;

public class AttachBoxCollider2D : MonoBehaviourBase
{

	private BoxCollider2D boxCollider2D
	{
		get { return m_BoxCollider2D; }
	}
	[SerializeField]
	private BoxCollider2D m_BoxCollider2D;

	private void Update()
	{
		UpdateBoxCollider2D();
	}

	private void UpdateBoxCollider2D()
	{
		componentCache.rectTransform.GetWorldCorners(m_cornerArray);
		boxCollider2D.size = new Vector2(m_cornerArray [2].x - m_cornerArray [0].x, m_cornerArray [2].y - m_cornerArray [0].y);
		boxCollider2D.transform.position = componentCache.rectTransform.position;
	}
	private Vector3[] m_cornerArray = new Vector3[4];

}
