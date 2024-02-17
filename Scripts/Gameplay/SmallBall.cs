using DG.Tweening;
using UnityEngine;

public class SmallBall : MonoBehaviour
{
    private BallType _currentType;

    private float _speed;

    private Rigidbody2D _rigibody;
    private Collider2D _collider;

    private GameObject _particlePrefab;

    private void Start()
    {
        _rigibody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }
    public void Initialize(BallType type, Sprite sprite, float speed, GameObject particlePrefab)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        _speed = speed;
        _currentType = type;
        _particlePrefab = particlePrefab;
    }
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        float verticalMovement = -_speed * Time.fixedDeltaTime;

        Vector2 currentPosition = _rigibody.position;

        Vector2 newPosition = currentPosition + new Vector2(0f, verticalMovement);

        _rigibody.MovePosition(newPosition);
    }
    private void SpawnParticle()
    {
        var particle = Instantiate(_particlePrefab).GetComponent<ParticleSystem>();

        particle.transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);
        particle.Play();

        Destroy(particle.gameObject, 2f);
    }
    private void OnBallCorrect()
    {
        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.ScoreAdd, 1f);
        PlayerScore.Instance.AddScore();

        Camera.main.DOShakePosition(0.1f, 0.1f, fadeOut: true).SetUpdate(true);
        Camera.main.DOShakeRotation(0.1f, 0.1f, fadeOut: true).SetUpdate(true);


        _collider.enabled = false;
        transform.DOScale(0, 0.2f).SetLink(gameObject);

        SpawnParticle();
        Destroy(gameObject,0.3f);
    }
    private void OnBallIncorrect()
    {
        transform.DOScale(0, 0.2f).SetLink(gameObject).SetUpdate(true);

        FindObjectOfType<BigBall>().OnBallIncorrect();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Football"))
        {
            if (_currentType == BallType.Football)
                OnBallCorrect();
            else 
                OnBallIncorrect();
        }
        if (collision.gameObject.CompareTag("BascketBall"))
        {
            if (_currentType == BallType.BascketBall)
                OnBallCorrect();
            else 
                OnBallIncorrect();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
                GameState.Instance.FinishGame();
    }
}
