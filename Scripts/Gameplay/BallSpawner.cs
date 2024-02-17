using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public static BallSpawner Instance;

    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Transform _positionSpawn;
    [SerializeField] private Transform _positionLeft;
    [SerializeField] private Transform _positionRight;

    [SerializeField] private Sprite[] _ballSprites;

    [SerializeField] private GameObject[] _particlePrefab;

    private float _timeToSpawn;
    private float _currentTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (_currentTime <= 0)
        {
            SpawnBall();
            SetRandomTime();
        }
        else
        {
            _currentTime -= Time.fixedDeltaTime;
        }
    }
    public void SpawnBall()
    {
        var ball = Instantiate(_ballPrefab, GetSpawnPosition(), Quaternion.identity);

        BallType type = GetRandomBallType();

        ball.GetComponent<SmallBall>().Initialize(type, _ballSprites[(int)type], GetRandomSpeed(), _particlePrefab[(int)type]);
    }
    private void SetRandomTime()
    {
        if (PlayerScore.Instance.Score > 50)
            _timeToSpawn = Random.Range(0.3f, 0.5f);
        if (PlayerScore.Instance.Score > 30)
            _timeToSpawn = Random.Range(0.5f, 0.7f);
        if (PlayerScore.Instance.Score > 20)
            _timeToSpawn = Random.Range(1f, 1.2f);
        if (PlayerScore.Instance.Score > 10)
            _timeToSpawn = Random.Range(1.2f, 1.5f);
        if (PlayerScore.Instance.Score < 5)
            _timeToSpawn = Random.Range(1.5f, 2f);

        _currentTime = _timeToSpawn;
    }
    private float GetRandomSpeed()
    {
        if (PlayerScore.Instance.Score > 50)
            return Random.Range(4f, 5f);
        if (PlayerScore.Instance.Score > 30)
            return Random.Range(3.5f, 4f);
        if (PlayerScore.Instance.Score > 20)
            return Random.Range(3f, 3.5f);
        if (PlayerScore.Instance.Score > 10)
            return Random.Range(2.5f, 3f);
        if (PlayerScore.Instance.Score < 5)
            return Random.Range(2f, 2.5f);

        return Random.Range(1.5f, 2f);
    }
    private Vector3 GetSpawnPosition()
    {
        return new Vector2(Random.Range(_positionLeft.position.x, _positionRight.position.x), _positionSpawn.position.y);
    }
    private BallType GetRandomBallType()
    {
        return (BallType)Random.Range(0, 2);
    }
}
