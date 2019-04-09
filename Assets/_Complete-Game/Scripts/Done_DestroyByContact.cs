using UnityEngine;

public class Done_DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    private Done_GameController gameController;
    public GameObject playerExplosion;
    public int scoreValue;

    private void Start()
    {
        var gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null) gameController = gameControllerObject.GetComponent<Done_GameController>();
        if (gameController == null) Debug.Log("Cannot find 'GameController' script");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Enemy") return;

        if (explosion != null) Instantiate(explosion, transform.position, transform.rotation);

        if (other.tag == "Player")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }

        gameController.AddScore(scoreValue);
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}