using System.Threading.Tasks;
using Characters;
using DG.Tweening;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private int money;
    private Transform _player;
    private bool _followToPlayer;
    private float _speed = 0.5f;

    public void SetPlayer(Transform player)
    {
        _player = player;
    }

    private async void Start()
    {
        transform.DOMoveY(transform.position.y - 0.2f, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.InOutSine);
        await Task.Delay(500);
        _followToPlayer = true;
    }


    private void Update()
    {
        if (!_followToPlayer) return;
        var direction = transform.position - _player.position;
        transform.position -= direction * (_speed * Time.deltaTime);
        _speed += 0.1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ITriggerable triggerable)) return;
        triggerable.AddMoney(money);
        transform.DOScale(0, 0.5f).OnComplete(() => Destroy(gameObject));
    }
}