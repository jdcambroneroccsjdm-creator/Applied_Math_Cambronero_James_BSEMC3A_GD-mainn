using UnityEngine;
using TMPro;
using DG.Tweening;
public class DamageTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageTxt;
    [SerializeField] private GameObject critIcon;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color critColor;
    [SerializeField] private Vector3 startPos;

 

    public void Initialize()
{
    damageTxt = transform.Find("DamageTxt").GetComponent<TextMeshProUGUI>();
    damageTxt.text = string.Empty;
    transform.localScale = Vector3.one;
}

    public void ShowDamage(float damage, bool isCrit, Transform location)
    {
        var pos = Camera.main.WorldToScreenPoint(location.position + offset);
        transform.position = pos;

        Sequence sequence = DOTween.Sequence();

        if (isCrit)
        {
            damageTxt.transform.localScale = Vector3.one * 5f;
            critIcon.SetActive(true);
        }
        else
        {
            damageTxt.transform.localScale = Vector3.one * 3f;
            critIcon.SetActive(false);
        }

        damageTxt.color = isCrit ? critColor : normalColor;
        damageTxt.text = Mathf.RoundToInt(damage).ToString();

        sequence.Join(damageTxt.DOFade(1f,2f).SetEase(Ease.OutSine));
        sequence.Join(damageTxt.transform.DOScale(Vector3.zero,1f).SetEase(Ease.OutSine));
        sequence.Join(damageTxt.transform.DOMove(new Vector3(Random.Range(-50,50),100f,0) + pos,2f).SetEase(Ease.OutSine));

        sequence.Join(damageTxt.DOFade(0,1f).SetEase(Ease.OutSine));
        sequence.OnComplete(() => { Destroy(gameObject); });
    }
}
