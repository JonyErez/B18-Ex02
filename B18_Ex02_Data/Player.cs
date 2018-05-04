using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	internal class Player
	{
		private	readonly	string			r_Name;
		private readonly	char			r_GamePieceSymbol;
		private readonly	char			r_KingSymbol;
		private readonly	eDirection		r_Direction;
		private readonly	bool			r_IsComputer;
		private				List<GamePiece>	m_GamePieces = new List<GamePiece>();
		private				uint			m_Score = 0;

		internal	enum			eDirection
		{
			DOWN = 1,
			UP = -1,
			LEFT = -1,
			RIGHT = 1
		}

		public						Player(string i_Name, char i_GamePieceSymbol, char i_KingSymbol, eDirection i_Direction, bool i_IsComputer)
		{
			r_Name = i_Name;
			r_GamePieceSymbol = i_GamePieceSymbol;
			r_KingSymbol = i_KingSymbol;
			r_Direction = i_Direction;
			r_IsComputer = i_IsComputer;
		}

		public		eDirection		Direction
		{
			get
			{
				return r_Direction;
			}
		}

		public		eDirection		ReverseDirection
		{
			get
			{
				eDirection reverseDirection;
				if (r_Direction == eDirection.DOWN)
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

		public		List<GamePiece>	GamePieces
		{
			get
			{
				return m_GamePieces;
			}
		}

		public		string			Name
		{
			get
			{
				return r_Name;
			}
		}

		public		uint			Score
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

		public		char			GamePieceSymbol
		{
			get
			{
				return r_GamePieceSymbol;
			}
		}

		public		char			KingSymbol
		{
			get
			{
				return r_KingSymbol;
			}
		}

		public		void			AddGamePiece(GamePiece i_NewPiece)
		{
			m_GamePieces.Add(i_NewPiece);
		}

		public		void			RemoveGamePiece(GamePiece i_PieceToRemove)
		{
			m_GamePieces.Remove(i_PieceToRemove);
		}

		public		bool			IsComputer
		{
			get
			{
				return r_IsComputer;
			}
		}
	}
}
