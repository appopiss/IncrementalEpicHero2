using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameControllerUI;

public enum SoundEffect
{
    LevelUp,
    DropResource,
    DropEquipment,
    Click,
    Equip,
    Remove,
    Upgrade,
    QuestClaim,
    Alchemise,
    Map,
    AreaInfo,
    AreaSelect,
}
public enum AttackSound
{
    Slash,
}
public class SoundUI : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioClip[] audioClips;
    public AudioClip[] attacks;

    public void Play(SoundEffect se)
    {
        Play(audioClips[(int)se]);
    }
    public void Play(AttackSound sound)
    {
        Play(attacks[(int)sound]);
    }
    void Play(AudioClip clip)
    {
        if (main.S.isToggleOn[(int)ToggleKind.SFX] && main.S.sfxVolume > 0)
            sfxSource.PlayOneShot(clip);
    }
    //public void MustPlaySound(AudioClip Clip)
    //{
    //    //if (main.S.seSliderValue > 0)
    //    //{
    //        audioSource.PlayOneShot(Clip);
    //    //}
    //}

    public void ChangeSFXVolume(float vol)
    {
        sfxSource.volume = vol;
    }

    public void ChangeBGMVolume(float vol)
    {
        bgmSource.volume = vol;
    }

    // Start is called before the first frame update
    void Start()
    {
        //ChangeBGMVolume(main.S.bgmVolume);
        //ChangeSFXVolume(main.S.sfxVolume);
    }
}
