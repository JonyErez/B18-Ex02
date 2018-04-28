using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	public class Point
	{
		private int x;
		private int y;

		public Point()
		{
			x = 0;
			y = 0;
		}

		public Point(int i_x, int i_y)
		{
			X = i_x;
			Y = i_y;
		}

		public int X
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}

		public int Y
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}

		public void UpdateCoordinates(int i_x, int i_y)
		{
			X = i_x;
			Y = i_y;
		}
	}
}
