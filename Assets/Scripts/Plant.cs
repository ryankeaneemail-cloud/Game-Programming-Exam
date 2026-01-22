using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CropName
{
    Carrot,
    Cabbage,
    Pumpkin
}

public class Plant : MonoBehaviour
{
    public CropName cropName;
    public float respawnTime;

    Collider2D plantCollider;
    SpriteRenderer spriteRenderer;
    public Sprite harvestedSprite, plantedSprite;

    // Start is called before the first frame update
    void Start()
    {
        plantCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HarvestPlant()
    {
        spriteRenderer.sprite = harvestedSprite;
        plantCollider.enabled = false;
        GameManager.instance.AddCropAmount(cropName);

        StartCoroutine(RespawnPlant());
    }

    IEnumerator RespawnPlant()
    {
        yield return new WaitForSeconds(respawnTime);

        plantCollider.enabled = true;
        spriteRenderer.sprite = plantedSprite;
    }
}
