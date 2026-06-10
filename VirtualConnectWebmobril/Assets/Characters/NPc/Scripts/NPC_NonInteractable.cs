using UnityEngine;

public class NPC_NonInteractable : NPC
{
    private Animator _animator;
    private int _respectiveTypeeAnimHash;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _respectiveTypeeAnimHash = Animator.StringToHash("NPC_Type_ID");
        SetAnim();
    }
    public override void SetAnim()
    {
        _animator.SetInteger(_respectiveTypeeAnimHash,(int)NPC_Type);
    }
}
