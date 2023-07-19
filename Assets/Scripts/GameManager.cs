using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> SpawnPoints;
    public Enemy EnemyPrefab;
    public GameObject FxPrefab;
    public GameObject Player;
    public int EnemiesCount = 3;
    public float PlayerAttackDistance = 2.5f;
    public float PlayerAttackAngle = 60f;
    
    private readonly List<Enemy> _enemies = new List<Enemy>();
    private Player _player;
    private int _spawnIndex = new System.Random().Next(0, 1000);

    private void Start()
    {
        _player = Player.GetComponent<Player>();
        _player.OnAttack += OnPlayerAttack;
    }

    private void OnPlayerAttack()
    {
        for (var index = _enemies.Count - 1; index >= 0; index--)
        {
            var enemy = _enemies[index];
            var enemyPosition = enemy.transform.position;
            var playerPosition = _player.transform.position;
            var distance = enemyPosition - playerPosition;
            var enemyDirection = (enemyPosition - playerPosition).normalized;
            var angle = Vector3.Angle(_player.transform.forward, enemyDirection);
            
            if (distance.magnitude < PlayerAttackDistance && angle < PlayerAttackAngle)
            {
                SpawnFx(enemyPosition);
                _enemies.Remove(enemy);
                enemy.PlayDieAnimation();
            }
        }
    }

    private async Task SpawnFx(Vector3 position)
    {
        var go = Instantiate(FxPrefab);
        go.transform.position = position;
        await Task.Delay(3000);
        Destroy(go);
    }

    private void Update()
    {
        if (_enemies.Count < EnemiesCount)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var enemyGo = Instantiate(EnemyPrefab.gameObject);
        var enemy = enemyGo.GetComponent<Enemy>();
        _enemies.Add(enemy);

        var spawnPoint = SpawnPoints[_spawnIndex++ % SpawnPoints.Count];
        enemyGo.transform.position = spawnPoint.transform.position;

        enemy.Init(Player);
    }
}
