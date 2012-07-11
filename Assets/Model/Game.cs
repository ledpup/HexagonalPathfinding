using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Game
    {
        public Tile[,] GameBoard;
        public IEnumerable<GamePiece> GamePieces;

        public int Width;
        public int Height;

        public Game(int width, int height)
        {
            Width = width;
            Height = height;

            InitialiseGameBoard();
            BlockOutTiles();
            
            InitialiseGamePieces();
        }

        private void InitialiseGamePieces()
        {
            var gamePieces = new List<GamePiece>
                                 {
                                     new GamePiece(new Point(0, 0)),
                                     new GamePiece(new Point(Width - 1, Height - 1))
                                 };
            GamePieces = gamePieces;
        }

        private void InitialiseGameBoard()
        {
            GameBoard = new Tile[Width, Height];

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    GameBoard[x, y] = new Tile(x, y);
                }
            }

            AllTiles.ToList().ForEach(o => o.FindNeighbours(GameBoard));
        }

        private void BlockOutTiles()
        {
            GameBoard[2, 5].CanPass = false;
            GameBoard[2, 4].CanPass = false;
            GameBoard[2, 2].CanPass = false;
            GameBoard[3, 2].CanPass = false;
            GameBoard[4, 5].CanPass = false;
            GameBoard[5, 5].CanPass = false;
            GameBoard[5, 3].CanPass = false;
            GameBoard[5, 2].CanPass = false;
        }

        public IEnumerable<Tile> AllTiles
        {
            get
            {
                for (var x = 0; x < Width; x++)
                    for (var y = 0; y < Height; y++)
                        yield return GameBoard[x, y];
            }
        }

    }
}

