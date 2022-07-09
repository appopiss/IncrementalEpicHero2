using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UsefulMethod;
using static GameController;
using static SpriteSourceUI;
using System;
using Cysharp.Threading.Tasks;

public class BATTLE : RANGE
{
    public virtual bool isHero => false;//Hero/Pet? or Monster
    
    public HeroKind heroKind;
    public MonsterSpecies species;
    public MonsterColor color;
    public bool isAlive;
    public NUMBER currentHp;
    public NUMBER currentMp;
    public virtual double hp { get; }
    public virtual double mp { get; }
    public virtual double atk { get; }
    public virtual double matk { get; }
    public virtual double def { get; }
    public virtual double mdef { get; }
    public virtual double spd { get; }
    public virtual double fire { get; }
    public virtual double ice { get; }
    public virtual double thunder { get; }
    public virtual double light { get; }
    public virtual double dark { get; }
    public virtual double phyCrit { get; }
    public virtual double magCrit { get; }
    public virtual double critDamage { get => 2; }
    public virtual float moveSpeed { get; }//秒速
    public virtual double damageFactor { get => 1; }
    public virtual double DamageFactorElement(Element element) { return 1; }

    public virtual double debuffResistance { get => 0; }
    public virtual double physicalAbsorption { get => 0; }
    public virtual double fireAbsorption { get => 0; }
    public virtual double iceAbsorption { get => 0; }
    public virtual double thunderAbsorption { get => 0; }
    public virtual double lightAbsorption { get => 0; }
    public virtual double darkAbsorption { get => 0; }
    public virtual double physicalInvalidChance { get => 0; }
    public virtual double fireInvalidChance { get => 0; }
    public virtual double iceInvalidChance { get => 0; }
    public virtual double thunderInvalidChance { get => 0; }
    public virtual double lightInvalidChance { get => 0; }
    public virtual double darkInvalidChance { get => 0; }
    public virtual double maxInvalidChance => 0.50d;
    public virtual double golemInvalidChanceDamageHpPercent { get => 0; }
    public Debuffing[] debuffings = new Debuffing[Enum.GetNames(typeof(Debuff)).Length];
    //public bool[] isDebuff
        //= new bool[Enum.GetNames(typeof(Debuff)).Length];
    public double DebuffFactor(Debuff kind)
    {
        switch (kind)
        {
            case Debuff.Stop:
                if (color == MonsterColor.Boss || species == MonsterSpecies.ChallengeBoss) return 1d - Convert.ToInt32(debuffings[(int)kind].isDebuff) * 0.90d;
                return 1d - Convert.ToInt32(debuffings[(int)kind].isDebuff);
            case Debuff.FireResDown:
                return -Convert.ToInt32(debuffings[(int)kind].isDebuff);
            case Debuff.IceResDown:
                return -Convert.ToInt32(debuffings[(int)kind].isDebuff);
            case Debuff.ThunderResDown:
                return -Convert.ToInt32(debuffings[(int)kind].isDebuff);
            case Debuff.LightResDown:
                return -Convert.ToInt32(debuffings[(int)kind].isDebuff);
            case Debuff.DarkResDown:
                return -Convert.ToInt32(debuffings[(int)kind].isDebuff);
        }
        if (color == MonsterColor.Boss || species == MonsterSpecies.ChallengeBoss) return 1d - Convert.ToInt32(debuffings[(int)kind].isDebuff) * 0.25d;
        return 1d - Convert.ToInt32(debuffings[(int)kind].isDebuff) * 0.50d;
    }
    //Monster, Pet用
    public List<Attack> attack = new List<Attack>();

    public virtual void Activate()
    {
        isAlive = true;        
        currentHp.ChangeValue(hp);
        currentMp.ChangeValue(0);
        ResetCooltime();
        CureDebuff();
    }
    public virtual void ResetCooltime() { }
    public void CureDebuff()
    {
        for (int i = 0; i < debuffings.Length; i++)
        {
            debuffings[i].Cure();
        }
        poisonDamagePerSec = 0;
    }

    public void Inactivate()
    {
        move.Initialize();
        isPilfered = false;
        isAlive = false;
    }

    public virtual Vector2 MoveDirection()
    {
        return Vector2.zero;
    }

