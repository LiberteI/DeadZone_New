using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class RaiderMeleeManager : BaseMeleeManager
{
    public bool isInCoroutine;

    [SerializeField] private float comboTimer;

    [SerializeField] private Coroutine curAttack;
    void Start()
    {
        comboTimer = 0;
    }
    void Update()
    {
        base.SetData();

        TryUpdateComboStep();
    }

    private void TryUpdateComboStep()
    {
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;

            return;
        }
    }
    public void Attack()
    {
        // do not attack if is attacking
        if (curAttack != null)
        {
            return;
        }

        // if in combo time, execute melee 2
        if (comboTimer > 0)
        {
            curAttack = StartCoroutine(ExecuteMeleeAttack("Melee2"));

            comboTimer = 0;
            return;
        }
        curAttack = StartCoroutine(ExecuteMeleeAttack("Melee1"));
        
        comboTimer = 2;
    }

    private IEnumerator ExecuteMeleeAttack(string name)
    {
        isInCoroutine = true;

        survivor.parameter.animator.Play(name);

        yield return WaitForAnimationEnds(name);

        isInCoroutine = false;

        curAttack = null;
    }

    private IEnumerator WaitForAnimationEnds(string name)
    {
        while (true)
        {
            AnimatorStateInfo info = survivor.parameter.animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName(name))
            {
                break;
            }
            yield return null;
        }

        while (true)
        {
            AnimatorStateInfo info = survivor.parameter.animator.GetCurrentAnimatorStateInfo(0);

            if (info.normalizedTime > 0.99f)
            {
                break;
            }
            yield return null;
        }
    }
}
