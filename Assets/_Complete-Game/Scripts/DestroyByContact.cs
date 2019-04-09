using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    private GameController _gameController;
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;

    private void Start()
    {
        var gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null) _gameController = gameControllerObject.GetComponent<GameController>();
        if (_gameController == null) Debug.Log("Cannot find 'GameController' script");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy")) return;

        if (explosion != null)
        {
            var myTransform = transform;
            Instantiate(explosion, myTransform.position, myTransform.rotation);
        }

        if (other.CompareTag("Player"))
        {
            var player = other.transform;
            Instantiate(playerExplosion, player.position, player.rotation);
            _gameController.GameOver();
        }

        _gameController.AddScore(scoreValue);
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}