    //Attacked
    public double DamageCutRate(double originDamage, Element element, bool isPreventRNG)
    {
        double tempValue = 1;
        switch (element)
        {
            case Element.Physical:
                if (!isPreventRNG && physicalInvalidChance > 0 && WithinRandom(Math.Min(maxInvalidChance, physicalInvalidChance))) return -1;
                tempValue *= 1 - def / (originDamage + def);
                break;
            case Element.Fire:
                if (!isPreventRNG && fireInvalidChance > 0 && WithinRandom(Math.Min(maxInvalidChance, fireInvalidChance))) return -1;
                tempValue *= 1 - mdef / (originDamage + mdef);
                tempValue *= 1 - Math.Min(0.9d, fire);
                break;
            case Element.Ice:
                if (!isPreventRNG && iceInvalidChance > 0 && WithinRandom(Math.Min(maxInvalidChance, iceInvalidChance))) return -1;
                tempValue *= 1 - mdef / (originDamage + mdef);
                tempValue *= 1 - Math.Min(0.9d, ice);
                break;
            case Element.Thunder:
                if (!isPreventRNG && thunderInvalidChance > 0 && WithinRandom(Math.Min(maxInvalidChance, thunderInvalidChance))) return -1;
                tempValue *= 1 - mdef / (originDamage + mdef);
                tempValue *= 1 - Math.Min(0.9d, thunder);
                break;
            case Element.Light:
                if (!isPreventRNG && lightInvalidChance > 0 && WithinRandom(Math.Min(maxInvalidChance, lightInvalidChance))) return -1;
                tempValue *= 1 - mdef / (originDamage + mdef);
                tempValue *= 1 - Math.Min(0.9d, light);
                break;
            case Element.Dark:
                if (!isPreventRNG && darkInvalidChance > 0 && WithinRandom(Math.Min(maxInvalidChance, darkInvalidChance))) return -1;
                tempValue *= 1 - mdef / (originDamage + mdef);
                tempValue *= 1 - Math.Min(0.9d, dark);
                break;
        }
        return tempValue;
    }
    double tempValue;
    public double CalculateDamage(double originDamage, Element element, bool isCrit, bool isPreventRNG = false)//isPreventRNGはstatPopupの表示用
    {
        tempValue = originDamage;
        tempValue *= DamageCutRate(originDamage, element, isPreventRNG);
        if (tempValue < 0) return 0;
        tempValue *= damageFactor;
        tempValue *= DamageFactorElement(element);//Damage%はDefenceの計算後に乗算する
        tempValue = Math.Max(1d, tempValue);//最小は1
        if (isCrit) tempValue *= critDamage;//Criticalの場合は1を超える
        if (color == MonsterColor.Metal) tempValue += game.questCtrl.TitleEffectValue(battleCtrl.heroKind, TitleKind.MetalHunter).main;
        if (tempValue <= golemInvalidChanceDamageHpPercent * hp) tempValue = 0;
        return tempValue;
    }
    bool isSlayerOil;
    double calculatedDamage;
    double elementAbsorptionValue;
    double totalDamage;
    public virtual void Attacked(double damage, int hitCount, bool isCrit, Element element, Debuff debuff)
    {
        if (damage <= 0 || Double.IsNaN(damage)) return;
        isSlayerOil = !isHero && battleCtrl.CurrentSlayerElement() != Element.Physical;
        if (isSlayerOil) element = battleCtrl.CurrentSlayerElement();
        calculatedDamage = CalculateDamage(damage, element, isCrit);
        if (calculatedDamage <= 0)
        {
            if (game.IsUI(battleCtrl.heroKind) && damageTextUIAction != null)
                damageTextUIAction("<size=24><color=#00ffff>Damage Nullified!");
            return;
        }
        ReceiveDebuff(debuff, calculatedDamage);
        totalDamage = calculatedDamage * hitCount;
        if (debuffings[(int)Debuff.Electric].isDebuff)
            totalDamage += calculatedDamage * hitCount * 0.1d;
        if (isSlayerOil)
            totalDamage += calculatedDamage * hitCount * game.statsCtrl.ElementSlayerDamage(battleCtrl.heroKind, element).Value();
        currentHp.Decrease(totalDamage);
        if (!isHero)//Monsterが受けたダメージはTotalDPSに登録する
            battleCtrl.totalDamage.Increase(totalDamage);
        //UI
        if (game.IsUI(battleCtrl.heroKind) && damageTextUIAction != null)
        {
            string tempString = optStr;
            if (element == Element.Physical)
            {
                if (isCrit) tempString += "<color=orange><size=30><b>";
                else tempString += "<color=white><size=24>";
            }
            else
            {
                if (isCrit) tempString += "<color=red><size=30><b>";
                else tempString += "<color=yellow><size=24>";
            }
            if (hitCount < 5)
            {
                for (int i = 0; i < hitCount; i++)
                {
                    tempString += optStr + tDigit(calculatedDamage, 1) + "\n ";
                    for (int j = 0; j < i; j++)
                    {
                        tempString += " ";
                    }
                }
            }
            else
                tempString += optStr + "<b>" + tDigit(calculatedDamage, 1) + " x" + tDigit(hitCount) + "</b>";
            if (debuffings[(int)Debuff.Electric].isDebuff) tempString += "\n  <color=yellow><size=20>" + tDigit(calculatedDamage * hitCount * 0.1, 1);
            if (isSlayerOil) tempString += "\n  <color=orange><size=20>" + tDigit(calculatedDamage * hitCount * game.statsCtrl.ElementSlayerDamage(heroKind, element).Value(), 1);
            damageTextUIAction(tempString);
        }
        if (HpPercent() <= 0) return;
        //Absorption
        elementAbsorptionValue = calculatedDamage * hitCount;
        switch (element)
        {
            case Element.Physical: elementAbsorptionValue *= physicalAbsorption; break;
            case Element.Fire: elementAbsorptionValue *= fireAbsorption; break;
            case Element.Ice: elementAbsorptionValue *= iceAbsorption; break;
            case Element.Thunder: elementAbsorptionValue *= thunderAbsorption; break;
            case Element.Light: elementAbsorptionValue *= lightAbsorption; break;
            case Element.Dark: elementAbsorptionValue *= darkAbsorption; break;
        }
        if (elementAbsorptionValue > 0.1) Heal(elementAbsorptionValue);
    }
    //Debuff
    public void ReceiveDebuff(Debuff kind, double damage)
    {
        if (WithinRandom(debuffResistance)) return;
        debuffings[(int)kind].StartDebuff();
        if (kind == Debuff.Poison) poisonDamagePerSec = Math.Max(poisonDamagePerSec, damage);
        if (kind == Debuff.Knockback) move.Knockback(move.position - Target().move.position);
    }

