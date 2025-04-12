using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] GameObject heartTemplate;
    [SerializeField] Sprite fullHeartSprite;
    [SerializeField] Sprite emptyHeartSprite;

    private List<Image> heartImages = new();

    private void Start()
    {
        int max = playerHealth.GetMaxHealth();

        for(int i = 0; i < max; i++)
        {
            GameObject heart = Instantiate(heartTemplate, transform);
            heart.SetActive(true);

            Image img = heart.GetComponent<Image>();
            heartImages.Add(img);
        }
    }

    private void Update()
    {
        int current = playerHealth.GetCurrentHealth();

        for(int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].sprite = i < current ? fullHeartSprite : emptyHeartSprite;
        }
    }
}
