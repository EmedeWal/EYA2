using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GhostUltimatePerk", menuName = "Perks/UltimatePerks/GhostUltimate")]
public class GhostUltimatePerk : PerkData
{
    [Header("CLONE SETTINGS")]
    [SerializeField] private CreatureAI _clonePrefab;
    [SerializeField] private int _cloneCount = 1; // spawn default 1 clone


    private List<CreatureAI> _clones;

    private Mana _mana;

    public override void Init(List<PerkData> perks, GameObject playerObject)
    {
        base.Init(perks, playerObject);

        for (int i = perks.Count - 1; i >= 0; i--)
        {
            PerkData perk = perks[i];
            if (perk.GetType() == GetType())
            {
                perk.Deactivate();
                perks.RemoveAt(i);
            }
        }

        _clones = new List<CreatureAI>();

        _mana = _PlayerObject.GetComponent<Mana>();
    }

    public override void Activate()
    {
        for (int i = 0; i < _cloneCount; i++)
        {
            // GET RANDOM SPAWN POSITION, BUT FOR NOW THIS WORKS.
            Vector3 spawnPosition = _PlayerTransform.position - new Vector3(-5, 0, 0);
            Quaternion spawnRotation = Quaternion.identity;

            CreatureAI currentClone = Instantiate(_clonePrefab, spawnPosition, spawnRotation);
            _clones.Add(currentClone);
            currentClone.Init();
        }

        _mana.ValueExhausted += GhostUltimatePerk_ValueExhausted;

        _mana.RemoveConstantValue(10);
    }


    public override void Tick(float delta)
    {
        for (int i = 0; i < _clones.Count; i++)
        {
            _clones[i].Tick(delta);
        }
    }

    public override void Deactivate()
    {
        for (int i = 0; i < _clones.Count; i++)
        {
            _clones[i].Cleanup();
        }

        _clones.Clear();
    }

    private void GhostUltimatePerk_ValueExhausted()
    {
        _mana.ValueExhausted -= GhostUltimatePerk_ValueExhausted;

        Deactivate();
    }
}
