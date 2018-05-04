using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	internal class PieceMove
	{
		private readonly	bool	r_DoesEat;
		private				Point	m_Location;
		private				Point	m_Destination;

		public			PieceMove(Point i_Location, Point i_Destination, bool i_DoesEat)
		{
			m_Location = i_Location;
			m_Destination = i_Destination;
			r_DoesEat = i_DoesEat;
		}

		public	Point	Location
		{
			get
			{
				return m_Location;
			}

			set
			{
				m_Location = value;
			}
		}

		public	Point	Destination
		{
			get
			{
				return m_Destination;
			}

			set
			{
				m_Destination = value;
			}
		}

		public	bool	DoesEat
		{
			get
			{
				return r_DoesEat;
			}
		}
	}
}
