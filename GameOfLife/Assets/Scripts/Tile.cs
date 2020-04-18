using UnityEngine;

public class Tile : MonoBehaviour
{
    #region System methods

    private void Awake()
    {
        _isAlive = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    #endregion


    #region Methods

    public bool IsAlive() => _isAlive;

    public void ChangeState(bool state)
    {
        _isAlive = state;

        if (_isAlive)
            _spriteRenderer.color = _aliveColor;
        else
            _spriteRenderer.color = _deadColor;
    }

    #endregion
    
    
    #region Private fields

    private bool _isAlive;
    private SpriteRenderer _spriteRenderer;

    [Header("Colors")]

    [SerializeField] private Color _deadColor = new Color();
    [SerializeField] private Color _aliveColor = new Color();

    #endregion
}
