using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public Slider slider2;
    public Slider slider3;

    private void Start()
    {
        slider.value = SaveController.Getinstance.Sound1;
        slider2.value = SaveController.Getinstance.Sound2;
        slider3.value = SaveController.Getinstance.Sound3;
    }

    public void MasterVolume()
    {
        float sound = slider.value;
        SaveController.Getinstance.Sound1 = sound;

        if (sound == -40f) mixer.SetFloat("Master", -80f);
        else mixer.SetFloat("Master", sound);
    }

    public void BGMVolume()
    {
        float sound = slider2.value;
        SaveController.Getinstance.Sound2 = sound;

        if (sound == -40f) mixer.SetFloat("BGM", -80f);
        else mixer.SetFloat("BGM", sound);
    }

    public void FXVolume()
    {
        float sound = slider3.value;
        SaveController.Getinstance.Sound3 = sound;

        if (sound == -40f) mixer.SetFloat("FX", -80f);
        else mixer.SetFloat("FX", sound);
    }

    // ** 위 함수들을 하나로 합치는 작업
    public void SoundManager(string Sound)
	{
        float sound = SoundValue(Sound);

        if (sound == -40f) mixer.SetFloat(Sound, -80f);
        else mixer.SetFloat(Sound, sound);
	}

    private float SoundValue(string Sound)
	{
        float sound;

        if (Sound == "Master")
        {
            sound = slider.value;
            SaveController.Getinstance.Sound1 = sound;
        }
        else if (Sound == "BGM")
        {
            sound = slider2.value;
            SaveController.Getinstance.Sound2 = sound;
        }
        else if (Sound == "FX")
        {
            sound = slider3.value;
            SaveController.Getinstance.Sound3 = sound;
        }
        else
            sound = 0;

        return sound;
	}
}