    public void Heal(double increment)
    {
        currentHp.Increase(increment);
        if (game.IsUI(battleCtrl.heroKind) && damageTextUIAction != null) damageTextUIAction(optStr + "<color=green><size=26>+ " + tDigit(increment, 1));
    }
    public void FullHeal()
    {
        currentHp.Increase(hp);
        if (game.IsUI(battleCtrl.heroKind) && damageTextUIAction != null) damageTextUIAction(optStr + "<color=green><size=26>+ " + tDigit(hp, 1));
    }
    public virtual void Regenerate(float deltaTime) { }
    public double poisonDamagePerSec = 0;
    //float countSec;
    public virtual void Poison()
    {
        if (!debuffings[(int)Debuff.Poison].isDebuff) return;
        currentHp.Decrease(poisonDamagePerSec);
        if (game.IsUI(battleCtrl.heroKind) && damageTextUIAction != null) damageTextUIAction("<color=purple><size=24>" + tDigit(poisonDamagePerSec, 1));

    }

    void UpdateCheckDefeated()
    {
        //画面下方にいる場合は強制Dead
        if (move.position.y < - Parameter.moveRangeY * 0.55) DeadAction();
        //NaNバグがたまーに発生するため、ここでチェック
        if (currentHp.value <= 0 || Double.IsNaN(currentHp.value)) DeadAction();
    }

    public virtual void DeadAction() { }
    public bool isPilfered;
    public virtual void Pilfer(double chance) { }
    Action<string> damageTextUIAction;
    public void SetDamageTextUI(Action<string> damageTextUIAction)
    {
        this.damageTextUIAction = damageTextUIAction;
    }


    public float HpPercent()
    {
        if (hp <= 0 || currentHp.value <= 0) return 0;
        return (float)(currentHp.value / hp);
    }
    public float MpPercent()
    {
        if (mp <= 0 || currentMp.value <= 0) return 0;
        return (float)(currentMp.value / mp);
    }

    //Target系
    public BATTLE ShortestTarget(BATTLE origin, params BATTLE[] targets)
    {
        BATTLE tempTarget = targets[0];
        float minDistance = 10000f;
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].isAlive)
            {
                if (Distance(origin, targets[i]) < minDistance)
                {
                    minDistance = Distance(origin, targets[i]);
                    tempTarget = targets[i];
                }
            }
        }
        return tempTarget;
    }
    public BATTLE FurthestTarget(BATTLE origin, params BATTLE[] targets)
    {
        BATTLE tempTarget = targets[0];
        float maxDistance = 0f;
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].isAlive)
            {
                if (Distance(origin, targets[i]) > maxDistance)
                {
                    maxDistance = Distance(origin, targets[i]);
                    tempTarget = targets[i];
                }
            }
        }
        return tempTarget;
    }
    public BATTLE RandomTarget(BATTLE origin, params BATTLE[] targets)
    {
        List<BATTLE> tempTargetList = new List<BATTLE>();
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].isAlive)
            {
                tempTargetList.Add(targets[i]);
            }
        }
        BATTLE[] battleArray = tempTargetList.ToArray();
        if (battleArray.Length <= 0) return targets[0];
        return battleArray[UnityEngine.Random.Range(0, battleArray.Length)];
    }
    public virtual BATTLE[] TargetArray() { return battleCtrl.monstersList.ToArray(); }
    public virtual BATTLE Target() { return this; }
    public virtual BATTLE FurthestTarget() { return this; }
    public virtual BATTLE RandomTarget() { return this; }
    public bool IsInputDirection(Direction direction)
    {
        if (GameControllerUI.isShiftPressed) return false;
        switch (direction)
        {
            case Direction.up:
                return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            case Direction.right:
                return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
            case Direction.down:
                return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            case Direction.left:
                return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        }
        return false;
    }

    public virtual void GetExp(double value) { }

    public void Update(float deltaTime)
    {
        if (!isAlive) return;
        UpdateMove(deltaTime);
        UpdateSkillCooltime(deltaTime);
        UpdateTriggerSkill();
        UpdateCheckDefeated();
        Regenerate(deltaTime);
    }
    public void UpdatePerSec()
    {
        Poison();
    }
    void UpdateMove(float deltaTime)
    {
        Move(MoveDirection(), deltaTime);
    }
    public void Move(Vector2 moveDirection, float deltatime)
    {
        move.MovePosition(moveDirection, deltatime);
    }
    public virtual void UpdateTriggerSkill() { }
    public virtual void UpdateSkillCooltime(float deltaTime) { }
    public virtual void TriggerRandomSkill(BATTLE battle, double profGainPercent)//petのOdeOfFriendship用
    {

    }

    public virtual bool CanShowStats(HeroKind heroKind) { return false; }
    public virtual bool TryCapture(HeroKind heroKind, bool isTamerSkill = false) { return false; }
    public virtual void Capture(HeroKind heroKind) { }

}

