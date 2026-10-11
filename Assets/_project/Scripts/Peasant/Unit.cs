using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private Transform[] _points;
    [SerializeField] private int _pointsCount = 5;
    [SerializeField] private float _spawnRadius = 5f;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Transform _currentPoint;
    private int i = 0;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        GeneratePoints();
        _currentPoint = _points[0];
    }

    private void Update()
    {
        if (_currentPoint != null)
        {
            if (i == _points.Length)
                i = 0;

            Move(_currentPoint);

            if (Vector2.Distance(transform.position, _currentPoint.position) < 0.01f)
            {
                _currentPoint = _points[i++];
            }
        }
    }

    private void Move(Transform target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, _movementSpeed * Time.deltaTime);
        MoveAnimation();
        ChangeFacingDirection(target.position);
    }
    
    private void ChangeFacingDirection(Vector2 target) => _spriteRenderer.flipX = transform.position.x > target.x;

    private void MoveAnimation()
    {
        float speed = (transform.position - _currentPoint.position).sqrMagnitude / Time.deltaTime;
        _animator.SetFloat("Speed", speed);
    }
    
    private void GeneratePoints()
    {
        _points = new Transform[_pointsCount];
        
        for (int j = 0; j < _pointsCount; j++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;
            Vector2 spawnPos = (Vector2)transform.position + randomOffset;

            Transform point = new GameObject($"Point_{j}").transform;
            point.position = spawnPos;

            _points[j] = point;
        }
    }
}