using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTerrainEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private AudioSource[] sounds;

    [SerializeField] private float cooldownTime = 0.5f;
    private bool canEffect;

    private AudioSource currentSound;

    void Start()
    {
        canEffect = true;
    }
    public void OnTerrainEffect(int terrainIndex)
    {
        if(canEffect)
        {
            Instantiate(particles[terrainIndex], gameObject.transform.position, particles[terrainIndex].transform.rotation, gameObject.transform);
            StartCoroutine(StartCooldown());
        }
        if(currentSound == null)
        {
            CreateSound(terrainIndex);
        }
    }

    public void OnTerrainChange(int newTerrainIndex)
    {
        CreateSound(newTerrainIndex);
    }

    public void OnStopTerrainEffect()
    {
        DestroySound();
    }

    private void DestroySound()
    {
        if (currentSound != null)
        {
            currentSound.Stop();
            Destroy(currentSound);
            currentSound = null;
        }
    }

    private void CreateSound(int terrainIndex)
    {
        DestroySound();
        currentSound = Instantiate(sounds[terrainIndex], gameObject.transform.position, Quaternion.identity, gameObject.transform);
        currentSound.Play();
    }

    public IEnumerator StartCooldown()
    {
        canEffect = false;
        yield return new WaitForSeconds(cooldownTime);
        canEffect = true;
    }
}
