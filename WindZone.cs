using System.Collections;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    [SerializeField] Vector2 windForce = new Vector2(10f, 0f);
    [SerializeField] bool windActive = true;
    [SerializeField] bool isConstantWind = true;
    [SerializeField] float toggleOnTime = 3f;
    [SerializeField] float toggleOffTime = 2f;
    [SerializeField] ParticleSystem windParticles;

    private void Start()
    {
        if(!isConstantWind)
        {
            StartCoroutine(ToggleWindOn());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(windActive && collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb == null) return;

            rb.AddForce(windForce, ForceMode2D.Force);
        }
    }

    public void SetWindActive(bool state)
    {
        windActive = state;

        if (windParticles != null)
        {
            if (windActive)
            {
                windParticles.Play();
            }
            else
            {
                windParticles.Stop();
            }
        }
    }

    public void ToggleWind()
    {
        windActive = !windActive;
    }

    private IEnumerator ToggleWindOn()
    {
        SetWindActive(true);
        yield return new WaitForSeconds(toggleOnTime);
        StartCoroutine(ToggleWindOff());
    }

    private IEnumerator ToggleWindOff()
    {
        SetWindActive(false);
        yield return new WaitForSeconds(toggleOffTime);
        StartCoroutine(ToggleWindOn());
    }
}
