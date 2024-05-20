using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastManager : MonoBehaviour
{

    [SerializeField] private BasicArrowSpell basicArrowSpell;
    [SerializeField] private SummonVillageWarriorSpell summonVillageWarriorSpell;
    private Dictionary<Spell, float> spellCooldowns = new Dictionary<Spell, float>();
    private Dictionary<KeyCode, Spell> keyCodeToSpell = new Dictionary<KeyCode,Spell>();
    void Awake()
    {
        spellCooldowns[basicArrowSpell] = basicArrowSpell.GetCooldown();
        spellCooldowns[summonVillageWarriorSpell] = summonVillageWarriorSpell.GetCooldown();
        keyCodeToSpell[KeyCode.Q] = summonVillageWarriorSpell;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpellsCooldowns();
    }

    public void TryToShootBasicArrow(Quaternion rotation)
    {
        if (spellCooldowns[basicArrowSpell] == 0)
        {
            print("instantiate!");
            BasicArrowSpell arrow =  Instantiate(basicArrowSpell, Constants.BowPosition, rotation);
            arrow.Cast(InputManager.Instance.GetMousePosition());
            spellCooldowns[basicArrowSpell] = basicArrowSpell.GetCooldown();
        }
    }

    public void TryToCastSpell(KeyCode keyCode)
    {
        Spell spell = keyCodeToSpell[keyCode];
        if (spellCooldowns[spell] == 0)
        { 
            spell.Cast(InputManager.Instance.GetMousePosition());
            spellCooldowns[spell] = spell.GetCooldown();
        }
        else
        {
            print("spell on cooldown !");
        }
    }

    private void UpdateSpellsCooldowns()
    {
        List<Spell> keys = new List<Spell>(spellCooldowns.Keys);

        // Iterate over the list of keys and update the values in the dictionary
        foreach (var key in keys)
        {
            spellCooldowns[key] = Mathf.Max(0, spellCooldowns[key] - Time.deltaTime);
        }
    }
    
}
