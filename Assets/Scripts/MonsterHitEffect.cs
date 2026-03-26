using UnityEngine;
using System.Collections;

public class MonsterHitEffect : MonoBehaviour
{
    private Renderer[] _renderers;
    private MaterialPropertyBlock _propBlock;

    [SerializeField] private string _flashAmountName = "_FlashAmount";
    private int _flashAmountId;

    void Awake()
    {

        _renderers = GetComponentsInChildren<Renderer>();

        _propBlock = new MaterialPropertyBlock();
        _flashAmountId = Shader.PropertyToID(_flashAmountName);
    }

    public void PlayHitFlash()
    {
        StopAllCoroutines();
        StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {

        SetFlashValue(1.0f);
        yield return new WaitForSeconds(0.1f);

        SetFlashValue(0.0f);
    }

    private void SetFlashValue(float value)
    {
        _propBlock.SetFloat(_flashAmountId, value);

        foreach (var r in _renderers)
        {
            r.SetPropertyBlock(_propBlock);
        }
    }
}