using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Coin settings")]
    [SerializeField] private TextMeshProUGUI _coinCount;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private int _coinReward;
    [SerializeField] private float _coinAnimDuration;
    [SerializeField] private float _speedOfCoinAdding;
    [Header("Blocks settings")]
    [SerializeField] private TextMeshProUGUI _currentBlocksCount;
    [SerializeField] private TextMeshProUGUI _maxBlocksCount;

    private float _currentCoinCount = 0;
    private float _nextCoinCount = 0;
    private BagForRewardBlock _bagForRewardBlock;
    private Stock _stock;

    void Start()
    {
        _bagForRewardBlock = FindObjectOfType<BagForRewardBlock>();
        if(_bagForRewardBlock == null)
        {
            Debug.LogError("Can't find BagForRewardBlock.cs on scene. (" + name + ")");
            Destroy(this);
        }
        _maxBlocksCount.text = _bagForRewardBlock.BagCapacity.ToString();
        _currentBlocksCount.text = _bagForRewardBlock.CurrentBlocksCount.ToString();
        _coinCount.text = "0";

        _stock = FindObjectOfType<Stock>();
        Stock.HandOverBlock.AddListener(SaleBlock);
        BagForRewardBlock.UpdateBlockCount.AddListener(UpdateCountBlock);

    }

    private void Update()
    {
        if (_currentCoinCount < _nextCoinCount)
        {
            _coinCount.transform.DOShakePosition(1);
            _currentCoinCount += _speedOfCoinAdding * Time.deltaTime;
            _coinCount.text = Mathf.RoundToInt(_currentCoinCount).ToString();
        }
    }

    private void SaleBlock()
    {
        if(_stock != null)
        {
            GameObject newCoin = Instantiate(_coinPrefab, transform);
            newCoin.transform.position = Camera.main.WorldToScreenPoint(_stock.StockPlace.position);
            newCoin.transform.DOMove(_coinCount.transform.position, _coinAnimDuration).OnComplete(() => AddCoins(newCoin));
        }
    }

    private void AddCoins(GameObject coin)
    {
        Destroy(coin);

        _nextCoinCount += _coinReward;
    }

    private void UpdateCountBlock()
    {
        _currentBlocksCount.text = _bagForRewardBlock.CurrentBlocksCount.ToString();
    }
}
