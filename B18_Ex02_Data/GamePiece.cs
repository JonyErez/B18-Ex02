using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	internal class GamePiece
	{
		private readonly	Player	r_Owner;
		private				char	m_Symbol;
		private				bool	m_IsKing = false;
		private				Point	m_Location;

		public			GamePiece(Player i_Owner, Point i_Location)
		{
			r_Owner = i_Owner;
			m_Symbol = i_Owner.GamePieceSymbol;
			m_Location = i_Location;
			i_Owner.AddGamePiece(this);
		}

		public	Player	Owner
		{
			get
			{
				return r_Owner;
			}
		}

		public	char	Symbol
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

		public	bool	IsKing
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

		public	void	MakeKing()
		{
			m_IsKing = true;
			m_Symbol = r_Owner.KingSymbol;
		}
    }
}
