using UnityEngine;
using UnityEngine.UIElements;

public class BaseEnemy : MonoBehaviour
{
    public float appearTime;    //出現時間
    public float isAlive;
    public bool hasAppeared;    //敵の出現確認
    public int scoreValue = 1;  // 倒したときに入るスコア
    public float moveSpeed = 2.5f;　//　敵の速度

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // === 初期化メソッド ===
    public void Initialize(Vector2 position, float appearTime)
    {
        gameObject.SetActive(false);
        transform.position = position;
        this.appearTime = appearTime;
        hasAppeared = false;
    }
    // === 移動メソッド ===
    public void Movement()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }
}