using UnityEngine;
using System.Collections;

public class AttachBoxCollider2D : MonoBehaviour
{
	private BoxCollider2D _boxCollider2D;

	private readonly Vector3[] _cornerArray = new Vector3[4];

	private RectTransform _rectTransform;

	private void Awake()
	{
		_boxCollider2D = GetComponentInChildren<BoxCollider2D>();
		_rectTransform = GetComponent<RectTransform>();
	}

	private void Update()
	{
		UpdateBoxCollider2D();
	}

	private void UpdateBoxCollider2D()
	{
		_rectTransform.GetWorldCorners(_cornerArray);
		_boxCollider2D.size = new Vector2(_cornerArray [2].x - _cornerArray [0].x, _cornerArray [2].y - _cornerArray [0].y);
		_boxCollider2D.transform.position = _rectTransform.position;
	}
}