//PetとEnemy
public class MONSTER_BATTLE : BATTLE
{
    //Challenge
    public virtual ChallengeMonsterKind challengeMonsterKind => ChallengeMonsterKind.SlimeKing;
    public AttackColor currentAttackColor;

    public virtual bool isPet { get => false; }
    public virtual MonsterGlobalInformation globalInformation { get => game.monsterCtrl.GlobalInformation(species, color); }
    public long level = 1;
    public double difficulty = 0;
    public Element attackElement { get => globalInformation.AttackElement(); }
    public override double hp { get => globalInformation.Hp(level, difficulty, isPet, battleCtrl.heroKind) * (1 + 4 * Convert.ToInt16(isMutant)); }
    public override double mp { get => globalInformation.Mp(level, difficulty, isPet, battleCtrl.heroKind); }
    public override double atk { get => DebuffFactor(Debuff.AtkDown) * globalInformation.Atk(level, difficulty, isPet, battleCtrl.heroKind) * (1 + Convert.ToInt16(isMutant)); }
    public override double matk { get => DebuffFactor(Debuff.MatkDown) * globalInformation.MAtk(level, difficulty, isPet, battleCtrl.heroKind) * (1 + Convert.ToInt16(isMutant)); }
    public override double def { get => DebuffFactor(Debuff.DefDown) * globalInformation.Def(level, difficulty, isPet, battleCtrl.heroKind) * (1 + Convert.ToInt16(isMutant)); }
    public override double mdef { get => DebuffFactor(Debuff.MdefDown) * globalInformation.MDef(level, difficulty, isPet, battleCtrl.heroKind) * (1 + Convert.ToInt16(isMutant)); }
    public override double spd { get => DebuffFactor(Debuff.SpdDown) * DebuffFactor(Debuff.Stop) * globalInformation.Spd(level, difficulty, isPet, battleCtrl.heroKind); }
    public override double fire { get => globalInformation.Fire(level, difficulty, isPet, battleCtrl.heroKind) + DebuffFactor(Debuff.FireResDown); }
    public override double ice { get => globalInformation.Ice(level, difficulty, isPet, battleCtrl.heroKind) + DebuffFactor(Debuff.IceResDown); }
    public override double thunder { get => globalInformation.Thunder(level, difficulty, isPet, battleCtrl.heroKind) + DebuffFactor(Debuff.ThunderResDown); }
    public override double light { get => globalInformation.Light(level, difficulty, isPet, battleCtrl.heroKind) + DebuffFactor(Debuff.LightResDown); }
    public override double dark { get => globalInformation.Dark(level, difficulty, isPet, battleCtrl.heroKind) + DebuffFactor(Debuff.DarkResDown); }
    public override double phyCrit { get => globalInformation.PhyCrit(level, difficulty, isPet, battleCtrl.heroKind); }
    public override double magCrit { get => globalInformation.MagCrit(level, difficulty, isPet, battleCtrl.heroKind); }
    public override double critDamage { get => game.statsCtrl.HeroStats(heroKind, Stats.CriticalDamage).Value(); }
    public override float range { get => globalInformation.Range(); }
    public override float moveSpeed { get => (float)DebuffFactor(Debuff.SpdDown) * (float)DebuffFactor(Debuff.Stop) * globalInformation.MoveSpeed(level, difficulty, isPet, battleCtrl.heroKind); }
    public override double damageFactor { get => game.statsCtrl.MonsterDamage(battleCtrl.heroKind, species).Value(); }
    public override double DamageFactorElement(Element element)
    {
        return game.statsCtrl.ElementDamage(battleCtrl.heroKind, element).Value();
    }
    public double exp { get => globalInformation.Exp(level, difficulty) * (1 + Convert.ToInt16(isMutant)); }
    public double gold { get => globalInformation.Gold(level, difficulty) * (1 + Convert.ToInt16(isMutant)); }
    public double resource { get => 1; }
    public double attackIntervalSec { get => mp / AttackSpeed(); }
    public Debuff debuff { get => globalInformation.Debuff(); }
    public double debuffChance { get => globalInformation.DebuffChance(level, difficulty, isPet, battleCtrl.heroKind); }
    public Debuff LotteryDebuff()
    {
        if (WithinRandom(debuffChance)) return debuff;
        return Debuff.Nothing;
    }

