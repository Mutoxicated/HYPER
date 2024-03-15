using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public class Playlist : MonoBehaviour
{
    [SerializeField] private List<AudioSource> songs;

    private int currIndex = 0;
    private bool played = false;

    private void Shuffle(IList<AudioSource> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            AudioSource value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void Start(){
        Shuffle(songs);
    }

    private void Update(){
        if (!songs[currIndex].isPlaying) {
            if (!played){
                played = true;
            }else {
                currIndex++;
                if (currIndex > songs.Count-1){
                    currIndex = 0;
                }
                played = false;
            }
            songs[currIndex].Play();
        }
    }
}
