using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	internal class Player
	{
		private List<GamePiece> m_GamePieces = new List<GamePiece>();
		private uint m_Score = 0;
		private string m_Name;
		private readonly char k_GamePieceSymbol;
		private readonly char k_KingSymbol;

		public Player(string i_Name, char i_GamePieceSymbol, char i_KingSymbol)
		{
			m_Name = i_Name;
			k_GamePieceSymbol = i_GamePieceSymbol;
			k_KingSymbol = i_KingSymbol;
		}

		public string Name
		{
			get
			{
				return m_Name;
			}

			set
			{
				m_Name = value;
			}
		}

		public uint Score
		{
			get
			{
				return m_Score;
			}
			set
			{
				m_Score = value;
			}
		}
	}
}