    public MONSTER_BATTLE(BATTLE_CONTROLLER battleCtrl)
    {
        this.battleCtrl = battleCtrl;
        move = new Move(Parameter.hidePosition, () => moveSpeed, false, this);
        currentHp = new NUMBER(() => hp);
        currentMp = new NUMBER(() => mp);
        for (int i = 0; i < debuffings.Length; i++)
        {
            int count = i;
            debuffings[i] = new Debuffing(this, (Debuff)count);
        }
        heroKind = battleCtrl.heroKind;
    }
    public bool isMutant;
    public void Spawn(MonsterSpecies species, MonsterColor color, long level, double difficulty, Vector2 position, bool isMutant = false)
    {
        this.species = species;
        this.color = color;
        this.level = level;
        this.difficulty = difficulty;
        this.isMutant = isMutant;
        isCaptured = false;
        Activate();
        if (game.IsUI(battleCtrl.heroKind) && spawnUIAction != null) spawnUIAction();
        move.MoveTo(position);
        SetAttack();
    }

    public override Vector2 MoveDirection()
    {
        if (!isAlive) return Vector2.zero;
        if (IsWithinRange(this, Target(), range)) return Vector2.zero;
        if (!Target().isAlive) return Vector2.zero;
        return Target().move.position - move.position;
    }
    public override void DeadAction()
    {
        battleCtrl.areaBattle.simulatedGold += gold * game.statsCtrl.GoldGain().Value() + game.upgradeCtrl.Upgrade(UpgradeKind.GoldGain, 0).EffectValue();
        battleCtrl.areaBattle.simulatedExp += CalculatedExp() * game.statsCtrl.HeroStats(battleCtrl.heroKind, Stats.ExpGain).Value() + game.upgradeCtrl.Upgrade(UpgradeKind.ExpGain, 0).EffectValue();
        FinishAction();
    }
    public void FinishAction()
    {
        //活動終了
        Inactivate();
        //次のWaveへ
        battleCtrl.areaBattle.CheckToNextWave();
    }
    public bool isCaptured;
    public override BATTLE Target()
    {
        return ShortestTarget(this, battleCtrl.heroesList.ToArray());
    }
    public double CalculatedExp()
    {
        double tempExp = exp;
        long tempSubLevel = level - game.statsCtrl.Level(battleCtrl.heroKind);
        if (tempSubLevel < -30) tempExp *= 1d;
        else if (tempSubLevel < -20) tempExp *= 1.15d;
        else if (tempSubLevel < -10) tempExp *= 1.30d;
        else if (tempSubLevel <= 10) tempExp *= 1.50d;
        else if (tempSubLevel <= 20) tempExp *= 1.30d;
        else if (tempSubLevel <= 30) tempExp *= 1.15d;
        else if (tempSubLevel <= 50) tempExp *= 1.00d;
        else if (tempSubLevel <= 75) tempExp *= 0.75d;
        else if (tempSubLevel <= 100) tempExp *= 0.50d;
        else if (tempSubLevel <= 200) tempExp *= 0.25d;
        else if (tempSubLevel <= 300) tempExp *= 0.10d;
        else tempExp *= 0.01d;
        if (color == MonsterColor.Metal) tempExp *= 1 + game.questCtrl.TitleEffectValue(battleCtrl.heroKind, TitleKind.MetalHunter).sub;
        return tempExp;
    }
    public double gainFactor { get => game.guildCtrl.Member(battleCtrl.heroKind).gainRate; }

    public Action spawnUIAction;
    public void SetSpawnUI(Action spawnUIAction)
    {
        this.spawnUIAction = spawnUIAction;
    }

    public virtual void SetAttack()
    {
        if (attack.Count > 0) return;
        attack.Add(new Attack(battleCtrl, battleCtrl.heroesList, (x) => Target().move.position, (x) => Damage(), IsCrit, () => 40f, () => 1, () => attackElement, () => LotteryDebuff()));
    }
    public override void UpdateTriggerSkill()
    {
        if (CanAttack())
        {
            Attack();
            currentMp.ChangeValue(0); 
        }
    }
    public void Attack(double damageMultiplier = 1)
    {
        attack[0].NormalAttack(this, 0, damageMultiplier);
        //UI
        if (game.IsUI(battleCtrl.heroKind) && skillEffectUIAction != null)
            skillEffectUIAction(attack[0], sprite.elementAttackEffects[(int)attackElement]);
    }
    public bool CanAttack()
    {
        if (!IsWithinRange(this, Target(), range)) return false;
        if (!isMpCharged) return false;
        return true;
    }
    public bool isMpCharged => currentMp.value >= mp;

