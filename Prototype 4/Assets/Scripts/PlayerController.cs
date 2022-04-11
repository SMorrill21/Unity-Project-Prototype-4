using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject powerUpIndicator;
    public float speed = 5;
    public bool hasPowerUp = false;

    private Rigidbody playerRb;
    private GameObject focalPoint;
    private float powerUpStrength = 15;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        powerUpIndicator.transform.position = transform.position - new Vector3(0, .6f, 0);

        float verticalSpeed = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * verticalSpeed * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp")) {
            hasPowerUp = true;
            powerUpIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountDownRoutine());
        }
    }

    private IEnumerator PowerUpCountDownRoutine()
    {
        yield return new WaitForSeconds(5);
        hasPowerUp = false;
        powerUpIndicator.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasPowerUp && collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }
    }
}
