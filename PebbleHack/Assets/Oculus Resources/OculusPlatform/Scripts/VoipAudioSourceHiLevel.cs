namespace Oculus.Platform
{
  using UnityEngine;
  using System;
  using System.Collections.Generic;

  public class VoipAudioSourceHiLevel : MonoBehaviour
  {
    BufferedAudioStream bufferedAudioStream;
    float[] scratchBuffer;
    public UInt64 senderID;

    public AudioSource audioSource;

    protected void Stop() {}

    void Awake() 
    {
      audioSource = gameObject.AddComponent<AudioSource>();
      bufferedAudioStream = new BufferedAudioStream(audioSource);

      const int frequency = 48000;
      const int bufferSizeMS = 150;
      const int bufferSizeElements = bufferSizeMS * frequency / 1000;
      scratchBuffer = new float[bufferSizeElements];
    }

    void Update()
    {
      if (bufferedAudioStream == null)
      {
        throw new Exception("VoipAudioSource failed to init");
      }

      if(senderID == 0)
      {
        throw new Exception("SenderID not set");
      }

      int copied = (int)CAPI.ovr_Voip_GetPCMFloat(senderID, scratchBuffer, (UIntPtr)scratchBuffer.Length);
      if (copied > 0)
      {
        bufferedAudioStream.AddData(scratchBuffer, copied);
      }

      bufferedAudioStream.Update();
    }
  }
}
