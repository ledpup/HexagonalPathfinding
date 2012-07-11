using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using Model;

public delegate void OnBoardChangeHandler(object sender);

public class BoardBehaviour : MonoBehaviour
{
    public GameObject Piece;
	public GameObject Tile;
    public GameObject Line;
    public GameObject SelectionObject;

	public int Width, Height;
    const float Spacing = 1.5f;

	GameObject[,] _gameBoard;
    Game _game;

    List<GameObject> _path;

    GamePiece _selectedPiece;
    GameObject _torus;


	void Start ()
	{
        CreateBoard();

        CreatePieces();

        transform.position = new Vector3(Width / 2.0f * Spacing - (Spacing / 2), Height / 2.0f * Spacing - (Spacing / 2), -(Width + Height) / 2 - 5);

        OnGameStateChanged();
        
        Messenger<TileBehaviour>.AddListener("Tile selected", OnTileSelected);
        Messenger<PieceBehaviour>.AddListener("Piece selected", OnPieceSelected);
	}

    private void DrawPath(IEnumerable<Tile> path)
    {
        if (_path == null)
            _path = new List<GameObject>();

        _path.ForEach(Destroy);
        _path = new List<GameObject>();
        path.ToList().ForEach(CreateLine);
    }

    void CreateLine(Tile tile)
    {
        var line = (GameObject)Instantiate(Line);
        line.transform.position = GetWorldCoordinates(tile.Location.X, tile.Location.Y, 1f);
        _path.Add(line);
    }

    void OnTileSelected(TileBehaviour tileBehaviour)
    {
        if (_selectedPiece == null)
            TileChanged(tileBehaviour);
        else
            MovePiece(tileBehaviour);
    }

    private void MovePiece(TileBehaviour tileBehaviour)
    {
        _selectedPiece.Location = tileBehaviour.Tile.Location;
        CreatePieces();
        OnPieceSelected(null);
        OnGameStateChanged();
    }

    void OnPieceSelected(PieceBehaviour pieceBehaviour)
    {
        Destroy(_torus);

        _selectedPiece = pieceBehaviour == null || _selectedPiece == pieceBehaviour.Piece ? null : pieceBehaviour.Piece;

        DrawSelection();
    }

    private void DrawSelection()
    {
        if (_selectedPiece == null)
            return;

        _torus = (GameObject)Instantiate(SelectionObject);
        _torus.transform.position = GetWorldCoordinates(_selectedPiece.Location.X, _selectedPiece.Location.Y, 1f);
    }

    List<GameObject> _gamePieces;

    private void CreatePieces()
    {
        if (_gamePieces == null)
            _gamePieces = new List<GameObject>();

        _gamePieces.ForEach(Destroy);
        
        var startPiece = _game.GamePieces.First();
        var destinationPiece = _game.GamePieces.Last();

        _gamePieces = new List<GameObject>
                          {
                              CreatePiece(startPiece, Color.blue),
                              CreatePiece(destinationPiece, Color.red)
                          };
    }

    private GameObject CreatePiece(GamePiece piece, Color colour)
    {
        var visualPiece = (GameObject)Instantiate(Piece);
        visualPiece.transform.position = GetWorldCoordinates(piece.X, piece.Y, 1f);
        var mat = new Material(Shader.Find(" Glossy")) {color = colour};
        visualPiece.renderer.material = mat;

        var pb = (PieceBehaviour)visualPiece.GetComponent("PieceBehaviour");

        pb.Piece = piece;

        return visualPiece;
    }

    private void CreateBoard()
    {
        _game = new Game(Width, Height);
        _gameBoard = new GameObject[Width, Height];

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var tile = (GameObject)Instantiate(Tile);

                _gameBoard[x, y] = tile;

                var tileTransform = tile.transform;

                tileTransform.position = GetWorldCoordinates(x, y, 0);

                var cylinder = tileTransform.Find("Cylinder");

                var tb = (TileBehaviour)cylinder.GetComponent("TileBehaviour");

                tb.Tile = _game.GameBoard[x, y];

                tb.SetMaterial();
            }
        }
    }

    static Vector3 GetWorldCoordinates(int x, int y, float z)
    {
        var yOffset = x % 2 == 0 ? 0 : -Spacing / 2;
        return new Vector3(x * Spacing, y * Spacing + yOffset, -z);
    }

    void TileChanged(TileBehaviour tileBehaviour)
    {
        tileBehaviour.Tile.CanPass = !tileBehaviour.Tile.CanPass;
        tileBehaviour.SetMaterial();
        OnGameStateChanged();
    }

    void OnGameStateChanged()
    {
        Debug.Log("Game-state changed");

        var sp = _game.GamePieces.First();
        var dp = _game.GamePieces.Last();

        var start = _game.AllTiles.Single(o => o.X == sp.Location.X && o.Y == sp.Location.Y);
        var destination = _game.AllTiles.Single(o => o.X == dp.Location.X && o.Y == dp.Location.Y);

        Func<Tile, Tile, double> distance = (node1, node2) => 1;
        Func<Tile, double> estimate = t => Math.Sqrt(Math.Pow(t.X - destination.X, 2) + Math.Pow(t.Y - destination.Y, 2));

        var path = PathFind.PathFind.FindPath(start, destination, distance, estimate);

        DrawPath(path);
    }
}
