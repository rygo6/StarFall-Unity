[System.Serializable]
public class Vector2Int 
{

	#region Properties

	public int x;
	
	public int y;
	
	#endregion
	
	#region LifeCycle
	
	public Vector2Int()
	{
		
	}
	
	public Vector2Int( int x, int y )
	{
		this.x = x;
		this.y = y;
	}
	
	#endregion
	
	#region object
	
	public override string ToString()
	{
		return x.ToString() + ", " + y.ToString();
	}
	
	#endregion
	
}