    public double Damage() { return globalInformation.Damage(level, difficulty, isPet, battleCtrl.heroKind); }
    public bool IsCrit()
    {
        if (attackElement == Element.Physical)
        {
            if (phyCrit <= 0) return false;
            return WithinRandom(phyCrit);
        }
        else
        {
            if (magCrit <= 0) return false;
            return WithinRandom(magCrit);
        }
    }
    double AttackSpeed()
    {
        return spd;
    }
    public override void UpdateSkillCooltime(float deltaTime)
    {
        //Cooltimeの代わりにmpを使う
        currentMp.Increase(AttackSpeed() * deltaTime);
    }
    public Action<Attack, Sprite> skillEffectUIAction;
    public Action<Attack, AnimationEffectKind> animationEffectUIAction;
    public Action<Attack, ParticleEffectKind> particleEffectUIAction;
    public List<Action> attackUIActionList = new List<Action>(); 
    public void SetSkillEffectUIAction(Action<Attack, Sprite> skillEffectUIAction)
    {
        this.skillEffectUIAction = skillEffectUIAction;
    }
    public void SetSkillAnimationEffectUIAction(Action<Attack, AnimationEffectKind> animationEffectUIAction)
    {
        this.animationEffectUIAction = animationEffectUIAction;
    }
    public void SetSkillParticleEffectUIAction(Action<Attack, ParticleEffectKind> particleEffectUIAction)
    {
        this.particleEffectUIAction = particleEffectUIAction;
    }
}

public class RANGE
{
    public BATTLE_CONTROLLER battleCtrl;
    public virtual float range { get; }
    public Move move;

    public float Distance(RANGE origin, RANGE target)
    {
        return vectorAbs(target.move.position - origin.move.position);
    }

    public bool IsWithinRange(RANGE origin, BATTLE target, float range)
    {
        if (!target.isAlive) return false;
        return Distance(origin, target) <= range;
    }
}

public class Attack : RANGE //有効範囲内に入るとダメージ
{
    public override float range { get => effectRange(); }
    public Func<BATTLE, double> damage;
    public Func<float> effectRange;
    public Func<int> hitCount;
    public Func<Element> element;
    public Func<Debuff> debuff;
    //public BATTLE[] target;
    public List<BATTLE> target;
    public Func<BATTLE, Vector2> initPosition;
    public Func<bool> isCrit;
    public int activeFrame = 5;
    public Func<Vector2> targetPosition;
    public Func<float> throwSpeed = () => 1500f;

    public Attack(BATTLE_CONTROLLER battleCtrl, List<BATTLE> target, Func<BATTLE, Vector2> initPosition, Func<BATTLE, double> damage, Func<bool> isCrit, Func<float> effectRange, Func<int> hitCount = null, Func<Element> element = null, Func<Debuff> debuff = null, Func<Vector2> targetPosition = null, Func<float> throwSpeed = null)
    {
        this.battleCtrl = battleCtrl;
        this.target = target;
        isAttacked = new bool[target.Count];
        this.initPosition = initPosition;
        this.damage = damage;
        this.isCrit = isCrit;
        this.hitCount = hitCount == null ? () => 1 : hitCount;
        this.element = element == null ? () => Element.Physical : element;
        this.debuff = debuff == null ? () => Debuff.Nothing : debuff;
        this.effectRange = effectRange;
        if (targetPosition != null) this.targetPosition = targetPosition;
        if (throwSpeed != null) this.throwSpeed = throwSpeed;
        move = new Move(Parameter.hidePosition, () => this.throwSpeed());
    }

