namespace Oculus.Platform
{
  using UnityEngine;
  using System.Collections.Generic;

  public class VoipAudioSource : MonoBehaviour
  {
    public bool spatialize = true;

    BufferedAudioStream bufferedAudioStream;
    Decoder decoder;
    protected List<float> debugOutputData;

    void Awake()
    {
      AudioSource audioSource = gameObject.AddComponent<AudioSource>();
      Debug.Log(audioSource);
      audioSource.spatialize = spatialize;
      bufferedAudioStream = new BufferedAudioStream(audioSource);
      decoder = new Decoder();
    }

    public void Stop()
    {
    }

    public void AddCompressedData(byte[] compressedData)
    {
      if(decoder == null || bufferedAudioStream == null)
      {
        throw new System.Exception("VoipAudioSource failed to init");
      }

      float[] decompressedData = decoder.Decode(compressedData);
      if (decompressedData != null && decompressedData.Length > 0)
      {
        bufferedAudioStream.AddData(decompressedData, decompressedData.Length);
        if (debugOutputData != null)
        {
          debugOutputData.AddRange(decompressedData);
        }
      }
    }

    void Update()
    {
      if (bufferedAudioStream == null)
      {
        throw new System.Exception("VoipAudioSource failed to init");
      }

      bufferedAudioStream.Update();
    }
  }
}
