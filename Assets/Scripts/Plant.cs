using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Plant : MonoBehaviour
{
    [Tooltip("Add parts in this List<> from top to bottom")]
    [SerializeField] private List<GameObject> _plantParts;
    [SerializeField] private float _resetDelay;
    [Header("Reward")]
    [SerializeField] private GameObject _rewardBlock;
    [SerializeField] private float _dropRadius;
    [SerializeField] private float _rewardJumpPower;
    [SerializeField] private int _rewardNumberJumps;
    [SerializeField] private float _rewardJumpDelay;

    private Collider _collider;
    private int _partNumber = 0;
    private bool _readyForCutting = true;

    public bool ReadyForCutting => _readyForCutting;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public void TakeDamage()
    {
        _plantParts[_partNumber].SetActive(false);
        _partNumber++;
        GameObject rewardBlock = Instantiate(_rewardBlock, transform.position, Quaternion.identity);
        Vector3 endPosition = new Vector3(Random.Range(-_dropRadius, _dropRadius), 0, Random.Range(-_dropRadius, _dropRadius)) + transform.position;
        rewardBlock.transform.DOJump(endPosition, _rewardJumpPower, _rewardNumberJumps, _rewardJumpDelay);
        if(_partNumber == _plantParts.Count)
        {
            _readyForCutting = false;
            _collider.enabled = false;
            _partNumber = 0;
            StartCoroutine(Regeneration());
        }
    }

    IEnumerator Regeneration()
    {
        for(int i = _plantParts.Count - 1; i > -1; i--)
        {
            yield return new WaitForSeconds(_resetDelay / 3);
            _plantParts[i].SetActive(true);
        }
        _readyForCutting = true;
        _collider.enabled = true;
    }
}
