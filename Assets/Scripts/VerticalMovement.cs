using UnityEngine;

public class VerticalMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if(GameData.Instance.IsPaused)
            return;

        var amount = GameData.Instance.SpeedConstant * _speed * Time.deltaTime;
        _transform.Translate(Vector3.down * amount, Space.World);
    }
}