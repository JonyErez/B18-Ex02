using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	internal class GamePiece
	{
		private readonly Player m_Owner;
		private char m_Symbol;
		private bool m_IsKing = false;
		private Point m_Location;

		internal GamePiece(Player i_Owner, Point i_Location)
		{
			m_Owner = i_Owner;
			m_Symbol = i_Owner.GamePieceSymbol;
			m_Location = i_Location;
			i_Owner.AddGamePiece(this);
		}

		internal Player Owner
		{
			get
			{
				return m_Owner;
			}
		}
		
		internal char Symbol
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

		internal bool IsKing
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

		internal Point Location
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

		internal void MakeKing()
		{
			m_IsKing = true;
			m_Symbol = m_Owner.KingSymbol;
		}
    }
}
