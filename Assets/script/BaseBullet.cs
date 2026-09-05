using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    // === 引数 === //
    public Vector3 direction;
    public Vector3 startPosition;


    private GameManager _manager;
    void Start()
    {
        
    }
    void Update()
    {
        _manager.OnBulletDamage(transform);
    }

    // === 初期化メソッド === //
    public void Initialize(Vector3 position, Vector3 direcition)
    {
        gameObject.SetActive(false);   
        transform.position = position;
        this.direction = direcition;

        _manager = GameManager.Instance;
    }

    // === 出現メソッド === //
    public void Appear(Vector3 position, Vector3 direction)
    {
        gameObject.SetActive(true);   
        transform.position = position;  //出現座標
        this.direction = direction;     //向きベクトル
        startPosition = position;       //生成位置
    }
    
    // === 移動メソッド === //
    public void Movement()
    {
        transform.Translate(direction * 10 * Time.deltaTime);

        bool outOfRange = CheckRange(10);
        if(outOfRange == true)
        {
            gameObject.SetActive(false);
        }
    }

    // === 一定距離を求める === //
    public bool CheckRange(float range)
    {
        float distance = Vector3.Distance(transform.position, startPosition);
        if(range <= distance)
        {     
            return true;
        }
        return false;
    }
}
