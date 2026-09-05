using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayer: MonoBehaviour
{
    // === Protected Value === //
    protected Vector2 _inputMoveValue;
    protected float _inputShotValue;
    protected GameManager _manager;
    public float moveSpeed = 4f;
    void Start()
    {
       
    }

    void Update()
    {
        _manager.OnCollision(player: transform);
    }
    // === 初期化メソッド === //
    public void Initialize( GameManager manager, Vector2 position )
    {
        transform.position = position;     
        _manager = manager;
    }

    // === 移動メソッド === //
    public void Movement()
    {
        transform.Translate(_inputMoveValue * moveSpeed * Time.deltaTime);

        float x = Mathf.Clamp(transform.position.x, -8f, 8f);
        float y = Mathf.Clamp(transform.position.y, -4f, 4f);

        transform.position = new Vector3(x, y, transform.position.z); //画面端サイズ設定

        Debug.Log($"{transform.position} >> 移動");
    }
    // === 攻撃メソッド === //
    public void Shot()
    {
        if (_inputShotValue <= 0.5f) return;

        Debug.Log($"{transform.name} >> 攻撃");

        BaseBullet[] bullets = _manager.bullets;

        for (int index = 0; index < 100; index++)
        {
            if (bullets[index].gameObject.activeSelf == false)
            {
                bullets[index].Appear(transform.position, Vector3.up);
                break;
            }
        }

        _inputShotValue = 0;
    }
    // === 移動入力イベント === //
    protected void OnMove(InputValue value)
    {
        Debug.Log($"移動入力値 = {value.Get<Vector2>()}" );
        _inputMoveValue = value.Get<Vector2>();     

    }

    // === 攻撃入力イベント === //
    protected void OnShot(InputValue value)
    {
        Debug.Log($"攻撃入力値 = {value.Get<float>()}");
        _inputShotValue = value.Get<float>();
    }
}
