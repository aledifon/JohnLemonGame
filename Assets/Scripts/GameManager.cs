using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Image Settings")]
    public Image caughtImage;
    public Image wonImage;
    public float fadeDuration;          // Image fade duration (image will be gradually shown)
    public float displayImageDuration;  // Total time the image will be shown in the display (once is completely shown)

    [Header("Flags")]
    public bool isPlayerAtExit;         // To know if the player has arrived to the exit
    public bool isPlayerCaught;         // To know if the player has been caught

    [Header("Audio Clip")]
    public AudioClip caughtClip;
    public AudioClip wonClip;

    AudioSource audioSource;
    float timer;                        // Timer
    bool restartLevel;                  // Will tell me if I reset the level or not


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player arrived to the exit and he won
        if (other.CompareTag("Player"))
        {
            isPlayerAtExit = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerAtExit || isPlayerCaught)
            EndLevel();
    }
    void EndLevel()
    {
        if (isPlayerAtExit)
            audioSource.clip = wonClip;     // Get the corresponding clip (Won or Caught)
        else if (isPlayerCaught)
            audioSource.clip = caughtClip;     

        if (!audioSource.isPlaying)
            audioSource.Play();

        timer += Time.deltaTime;        // Increase timer
        
        // Increase the corresponding image alpha channel
        // (Keep the RGB values and just modify the alpha channel)        
        if (isPlayerAtExit)
            wonImage.color = new Color(wonImage.color.r,
                                        wonImage.color.g,
                                        wonImage.color.b,
                                        timer/fadeDuration);    
        else if (isPlayerCaught)
            caughtImage.color = new Color(caughtImage.color.r,
                                        caughtImage.color.g,
                                        caughtImage.color.b,
                                        timer / fadeDuration);        

        // Leave the image on the display for a certain time
        if (timer > fadeDuration + displayImageDuration)
        {
            if (isPlayerAtExit)
                Debug.Log("I won!");
            else if (isPlayerCaught)
                Debug.Log("I got caught!");

            // Then a transition to a new (or tho the same) Scene should be implemented
            // 
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
    }    
}
