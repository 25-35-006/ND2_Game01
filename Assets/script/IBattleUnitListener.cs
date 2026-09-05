using UnityEngine;

public interface IBattleUnitListener 
{


    // === バトルユニット（戦闘機）の取り扱いルール === //
    // ※記述ルールとなるインターフェース
    //　　このインターフェースを<継承>したオブジェクトに対してルールを決める
 
        public void OnCollision(Transform player = null, Transform enemy = null);
    
}