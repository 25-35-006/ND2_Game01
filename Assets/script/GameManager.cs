
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // === 各種変数を宣言 ===
    public BasePlayer playerPrefab;     
    public BaseEnemy enemyPrefab;
    public BaseEnemy oniPrefab;
    public BaseEnemy zonbiPrefab;
    public BaseBullet bulletPrefab;
    public GameObject startPanel;
    public bool gameStarted = false; 

    public BasePlayer[] players;        
    public BaseEnemy[] enemies;         
    public BaseBullet[] bullets;

    // === スコアボードのやつ === //
    public TextMeshProUGUI scoreText;

    // === タイマーのやつ === //
    public TextMeshProUGUI timeText;

    // === ゲームオーバー === //
    public GameObject gameOverText;

    // === リトライボタン === //
    public GameObject retryButton;

    // === Quitボタン === ///
    public GameObject quitButton;

    // === ゲーム時間の測定 === //
    public float gameTimer;

    // === スコア === //
    public int score;

    // === 制限時間 === //
    public float timeLimit = 60f;

    public void StartGame()
    { // === ゲーム開始処理 ===

        {
            gameStarted = true;
            startPanel.SetActive(false);
            Debug.Log("GAME START!");
        }
    }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    void Start()
    {
        // ===　各配列の初期化 ===
        players = new BasePlayer[1];    
        enemies = new BaseEnemy[100];    
        bullets = new BaseBullet[100];   

        // === キャラクターをスポーンさせる ===//
        for(int index = 0; index < 1; index++)
        {
            players[index] = Instantiate(playerPrefab);     
        }
        // === エネミーをスポーンさせる ===//
        for (int index = 0; index < 100; index++)
        {
            int enemyType = Random.Range(0, 100);
            if (enemyType < 60)
            {
                enemies[index] = Instantiate(enemyPrefab);  //　幽霊の出現
            }
            else if (enemyType < 75)
            {
                enemies[index] = Instantiate(oniPrefab);    // 鬼の出現
            }
            else
            {
                enemies[index] = Instantiate(zonbiPrefab);  // ゾンビの出現
            }
        }
        // === 弾丸をスポーンさせる ===//
        for (int index = 0; index < 100; index++)
        {
            bullets[index] = Instantiate(bulletPrefab);    
        }

            // === キャラクターを初期化する === //
            for (int index = 0; index < 1; index++)
            {
                //　プレイヤーたちの初期化
                players[index].Initialize(this,new Vector2(-3, 0));
            }

        for(int index = 0; index < 100; index++)
        {
            //　敵はランダムな位置で初期化
            Vector2 randomPos = Vector2.zero;
            randomPos.x = Random.Range(-5f, 5f);
            randomPos.y = 15f;

            float randomTime = Random.Range(1f, 60f);
            enemies[index].Initialize(randomPos, randomTime);
        }

        for (int index = 0; index < 100; index++) 
        {
            bullets[index].Initialize(players[0].transform.position, Vector3.up);
        }
    }
    //ゲーム終了表示
    // === ゲームオーバー処理 ===
    public void GameOver()
    {
        Debug.Log("GAME OVER表示！");

        gameOverText.SetActive(true);//　ゲームオーバー表示
        retryButton.SetActive(true);// リトライボタン表示
        quitButton.SetActive(true);// Quitボタン表示

        gameOverText.GetComponent<TextMeshProUGUI>().text = $"GAME OVER\nSCORE: {score}";  //　リザルト表示

        enabled = false;
    }
    void Update()
    {
        // ゲームスタート
        if (!gameStarted)
        {
            return;
        }
        //時間計算
        gameTimer += Time.deltaTime;
        // 時間切れ
        if (gameTimer >= timeLimit)
        {
            GameOver();
            return;
        }
        {
        // スコア表示を更新
        scoreText.text = $"SCORE:{score}";
        }
        // タイマー処理
        float remainingTime = Mathf.Max(0, timeLimit - gameTimer);
        timeText.text = $"TIME: {Mathf.CeilToInt(remainingTime)}";
        // プレイヤーを全員動かす
        for (int index = 0; index < 1; index++)
        {
            players[index].Movement();
            players[index].Shot();
        }
        //敵を全員動かす
        for (int index = 0; index < 100; index++)
        {
            if (enemies[index].gameObject.activeSelf == true)
            enemies[index].Movement();
        }
        //時間に合わせて出現
        for (int index = 0; index < 100; index++)
        {
            // 非アクティブの敵だけを対象にする
            if (enemies[index].gameObject.activeSelf == false &&　enemies[index].hasAppeared == false &&　gameTimer > enemies[index].appearTime)
            {
                enemies[index].gameObject.SetActive(true);
                enemies[index].hasAppeared = true;
            }
        }
        //弾丸を動かす
        for (int index = 0; index < 100; index++)
        {
            bullets[index].Movement();
        }  
        }

    //=== オブジェクトを登録するメソッド === //
    public void EnterPlayer (BasePlayer player)
    {
        if(players == null || players.Length == 0)
        {
            Debug.Log("配列の初期化に失敗していますよ...");
            return;        
        }
        for(int index = 0; index <players.Length; index++)
        {
            Debug.Log($"配列の{index}番目の中身 >> {players[index]}");
            if (players[index] == null) 
            {
                Debug.Log($"[{index}番目に中身がない(Null)ので、{index}を代入します。");
                players[index] = player;

                return;     
            }
        }
        Debug.Log($"空きがないので、{player}を設定出来ませんでした...");
    }
    public void EnterEnemy(BaseEnemy enemy)
    {
        if (enemies == null || enemies.Length == 0)
        {
            Debug.Log("配列の初期化に失敗していますよ...");
            return;         
        }
        for (int index = 0; index < enemies.Length; index++)
        {
            Debug.Log($"配列の{index}番目の中身 >> {enemies[index]}");
            if (enemies[index] == null)
            {
                Debug.Log($"[{index}番目に中身がない(Null)ので、{index}を代入します。");
                enemies[index] = enemy;

                return;     
            }
        }
        Debug.Log($"空きがないので、{enemy}を設定出来ませんでした...");
    }
    // === 衝突判定 === //
    public void OnCollision(Transform player = null, Transform enemy = null)
    {
        try
        {
            Debug.Log($"衝突対象 >> {player.name} vs {enemy.name}");
        }
        catch (System.Exception error)
        {
            Debug.Log($"エラーを無視します... >> \n{ error }");
        }
        // ------- すべての敵と判定を取る ------- //
        for (int e = 0; e < enemies.Length; e++)
        {
            if (enemies[e].gameObject.activeSelf)
            {

                // 2点間の距離
                float distance = Vector2.Distance(player.transform.position, enemies[e].transform.position);
                if (distance <= 1)
                {
                    // nメートルの範囲内で衝突
                    Debug.Log($"範囲内！！{player} vs {enemies[e]}");

                    // ゲームオーバー
                    GameOver();
                    return;
                }
            }
        }
    }

    // === 弾丸の衝突判定 === //
    public void OnBulletDamage(Transform bullet)
    {
        // 全ての敵をループで判定する
        for (int e = 0; e < enemies.Length; e++)
        {
            if (enemies[e].gameObject.activeSelf)
            {   // 表示されている敵だけ判定する
                float distance = Vector2.Distance(bullet.position, enemies[e].transform.position);

                if (distance <= 1)
                {
                    Debug.Log($"範囲内!! {bullet} vs {enemies[e]}");

                    bullet.gameObject.SetActive(false);
                    enemies[e].gameObject.SetActive(false);

                    score += enemies[e].scoreValue;
                    Debug.Log($"スコア：{score}");
                }
            }
        }
    }
    // === リトライ処理 === //
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // === ゲーム終了処理 === //
    public void QuitGame()
    {
        Debug.Log("ゲームを終了します");
        Application.Quit();
    }
}

