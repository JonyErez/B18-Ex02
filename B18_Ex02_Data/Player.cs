using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	internal class Player
	{
		private List<GamePiece> m_GamePieces = new List<GamePiece>();
		private uint m_Score = 0;
		private readonly string k_Name;
		private readonly char k_GamePieceSymbol;
		private readonly char k_KingSymbol;
		private readonly eDirection m_Direction;
		private readonly bool k_IsComputer;

		internal enum eDirection {DOWN = 1, UP = -1, LEFT = -1, RIGHT = 1};

		public Player(string i_Name, char i_GamePieceSymbol, char i_KingSymbol, eDirection i_Direction, bool i_IsComputer)
		{
			k_Name = i_Name;
			k_GamePieceSymbol = i_GamePieceSymbol;
			k_KingSymbol = i_KingSymbol;
			m_Direction = i_Direction;
			k_IsComputer = i_IsComputer;
		}

		public eDirection Direction
		{
			get
			{
				return m_Direction;
			}
		}

		public eDirection ReverseDirection
		{
			get
			{
				eDirection reverseDirection;
				if (m_Direction == eDirection.DOWN)
				{
					reverseDirection = eDirection.UP;
				}
				else
				{
					reverseDirection = eDirection.DOWN;
				}
				return reverseDirection;
			}
		}

		public List<GamePiece> GamePieces
		{
			get
			{
				return m_GamePieces;
			}
		}

		public string Name
		{
			get
			{
				return k_Name;
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

		public char GamePieceSymbol
		{
			get
			{
				return k_GamePieceSymbol;
			}
		}

		public char KingSymbol
		{
			get
			{
				return k_KingSymbol;
			}
		}

		public void AddGamePiece(GamePiece i_NewPiece)
		{
			m_GamePieces.Add(i_NewPiece);
		}

		public void RemoveGamePiece(GamePiece i_PieceToRemove)
		{
			m_GamePieces.Remove(i_PieceToRemove);
		}
	}
}
