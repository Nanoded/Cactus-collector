using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.Events;

public class BagForRewardBlock : MonoBehaviour
{
    [Header("Bag setting")]
    [SerializeField] private Transform _bag;
    [SerializeField] private int _bagCapacity;
    [Header("Block settings")]
    [SerializeField] private float _collectDuration;
    [SerializeField] private float _handOverDuration;
    [SerializeField] private float _handOverSpeed;

    private List<GameObject> _blocksInBag = new List<GameObject>();
    private Stock _stock;
    private int _currentNumbBlocks = 0;
    private bool _isHandOver = false;
    public static UnityEvent UpdateBlockCount = new UnityEvent();
    public int BagCapacity => _bagCapacity;
    public int CurrentBlocksCount => _currentNumbBlocks;

    void Start()
    {
        DOTween.Init();
        _stock = FindObjectOfType<Stock>();
        if(_stock == null)
        {
            Debug.LogError("Can't find Stock.cs on scene. (" + name + ")");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Stock stock))
        {
            if (_isHandOver == false)
            {
                StartCoroutine(HandOver(stock));
            }
        }
    }

    IEnumerator HandOver(Stock stock)
    {
        _isHandOver = true;
        DOTween.Clear();

        for(int i = _blocksInBag.Count - 1; i > -1; i--)
        {
            yield return new WaitForSeconds(_handOverSpeed);
            _blocksInBag[i].transform.SetParent(stock.StockPlace);
            _blocksInBag[i].transform.DOLocalMove(Vector3.zero, _handOverDuration).OnComplete(() => _stock.SaleBlock());
            Destroy(_blocksInBag[i], 10f);
            _currentNumbBlocks--;
            UpdateBlockCount.Invoke();
        }
        _isHandOver = false;
    }

    public void AddBlockInBag(Transform block)
    {
        if (_currentNumbBlocks == 0)
        {
            _blocksInBag.Clear();
        }
        block.DOPause();
        block.DOLocalMove(Vector3.zero + transform.up * block.lossyScale.y * _currentNumbBlocks, _collectDuration);
        block.DOLocalRotate(Vector3.zero, _collectDuration);
        block.SetParent(_bag.transform);
        _blocksInBag.Add(block.gameObject);
        _currentNumbBlocks++;
        UpdateBlockCount.Invoke();
    }
}
