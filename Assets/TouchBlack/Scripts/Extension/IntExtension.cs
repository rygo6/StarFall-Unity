using UnityEngine;
using System.Collections;

static public class IntExtension 
{

	static public int NoNegative(this int value)
	{
		if (value < 0)
		{
			value = 0;
		}
		return value;
	}
	
}
