using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	internal class GamePiece
	{
		private readonly Player m_BelongsTo;
		private char m_Symbol;
		private bool m_IsKing;
		private Point m_Location;

		public char Symbol
		{
			get
			{
				return m_Symbol;
			}

			set
			{
				m_Symbol = value;
			}
		}
		public bool IsKing
		{
			get
			{
				return m_IsKing;
			}
			set
			{
				m_IsKing = value;
			}
		}
		public Point Location
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
    }
}
