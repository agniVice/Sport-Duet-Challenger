using DG.Tweening;
using UnityEngine;

public class BigBall : MonoBehaviour
{
    public BallType CurrentType;

    [SerializeField] private GameObject _particlePrefab;

    private bool _isActive = false;

    private Vector3 _targerRotation;

    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;
        if (!_isActive)
            return;

        transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y);
    }
    private void OnEnable()
    {
        PlayerInput.Instance.PlayerMouseDown += OnPlayerMouseDown;
        PlayerInput.Instance.PlayerMouseUp += OnPlayerMouseUp;
    }
    private void OnDisable()
    {
        PlayerInput.Instance.PlayerMouseDown -= OnPlayerMouseDown;
        PlayerInput.Instance.PlayerMouseUp -= OnPlayerMouseUp;
    }
    private void OnPlayerMouseDown()
    {
        _isActive = true;
        _targerRotation = new Vector3(0, 0, 180);

        RotateBall();
    }
    private void OnPlayerMouseUp()
    {
        _isActive = false;
        _targerRotation = new Vector3(0, 0, 0);

        RotateBall();
    }
    private void RotateBall()
    {
        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Swap, 1f);
        transform.DORotate(_targerRotation, 0.5f).SetLink(gameObject);
    }
    private void SpawnParticle()
    {
        var particle = Instantiate(_particlePrefab).GetComponent<ParticleSystem>();

        particle.transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);
        particle.Play();

        Destroy(particle.gameObject, 2f);
    }
    public void OnBallIncorrect()
    {
        SpawnParticle();

        Camera.main.DOShakePosition(0.4f, 0.2f, fadeOut: true).SetUpdate(true);
        Camera.main.DOShakeRotation(0.4f, 0.2f, fadeOut: true).SetUpdate(true);


        transform.DOScale(0, 0.2f).SetLink(gameObject).SetUpdate(true);

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Win, 1f);
        GameState.Instance.FinishGame();
    }
}
