using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Main;
using static GameController;
using static GameControllerUI;
using static UsefulMethod;
using static SpriteSourceUI;
using System;

public enum AnimationEffectKind
{
    FireStorm,
}
public enum ParticleEffectKind
{
    SwingAround,
    FanSwing,
    MeteorStrike,
    ChillingTouch,
    Blizzard,
    LightningThunder,
    WingStorm,

    ShadowStrike,
    DarkWield,
    Assassination,

    //ChallengeBoss
    PoisonGas,
    Venom,
    GolemBomb,

    ShockArrow,
    ExplodingArrow,
    GravityArrow,

    SonicSlash,
}
public class SkillEffectUI : MonoBehaviour
{
    Func<BattleController> battleCtrl;
    [SerializeField] GameObject[] effects;
    [SerializeField] GameObject[] effectAnimations;
    [SerializeField] GameObject[] effectParticles;
    EffectUI[] effectsUI;
    EffectUI[] effectAnimationsUI;
    EffectUI[] effectParticlesUI;
    
    int currentId;

    public void SetUI(Attack attack, Sprite sprite = null)
    {
        if (main.S.isToggleOn[(int)ToggleKind.PerformanceMode]) return;
        if (SettingMenuUI.Toggle(ToggleKind.DisableAttackEffect).isOn) return;
        effectsUI[currentId].SetUI(attack.move, sprite, attack.range);
        currentId = currentId >= effects.Length - 1 ? 0 : currentId + 1;
    }
    public void SetUI(Attack attack, Sprite sprite, Func<Vector2> direction)
    {
        if (main.S.isToggleOn[(int)ToggleKind.PerformanceMode]) return;
        if (SettingMenuUI.Toggle(ToggleKind.DisableAttackEffect).isOn) return;
        effectsUI[currentId].SetUI(attack.move, sprite, attack.range, -Vector2.SignedAngle(direction(), Vector2.up));
        currentId = currentId >= effects.Length - 1 ? 0 : currentId + 1;
    }
    public void SetUI(Attack attack, AnimationEffectKind kind)
    {
        if (main.S.isToggleOn[(int)ToggleKind.PerformanceMode]) return;
        if (SettingMenuUI.Toggle(ToggleKind.DisableAttackEffect).isOn) return;
        effectAnimationsUI[(int)kind].SetUI(attack.move, null, attack.range);
    }
    public void SetUI(Attack attack, ParticleEffectKind kind)
    {
        if (main.S.isToggleOn[(int)ToggleKind.PerformanceMode]) return;
        if (SettingMenuUI.Toggle(ToggleKind.DisableAttackEffect).isOn) return;
        if (SettingMenuUI.Toggle(ToggleKind.DisableParticle).isOn) return;
        effectParticlesUI[(int)kind].SetUI(attack.move, null, attack.range);
    }
    public void UpdateUI()
    {
        if (gameUI.worldMapUI.thisOpenClose.isOpen) return;

        for (int i = 0; i < effectsUI.Length; i++)
        {
            effectsUI[i].UpdateUI();
        }
        for (int i = 0; i < effectAnimationsUI.Length; i++)
        {
            effectAnimationsUI[i].UpdateUI();
        }
        for (int i = 0; i < effectParticlesUI.Length; i++)
        {
            effectParticlesUI[i].UpdateUI();
        }
    }
    private void Awake()
    {
        battleCtrl = () => game.battleCtrl;
        effectsUI = new EffectUI[effects.Length];
        effectAnimationsUI = new EffectUI[effectAnimations.Length];
        effectParticlesUI = new EffectUI[effectParticles.Length];
        for (int i = 0; i < effectsUI.Length; i++)
        {
            effectsUI[i] = new EffectUI(effects[i]);
        }
        for (int i = 0; i < effectAnimationsUI.Length; i++)
        {
            effectAnimationsUI[i] = new EffectUI(effectAnimations[i], true);
        }
        for (int i = 0; i < effectParticlesUI.Length; i++)
        {
            effectParticlesUI[i] = new EffectUI(effectParticles[i], true, true);
        }
    }
    private void Start()
    {
        //Hero
        for (int i = 0; i < Enum.GetNames(typeof(HeroKind)).Length; i++)
        {
            int countI = i;
            for (int j = 0; j < Enum.GetNames(typeof(SkillKindWarrior)).Length; j++)
            {
                int countJ = j;
                game.skillCtrl.Skill((HeroKind)countI, countJ).SetSkillEffectUIAction(SetUI);
                game.skillCtrl.Skill((HeroKind)countI, countJ).effectUIActionWithDirection = SetUI;
                game.skillCtrl.Skill((HeroKind)countI, countJ).SetSkillAnimationEffectUIAction(SetUI);
                game.skillCtrl.Skill((HeroKind)countI, countJ).SetSkillParticleEffectUIAction(SetUI);
            }
        }
    }

    public void Initialize()
    {
        //Pet
        for (int i = 0; i < battleCtrl().pets.Length; i++)
        {
            battleCtrl().pets[i].SetSkillEffectUIAction(SetUI);
        }
        //Monster
        for (int i = 0; i < battleCtrl().monsters.Length; i++)
        {
            battleCtrl().monsters[i].SetSkillEffectUIAction(SetUI);
        }
        //ChallengeMonster
        for (int i = 0; i < battleCtrl().challengeMonsters.Length; i++)
        {
            battleCtrl().challengeMonsters[i].SetSkillEffectUIAction(SetUI);
            battleCtrl().challengeMonsters[i].SetSkillAnimationEffectUIAction(SetUI);
            battleCtrl().challengeMonsters[i].SetSkillParticleEffectUIAction(SetUI);
        }
    }
}


public class EffectUI
{
    GameObject thisObject;
    Move move;
    RectTransform thisRect;
    Image thisImage;
    Func<Vector2> position = () => Parameter.hidePosition;
    bool isAnimation;
    bool isParticle;
    ParticleSystem thisParticle;
    public EffectUI(GameObject gameObject, bool isAnimation = false, bool isParticle = false)
    {
        thisObject = gameObject;
        this.isAnimation = isAnimation;
        this.isParticle = isParticle;
        thisRect = gameObject.GetComponent<RectTransform>();
        thisImage = gameObject.GetComponent<Image>();
        if (isParticle) thisParticle = thisObject.GetComponent<ParticleSystem>();
        SetActive(thisObject, false);
    }
    public void SetUI(Move move, Sprite sprite, float size, float rotateZ = 0)
    {
        this.move = move;
        SetActive(thisObject, true);
        thisRect.eulerAngles = Vector3.forward * rotateZ;
        if (sprite != null) thisImage.sprite = sprite;
        position = () => move.position;
        thisRect.anchoredPosition = position();
        if (isAnimation || isParticle)
            thisRect.anchoredPosition = position();
        if (isParticle)
        {
            if (!thisParticle.isPlaying)
                thisParticle.Play();
        }
        else if (isAnimation)
        {
            SetActive(thisObject, false);
            SetActive(thisObject, true);
        }
        else
            thisRect.sizeDelta = Vector2.one * size;
        
    }
    public void UpdateUI()
    {
        if (thisObject.activeSelf)
        {
            //if (!isParticle)
            thisRect.anchoredPosition = position();
            if (move.IsInitialized()) SetActive(thisObject, false);
        }
    }
}
