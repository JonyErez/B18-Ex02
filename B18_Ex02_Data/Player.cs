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
		private readonly char m_GamePieceSymbol;
		private readonly char m_KingSymbol;

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
