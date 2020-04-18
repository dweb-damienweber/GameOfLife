using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region System methods

    private void Awake()
    {
        _cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

        if (!_tilesContainer || !_tilePrefab || !_cameraTransform)
            Debug.LogError("Missing references");

        GenerateBoard();
        _nextTurn = Time.time + _timeBeforeNextTurn;
    }

    private void Update()
    {
        if (Time.time > _nextTurn)
        {
            CheckTiles();
            _nextTurn = Time.time + _timeBeforeNextTurn;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            GenerateBoard();
    }

    #endregion


    #region Methods

    /// <summary>
    /// Create the initial board
    /// </summary>
    private void GenerateBoard()
    {
        _tiles = new Tile[_width, _height];

        for (int h = 0; h < _height; h++)
        {
            for (int w = 0; w < _width; w++)
            {
                Vector2 position = new Vector2(w, h);
                Tile tile = Instantiate(_tilePrefab, position, Quaternion.identity, _tilesContainer);
                tile.gameObject.name = $"Tile_{w}_{h}";
                SetIfTileIsAlive(tile);
                _tiles[w, h] = tile;
            }
        }

        SetCameraPosition();
    }

    /// <summary>
    /// Center camera on the map
    /// </summary>
    private void SetCameraPosition()
    {
        Vector3 position = new Vector3(_width * .5f - .5f, _height * .5f - .5f, -10);
        _cameraTransform.position = position;
    }

    /// <summary>
    /// Is the tile alive ? (Used when the board is generated)
    /// </summary>
    private void SetIfTileIsAlive(Tile tile)
    {
        if (Random.Range(0, 6) == 1)
            tile.ChangeState(true);
        else
            tile.ChangeState(false);
    }

    /// <summary>
    /// Check if the tiles are alive or not
    /// </summary>
    private void CheckTiles()
    {
        for (int h = 0; h < _height; h++)
        {
            for (int w = 0; w < _width; w++)
            {
                int n = HowManyAliveAround(w, h);

                if (_tiles[w, h].IsAlive())
                {
                    if (n < 2 || n > 3)
                        _tiles[w, h].ChangeState(false);
                    else if (n == 2 || n == 3)
                        _tiles[w, h].ChangeState(true);
                }
                else
                {
                    if (n == 3)
                        _tiles[w, h].ChangeState(true);
                }
            }
        }
    }

    /// <summary>
    /// Return how many living tile is around the given tile
    /// </summary>
    private int HowManyAliveAround(int w, int h)
    {
        int n = 0;

        if (w > 0 && _tiles[w - 1, h].IsAlive())
            n++;
        if (w < _width - 1 && _tiles[w + 1, h].IsAlive())
            n++;
        if (h > 0 && _tiles[w, h - 1].IsAlive())
            n++;
        if (h < _height - 1 && _tiles[w, h + 1].IsAlive())
            n++;

        if (w > 0 && h < _height - 1 && _tiles[w - 1, h + 1].IsAlive())
            n++;
        if (w < _width - 1 && h < _height - 1 && _tiles[w + 1, h + 1].IsAlive())
            n++;
        if (w > 0 && h > 0 && _tiles[w - 1, h - 1].IsAlive())
            n++;
        if (w < _width - 1 && h > 0 && _tiles[w + 1, h - 1].IsAlive())
            n++;

        return n;
    }

    #endregion


    #region Private fields

    private Tile[,] _tiles;

    [Header("Settings")]

    [SerializeField] private int _width = 35;
    [SerializeField] private int _height = 20;
    [SerializeField] private float _timeBeforeNextTurn = 1f;

    [Header("References")]

    [SerializeField] private Transform _tilesContainer = null;
    [SerializeField] private Tile _tilePrefab = null;
    private Transform _cameraTransform;

    private float _nextTurn;

    #endregion
}
