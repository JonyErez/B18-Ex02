using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	internal class GameBoard
	{
		private readonly int m_BoardSize;
		private GamePiece[,] m_Board;

		public GameBoard(int i_BoardSize)
		{
			m_BoardSize = i_BoardSize;
			m_Board = new GamePiece[i_BoardSize, i_BoardSize];
		}

		//public List<PieceMove> FindPossibleMoves(GamePiece i_GamePiece)
		//{
		//	List<PieceMove> currentPiecePossibleMoves = new List<PieceMove>(4);
		//	currentPiecePossibleMoves.AddRange(findPossibleEatingMoves(i_GamePiece));
		//	if(currentPiecePossibleMoves.Count == 0)
		//	{
		//		currentPiecePossibleMoves.AddRange(findPossibleSteppingForwardMoves(i_GamePiece));
		//	}
		//	return currentPiecePossibleMoves;
		//}

		public List<PieceMove> findPossibleSteppingForwardMoves(GamePiece i_GamePiece)
		{
			List<PieceMove> possibleSteppingForwardMoves = new List<PieceMove>(2);
			if(i_GamePiece.IsKing)
			{
				possibleSteppingForwardMoves.AddRange(findSteppingForwardMoves(i_GamePiece, i_GamePiece.Owner.ReverseDirection));
			}
			possibleSteppingForwardMoves.AddRange(findSteppingForwardMoves(i_GamePiece, i_GamePiece.Owner.Direction));

			return possibleSteppingForwardMoves;
		}

		private List<PieceMove> findSteppingForwardMoves(GamePiece i_GamePiece, Player.eDirection i_VerticalDirection)
		{
			List<PieceMove> currentPieceSteppingForward = new List<PieceMove>(2);
			PieceMove currentPieceSteppingLeft = findSpecificSteppingMove(i_GamePiece.Location, i_VerticalDirection, Player.eDirection.LEFT);
			PieceMove currentPieceSteppingRight = findSpecificSteppingMove(i_GamePiece.Location, i_VerticalDirection, Player.eDirection.RIGHT);

			if(currentPieceSteppingLeft != null)
			{
				currentPieceSteppingForward.Add(currentPieceSteppingLeft);
			}
			if (currentPieceSteppingRight != null)
			{
				currentPieceSteppingForward.Add(currentPieceSteppingRight);
			}

			return currentPieceSteppingForward;
		}

		private PieceMove findSpecificSteppingMove(Point i_Location, Player.eDirection i_VerticalDirection, Player.eDirection i_HorizontalDirection)
		{
			PieceMove steppingMove = null;
			int row = i_Location.Y + (int)i_VerticalDirection;
			int col = i_Location.X + (int)i_HorizontalDirection;

			if(isCoordinateInBoard(row, col) && getSquareOwnership(row, col) == null)
			{
				Point destination = new Point(col, row);
				steppingMove = new PieceMove(i_Location, destination);
			}
			return steppingMove;
		}

		public List<PieceMove> findPossibleEatingMoves(GamePiece i_GamePiece)
		{
			List<PieceMove> possibleEatingMoves = new List<PieceMove>(2);
			if(i_GamePiece.IsKing)
			{
				possibleEatingMoves.AddRange(findEatingMoves(i_GamePiece, i_GamePiece.Owner.ReverseDirection));
			}
			possibleEatingMoves.AddRange(findEatingMoves(i_GamePiece, i_GamePiece.Owner.Direction));

			return possibleEatingMoves;
		}

		private List<PieceMove> findEatingMoves(GamePiece i_Piece, Player.eDirection i_VerticalDirection)
		{
			List<PieceMove> currentPieceEatingMoves = new List<PieceMove>(2);
			PieceMove currentPieceEatingLeft = findSpecificEatingMoves(i_Piece, i_VerticalDirection, Player.eDirection.LEFT);
			PieceMove currentPieceEatingRight = findSpecificEatingMoves(i_Piece, i_VerticalDirection, Player.eDirection.RIGHT);

			if (currentPieceEatingLeft != null)
			{
				currentPieceEatingMoves.Add(currentPieceEatingLeft);
			}
			if (currentPieceEatingRight != null)
			{
				currentPieceEatingMoves.Add(currentPieceEatingRight);
			}

			return currentPieceEatingMoves;
		}

		private PieceMove findSpecificEatingMoves(GamePiece i_Piece, Player.eDirection i_VerticalDirection, Player.eDirection i_HorizontalDirection)
		{
			PieceMove EatingMove = null;
			int pieceToEatRow = i_Piece.Location.Y + (int)i_VerticalDirection;
			int pieceToEatCol = i_Piece.Location.X + (int)i_HorizontalDirection;
			if (isCoordinateInBoard(pieceToEatRow, pieceToEatCol))
			{
				Player leftSquareOwner = getSquareOwnership(pieceToEatRow, pieceToEatCol);
				if (leftSquareOwner != null && leftSquareOwner != i_Piece.Owner)
				{
					int squareToJumpToRow = pieceToEatRow + (int)i_VerticalDirection;
					int squareToJumpToCol = pieceToEatCol + (int)i_HorizontalDirection;
					if (isCoordinateInBoard(squareToJumpToRow, squareToJumpToCol))
					{
						if (getSquareOwnership(squareToJumpToRow, squareToJumpToCol) == null)
						{
							Point destination = new Point(squareToJumpToCol, squareToJumpToRow);
							EatingMove = new PieceMove(i_Piece.Location, destination);
						}
					}
				}
			}
			return EatingMove;
		}













		public bool isFirstActionLegal(GamePiece i_Piece, Point i_NewLocation, out bool o_HasEaten, out GamePiece o_EatenPiece)
		{
			bool isLegalMove = false;
			bool isLegalEat = false;
			o_EatenPiece = null;
			o_HasEaten = false;

			if (i_Piece.IsKing)
			{
				isLegalMove = isAllowedStepByInput(i_Piece.Location, i_Piece.Owner.ReverseDirection, i_NewLocation);
				isLegalEat = isAllowedEatByInput(i_Piece, i_Piece.Owner.ReverseDirection, i_NewLocation, out o_EatenPiece);
			}
			isLegalMove = isLegalMove || isAllowedStepByInput(i_Piece.Location, i_Piece.Owner.Direction, i_NewLocation);
			isLegalEat = isLegalEat || isAllowedEatByInput(i_Piece, i_Piece.Owner.Direction, i_NewLocation, out o_EatenPiece);
			o_HasEaten = isLegalEat;
			return isLegalEat || isLegalMove;
		}

		public bool isContinuationActionLegal(GamePiece i_Piece, Point i_NewLocation, out GamePiece o_EatenPiece)
		{
			bool isLegalEat = false;
			o_EatenPiece = null;

			if (i_Piece.IsKing)
			{
				isLegalEat = isAllowedEatByInput(i_Piece, i_Piece.Owner.ReverseDirection, i_NewLocation, out o_EatenPiece);
			}
			isLegalEat = isLegalEat || isAllowedEatByInput(i_Piece, i_Piece.Owner.Direction, i_NewLocation, out o_EatenPiece);
			return isLegalEat;
		}

		private bool isAllowedStepByInput(Point i_CurrentLocation, Player.eDirection i_VerticalDirection, Point i_NewLocation)
		{
			bool isAllowed = false;
			if (isCoordinateInBoard(i_NewLocation.Y, i_NewLocation.X))
			{
				if (getSquareOwnership(i_NewLocation.Y, i_NewLocation.X) == null)
				{
					Point checkIfAllowed = new Point(i_CurrentLocation.X, i_CurrentLocation.Y);
					checkIfAllowed.Y += (int)i_VerticalDirection;
					if (checkIfAllowed.Y == i_NewLocation.Y)
					{
						isAllowed = checkIfAllowed.X + (int)Player.eDirection.LEFT == i_NewLocation.X 
								|| checkIfAllowed.X + (int)Player.eDirection.RIGHT == i_NewLocation.X;
					}
				}
			}
			return isAllowed;
		}

		private bool isAllowedEatByInput(GamePiece i_Piece, Player.eDirection i_VerticalDirection, Point i_NewLocation, out GamePiece o_EatenPiece)
		{
			bool isAllowed = false;
			o_EatenPiece = null;

			if (isCoordinateInBoard(i_NewLocation.Y, i_NewLocation.X))
			{
				if (getSquareOwnership(i_NewLocation.Y, i_NewLocation.X) == null)
				{
					int rowDiff = i_NewLocation.Y - i_Piece.Location.Y;
					int colDiff = i_NewLocation.X - i_Piece.Location.X;
					if (Math.Abs(rowDiff) == 2 && Math.Abs(colDiff) == 2)
					{
						rowDiff /= 2;
						if (rowDiff == (int)i_VerticalDirection)
						{
							colDiff /= 2;
							int eatenSquareRow = i_Piece.Location.Y + rowDiff;
							int eatenSquareCol = i_Piece.Location.X + colDiff;
							if (getSquareOwnership(eatenSquareRow, eatenSquareCol) != null)
							{
								if (getSquareOwnership(eatenSquareRow, eatenSquareCol) != i_Piece.Owner)
								{
									o_EatenPiece = m_Board[eatenSquareRow, eatenSquareCol];
									isAllowed = true;
								}
							}
						}
					}
				}
			}
			return isAllowed;
		}

		public bool hasLegalEatingMoves(GamePiece i_Piece)
		{
			bool hasLegalEatingMove = false;
			if (i_Piece.IsKing)
			{
				hasLegalEatingMove = checkIfManCanEat(i_Piece, i_Piece.Owner.ReverseDirection);
			}
			hasLegalEatingMove = hasLegalEatingMove || checkIfManCanEat(i_Piece, i_Piece.Owner.Direction);
			return hasLegalEatingMove;
		}

		public bool hasLegalMoves(GamePiece i_Piece, out bool o_CanEat)
		{
			bool hasLegalMove = false;
			bool canKingEat = false;
			bool canManEat = false;
			if(i_Piece.IsKing)
			{
				hasLegalMove = canManMoveForward(i_Piece, i_Piece.Owner.ReverseDirection, out canKingEat);
			}
			hasLegalMove = hasLegalMove || canManMoveForward(i_Piece, i_Piece.Owner.Direction, out canManEat);
			o_CanEat = canManEat || canKingEat;
			return hasLegalMove;

		}

		private bool canManMoveForward(GamePiece i_Piece, Player.eDirection i_VerticalDirection, out bool o_CanEat)
		{
			bool canMove = false;
			canMove = o_CanEat = checkIfManCanEat(i_Piece, i_VerticalDirection);
			canMove = canMove || checkManSteppingForwardMoves(i_Piece, i_VerticalDirection);
			return canMove;
		}

		private bool checkIfManCanEat(GamePiece i_Piece, Player.eDirection i_VerticalDirection)
		{
			return checkIfCurrentPieceCanEat(i_Piece, i_VerticalDirection, Player.eDirection.LEFT) ||
					checkIfCurrentPieceCanEat(i_Piece, i_VerticalDirection, Player.eDirection.RIGHT);
		}

		private bool checkIfCurrentPieceCanEat(GamePiece i_Piece, Player.eDirection i_VerticalDirection, Player.eDirection i_HorizontalDirection)
		{
			int pieceToEatRow = i_Piece.Location.Y + (int)i_VerticalDirection;
			int pieceToEatCol = i_Piece.Location.X + (int)i_HorizontalDirection;
			bool canEat = false;
			if (isCoordinateInBoard(pieceToEatRow, pieceToEatCol))
			{
				Player leftSquareOwner = getSquareOwnership(pieceToEatRow, pieceToEatCol);
				if (leftSquareOwner != null && leftSquareOwner != i_Piece.Owner)
				{
					int squareToJumpToRow = pieceToEatRow + (int)i_VerticalDirection;
					int squareToJumpToCol = pieceToEatCol + (int)i_HorizontalDirection;
					if (isCoordinateInBoard(squareToJumpToRow, squareToJumpToCol))
					{
						if (getSquareOwnership(squareToJumpToRow, squareToJumpToCol) == null)
						{
							canEat = true;
						}
					}
				}
			}
			return canEat;
		}

		private bool checkManSteppingForwardMoves(GamePiece i_Piece, Player.eDirection i_VerticalDirection)
		{
			return checkIfPieceCanStepForward(i_Piece.Location, i_VerticalDirection, Player.eDirection.RIGHT) || 
					checkIfPieceCanStepForward(i_Piece.Location, i_VerticalDirection, Player.eDirection.LEFT);
		}

		private bool checkIfPieceCanStepForward(Point i_Location, Player.eDirection i_VerticalDirection, Player.eDirection i_HorizontalDirection)
		{
			int row = i_Location.Y + (int)i_VerticalDirection;
			int col = i_Location.X + (int)i_HorizontalDirection;

			return isCoordinateInBoard(row,col) && getSquareOwnership(row, col) == null;
		}

		private Player getSquareOwnership(int i_Row, int i_Col)
		{
			Player owner = null;
			if (m_Board[i_Row, i_Col] != null)
			{
				owner = m_Board[i_Row, i_Col].Owner;
			}
			return owner;
		}

		private bool isCoordinateInBoard(int i_Row, int i_Col)
		{
			return !(i_Col < 0 || i_Col >= m_BoardSize || i_Row < 0 || i_Row >= m_BoardSize);
		}

		public void InitializeBoard(Player i_PlayerOne, Player i_PlayerTwo)
		{
			int howManyRows = (m_BoardSize / 2) - 1;
			initializePlayerPieces(i_PlayerOne, 0, howManyRows);
			initializePlayerPieces(i_PlayerTwo, (m_BoardSize / 2) + 1, howManyRows);
		}

		private void initializePlayerPieces(Player i_Player, int i_StartingRow, int i_HowManyRows)
		{
			for(int currentRow = i_StartingRow; currentRow < i_StartingRow + i_HowManyRows; currentRow++)
			{
				int currentCol = currentRow % 2 == 0 ? 1 : 0;
				for(; currentCol < m_BoardSize; currentCol += 2)
				{
					m_Board[currentRow, currentCol] = new GamePiece(i_Player, new Point(currentCol, currentRow));
				}
			}
		}

		public int Size
		{
			get
			{
				return m_BoardSize;
			}
		}

		public GamePiece[,] Board
		{
			get
			{
				return m_Board;
			}
		}

		public char GetSymbol(Point i_Coordinates)
		{
			char symbol = ' ';
			if (m_Board[i_Coordinates.X, i_Coordinates.Y] != null)
			{
				symbol = m_Board[i_Coordinates.X, i_Coordinates.Y].Symbol;
			}
			return symbol;
		}
	}
}