    int frameCount = 0;
    bool isTriggered = false;
    bool isPenetrate = false;
    bool isToFinish = false;
    bool isLoopAttack = false;
    bool isThrowAttack = false;
    double damageMultiplier = 1;
    public void Update(float deltaTime)
    {
        if (move.IsInitialized()) return;
        if (isToFinish)
        {
            frameCount++;
            if (frameCount >= activeFrame)
            {
                move.Initialize();
                frameCount = 0;
                //終了
            }
            return;
        }

        if (isLoopAttack)
        {
            frameCount++;
            for (int i = 0; i < target.Count; i++)
            {
                if (IsWithinRange(this, target[i], range))
                {
                    if (frameCount % loopIntervalFrame == 0)//attack
                        target[i].Attacked(damage(battle) * damageMultiplier, hitCount(), isCrit(), element(), debuff());
                    if (isGravity && target[i].color != MonsterColor.Boss) target[i].move.Gravity(Vector2.zero, deltaTime * pullStrength);
                }
            }
            if (frameCount > loopActiveFrame) isToFinish = true;
        }
        else
        {
            if (isThrowAttack)
            {
                if (move.CanMove())
                {
                    //attackの処理
                    if (!isTriggered || isPenetrate)
                    {
                        for (int i = 0; i < target.Count; i++)
                        {
                            if (IsWithinRange(this, target[i], range) && !isAttacked[i])
                            {
                                target[i].Attacked(damage(battle) * damageMultiplier, hitCount(), isCrit(), element(), debuff());
                                isTriggered = true;
                                isAttacked[i] = true;
                            }
                        }
                    }
                    else isToFinish = true;
                    //move
                    move.MovePosition(throwVec, deltaTime, false, true);
                }
                else isToFinish = true;
            }
            else//NormalAttack
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (IsWithinRange(this, target[i], range) && !isAttacked[i])
                    {
                        target[i].Attacked(damage(battle) * damageMultiplier, hitCount(), isCrit(), element(), debuff());
                    }
                }
                isToFinish = true;
            }
        }
    }

    void Initialize()
    {
        isToFinish = false;
        isTriggered = false;
        for (int i = 0; i < isAttacked.Length; i++)
        {
            isAttacked[i] = false;
        }
    }
    bool[] isAttacked;
    public Vector2 throwVec = Vector2.zero;
    BATTLE battle;
    public void ThrowAttack(BATTLE battle, Func<Vector2> throwDirection = null, bool isPenetrate = false, double damageMultiplier = 1)//投擲
    {
        Initialize();
        this.battle = battle;
        //target =  battle.TargetArray();
        isThrowAttack = true;
        this.isPenetrate = isPenetrate;
        this.damageMultiplier = damageMultiplier;
        if (throwDirection != null) throwVec = throwDirection();
        else throwVec = targetPosition() - initPosition(battle);
        if (throwVec == Vector2.zero) throwVec = Vector2.up;
        //isTriggered = false;
        //for (int i = 0; i < isAttacked.Length; i++)
        //{
        //    isAttacked[i] = false;
        //}
        move.MoveTo(initPosition(battle));
        //throwVec = Vector2.up; 
        //int framecount = 0;
        //while (true)
        //{
        //    move.MovePosition(throwVec, battleCtrl.deltaTime, false, true);
        //    for (int i = 0; i < target.Count; i++)
        //    {
        //        if (IsWithinRange(this, target[i], range) && !isAttacked[i])
        //        {
        //            target[i].Attacked(damage(), hitCount(), isCrit(), element(), debuff());
        //            isAttacked[i] = true;
        //            isTriggered = true;
        //        }
        //    }
        //    if (!isPenetrate && isTriggered) break;
        //    if (!move.CanMove()) break;
        //    if(!battleCtrl.isSimulated) await UniTask.DelayFrame(1, PlayerLoopTiming.Update);
        //    framecount++;
        //    if (framecount >= 60 * 5) break;//5秒以上は持続しない
        //}
        //await UniTask.Delay((int)(battleCtrl.deltaTime * 1000));
        //move.Initialize();
    }
    public void NormalAttack(BATTLE battle, int activeFrame = 0, double damageMultiplier = 1)//通常攻撃
    {
        Initialize();
        //target = battle.TargetArray();
        this.battle = battle;
        this.damageMultiplier = damageMultiplier;
        move.MoveTo(initPosition(battle));
        if (activeFrame != 0) this.activeFrame = activeFrame;
        //for (int i = 0; i < target.Count; i++)
        //{
        //    if (IsWithinRange(this, target[i], range))
        //    {
        //        target[i].Attacked(damage(), hitCount(), isCrit(), element(), debuff());
        //    }
        //}
        //await UniTask.Delay(this.activeFrame * (int)(battleCtrl.deltaTime * 1000));
        //move.Initialize();
    }

    float loopActiveFrame = 5 * 60f;
    float loopIntervalFrame = 1 * 60f;
    bool isGravity = false;
    float pullStrength = 2.0f;
    public void LoopAttack(BATTLE battle, float activeSec = 5, float loopIntervalSec = 1, bool isGravity = false, float pullStrength = 2.0f, double damageMultiplier = 1)
    {
        Initialize();
        //target = battle.TargetArray();
        this.battle = battle;
        isLoopAttack = true;
        activeFrame = 0;
        loopActiveFrame = activeSec * 60f;
        loopIntervalFrame = loopIntervalSec * 60f;
        this.isGravity = isGravity;
        this.pullStrength = pullStrength;
        this.damageMultiplier = damageMultiplier;
        move.MoveTo(initPosition(battle));
        //for (int j = 0; j < activeSec / loopIntervalSec; j++)
        //{
        //    for (int i = 0; i < target.Count; i++)
        //    {
        //        if (IsWithinRange(this, target[i], range))
        //        {
        //            target[i].Attacked(damage(), hitCount(), isCrit(), element(), debuff());
        //            if (isGravity && target[i].color != MonsterColor.Boss) target[i].move.Gravity(Vector2.zero, loopIntervalSec * pullStrength);
        //        }
        //    }
        //    await UniTask.Delay((int)(loopIntervalSec * 1000));
        //}
        //move.Initialize();
    }
}

public class Move
{
    public Vector2 position;
    bool isHero;
    public bool isPet;
    BATTLE battle;

    public Move(Vector2 initPosition, Func<float> moveSpeed, bool isHero = false, BATTLE battle = null)
    {
        this.initPosition = initPosition;
        this.moveSpeed = moveSpeed;
        Initialize();
        this.isHero = isHero;
        if (battle != null) this.battle = battle;
    }
    float tempTotalDistance;
    float tempDistance;
    public void MovePosition(Vector2 direction, float deltaTime, bool isModify = true, bool isAttack = false)
    {
        if (direction == Vector2.zero) return;
        if (!CanMove()) return;
        if (isStun)
        {
            stunCount += deltaTime;
            if (stunCount >= stunTime)
            {
                stunCount = 0;
                isStun = false;
            }
        }
        else
        {
            tempDistance = 0;
            if (isAttack) tempDistance = moveSpeed() * deltaTime;
            else tempDistance = Mathf.Min(vectorAbs(direction), moveSpeed() * deltaTime);
            position += normalize(direction) * tempDistance;
            if (isHero)
            {
                if (!battle.battleCtrl.isSimulated)
                {
                    game.statsCtrl.MovedDistance(battle.heroKind, false).Increase(tempDistance);
                    game.statsCtrl.MovedDistance(battle.heroKind, true).Increase(tempDistance);
                    Main.main.S.totalMovedDistance[(int)battle.heroKind] += tempDistance;
                }
                tempTotalDistance += moveSpeed() * deltaTime;
                if (tempTotalDistance >= 1000)//Potion
                {
                    game.inventoryCtrl.ConsumePotion(PotionConsumeConditionKind.Move, battle, battle.battleCtrl.isSimulated);
                    tempTotalDistance = 0;
                }
            }
            if (isPet)
            {
                if (!battle.battleCtrl.isSimulated)
                {
                    Main.main.SR.movedDistancePet += tempDistance;
                    Main.main.S.movedDistancePet += tempDistance;
                    Main.main.S.totalMovedDistancePet += tempDistance;
                }
            }
            if (isModify && !CanMove()) ModifyPosition();
        }
    }
    float tempDistanceForKnockback;
    public void Knockback(Vector2 direction, float deltaTime = 1.0f, float stunTime = 0.5f)
    {
        if (direction == Vector2.zero) return;
        if (!CanMove()) return;
        if (isStun) return;
        tempDistanceForKnockback = moveSpeed() * deltaTime;
        position += normalize(direction) * tempDistanceForKnockback;
        if (!CanMove()) ModifyPosition();
        isStun = true;
        this.stunTime = stunTime;
    }
    float tempDistanceForGravity;
    public void Gravity(Vector2 centerPosition, float deltaTime = 1.0f)
    {
        if (!CanMove()) return;
        tempDistanceForGravity = Mathf.Min(vectorAbs(centerPosition - position), moveSpeed() * deltaTime);
        position += normalize(centerPosition - position) * tempDistanceForGravity;
        if (!CanMove()) ModifyPosition();
    }
    public bool isStun;
    float stunCount;
    float stunTime;

    public void Initialize()
    {
        MoveTo(initPosition);
        isStun = false;
    }
    public void MoveTo(Vector2 toPosition, bool isModify = false)
    {
        position = toPosition;
        if (isModify && !CanMove()) ModifyPosition();
    }
    public bool IsInitialized()
    {
        return position == initPosition;
    } 

    Vector2 initPosition;
    Func<float> moveSpeed;//秒速
    public bool CanMove()
    {
        //if (isStun) return false;
        for (int i = 0; i < Enum.GetNames(typeof(Direction)).Length; i++)
        {
            if (!CanMove((Direction)i)) return false;
        }
        return true;
    }
    bool CanMove(Direction direction)
    {
        switch (direction)
        {
            case Direction.up:
                return position.y <= Parameter.moveRangeY / 2;
            case Direction.right:
                return position.x <= Parameter.moveRangeX / 2;
            case Direction.down:
                return position.y >= -Parameter.moveRangeY / 2;
            case Direction.left:
                return position.x >= -Parameter.moveRangeX / 2;
        }
        return false;
    }
    float tempX;
    float tempY;
    void ModifyPosition()
    {
        tempX = position.x;
        tempY = position.y;
        if (!CanMove(Direction.up)) tempY = Parameter.moveRangeY / 2;
        if (!CanMove(Direction.right)) tempX = Parameter.moveRangeX / 2;
        if (!CanMove(Direction.down)) tempY = -Parameter.moveRangeY / 2;
        if (!CanMove(Direction.left)) tempX = -Parameter.moveRangeX / 2;
        position = new Vector2(tempX, tempY);
    }
}
