using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections.Generic;


public class GridManager : MonoBehaviour
{
    [SerializeField] private int rows = 0;
    [SerializeField] private int cols = 0;
    
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject giftPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject pathPrefab;

    [SerializeField] private GameObject cellPrefabLight;
    [SerializeField] private GameObject cellPrefabDark;

    [SerializeField] private GameObject secretGrid;
     
    [SerializeField] private UIManager uiManager;

    [SerializeField] private int lives = 10;
    [SerializeField] private int coins = 0;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text coinsText;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private Button[] _buttons;

    [SerializeField] private GameObject _redCanvas;
    [SerializeField] private GameObject _greenCanvas;
    
    private GameObject[,] _grid;
    
    private Vector2Int _playerPosition;

    private const string SOUND_PREF = "Sound";
    private const string MUSIC_PREF = "Music";
    private const string VIBRATION_PREF = "Vibration";

    public string[] _objects = new string[48];

    public void RestartGame()
    {
        PlayerPrefs.SetInt("Lives", 10);
        PlayerPrefs.SetInt("Coin", 0);
        PlayerPrefs.SetInt("Bombs", 0);
        SceneManager.LoadScene("GameScene");    
    }
    
    private void Start()
    {
        _redCanvas.SetActive(false);
        _greenCanvas.SetActive(false);
    
        EnableAllButtons(true);

        // Получаем количество бомб из PlayerPrefs
        int bombCount = PlayerPrefs.GetInt("Bombs", 0);

        // Генерируем массив _objects с бомбами и ячейками Safe
        GenerateObjectArray(bombCount);

        //Debug.Log("Total objects count = " + _objects.Length);

        audioSource.volume = PlayerPrefs.GetInt(SOUND_PREF, 1);

        if (PlayerPrefs.GetInt("Lives") == 0)
        {
            PlayerPrefs.SetInt("Lives", lives);
            PlayerPrefs.SetInt("Coin", coins);
            PlayerPrefs.SetInt("Bombs", 0);
        }

        livesText.text = PlayerPrefs.GetInt("Lives").ToString();
        coinsText.text = PlayerPrefs.GetInt("Coin").ToString();

        _grid = new GameObject[rows, cols];

        LoadPlayerPosition();

        ShuffleGrid();
        GenerateGrid();

        SwapPlayerAndRandom();

        GenerateSecretLayer();
        //Debug.Log(_playerPosition);
    }

    private void GenerateObjectArray(int bombCount)
    {
        // Инициализация массива _objects длиной 48
        _objects = new string[48];

        // Добавляем статичные элементы в начале массива
        _objects[0] = "Coin";
        _objects[1] = "Gift";
        _objects[2] = "Player";

        // Добавляем нужное количество бомб
        for (int i = 3; i < 3 + bombCount; i++)
        {
            _objects[i] = "Bomb";
        }

        // Оставшиеся ячейки заполняем "Safe"
        for (int i = 3 + bombCount; i < _objects.Length; i++)
        {
            _objects[i] = "Safe";
        }
    }

    private void SwapPlayerAndRandom()
    {
        int savedX = PlayerPrefs.GetInt("PlayerPosX");
        int savedY = PlayerPrefs.GetInt("PlayerPosY");

        // Индекс сохраненной позиции в массиве _objects
        int savedIndex = savedX * cols + savedY;

        // Индекс текущей позиции игрока в массиве _objects
        int playerIndex = _playerPosition.x * cols + _playerPosition.y;

        // Меняем местами объекты в массиве _objects
        string temp = _objects[savedIndex];
        _objects[savedIndex] = _objects[playerIndex];
        _objects[playerIndex] = temp;

        //Debug.Log($"Swapped player with object at saved position ({savedX}, {savedY})");

        // Обновляем сетку, чтобы изменения отразились в игровом мире
        ClearGrid();
        GenerateGrid();
    }

    private void SavePlayerPosition()
    {
        PlayerPrefs.SetInt("PlayerPosX", _playerPosition.x);
        PlayerPrefs.SetInt("PlayerPosY", _playerPosition.y);
        PlayerPrefs.Save();
    }
    
    private void LoadPlayerPosition()
    {
        if (PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY"))
        {
            int savedX = PlayerPrefs.GetInt("PlayerPosX");
            int savedY = PlayerPrefs.GetInt("PlayerPosY");
            _playerPosition = new Vector2Int(savedX, savedY);
            //Debug.Log($"Player position loaded: {_playerPosition}");
        }
        else
        {
            //Debug.Log("No saved player position found. Using default position.");
        }
    }
    
    private void GenerateSecretLayer()
    {
        var spacing = 0.7f;  // Расстояние между ячейками, аналогичное основной сетке
        
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                var pos = new Vector3(i * spacing, j * spacing, 0);
                GameObject cell;

                // Чередуем префабы, используя "шахматный" порядок
                if ((i + j) % 2 == 0)
                {
                    cell = Instantiate(cellPrefabLight, pos, Quaternion.identity);
                }
                else
                {
                    cell = Instantiate(cellPrefabDark, pos, Quaternion.identity);
                }
                
                // Помещаем ячейку внутрь объекта secretGrid
                cell.transform.parent = secretGrid.transform;
            }
        }
    }

    private void ShuffleGrid()
    {
        for (var i = _objects.Length - 1; i > 0; i--)
        {
            var randomIndex = Random.Range(0, i + 1);
            // Меняем местами элементы
            var temp = _objects[i];
            _objects[i] = _objects[randomIndex];
            _objects[randomIndex] = temp;
        }
    }

    public void GenerateGrid()
    {
        var idx = 0;
        var spacing = 0.7f;
        
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                var pos = new Vector3(i * spacing, j * spacing, 0);
                var cell = new GameObject();
                
                switch (_objects[idx])
                {
                    case "Coin":
                        cell = coinPrefab;
                        break;
                    case "Gift":
                        cell = giftPrefab;
                        break;
                    case "Player":
                        cell = playerPrefab;
                        _playerPosition = new Vector2Int(i, j); // Запоминаем стартовую позицию игрока
                        break;
                    case "Bomb":
                        cell = bombPrefab;
                        break;
                    case "Safe":
                        cell = cellPrefab;
                        break;
                    case "Path":
                        cell = pathPrefab;
                        break;
                }
                
                _grid[i, j] = Instantiate(cell, pos, Quaternion.identity);
                
                idx++;
            }
        }
    }

    public void MoveUp()
    {
        TryMove(Vector2Int.up);
    }

    public void MoveDown()
    {
        TryMove(Vector2Int.down);
    }

    public void MoveLeft()
    {
        TryMove(Vector2Int.left);
    }

    public void MoveRight()
    {
        TryMove(Vector2Int.right);
    }

    private void TryMove(Vector2Int direction)
    {
        Vector2Int newPosition = _playerPosition + direction;

        if (newPosition.x >= 0 && newPosition.x < rows && newPosition.y >= 0 && newPosition.y < cols)
        {
            audioSource.Play();
            string objectAtNewPosition = _objects[newPosition.x * cols + newPosition.y];

            switch (objectAtNewPosition)
            {
                case "Coin":
                    if (PlayerPrefs.GetInt(VIBRATION_PREF) == 1)
                    {
                        Handheld.Vibrate();
                    }
                    StartCoroutine(HandleCoin());
                    break;
                case "Gift":
                    //Debug.Log("Player found a gift!");
                    if (PlayerPrefs.GetInt(VIBRATION_PREF) == 1)
                    {
                        Handheld.Vibrate();
                    }
                    EnableAllButtons(false);
                    uiManager.EnableRoulettePanel();
                    break;
                case "Bomb":
                    if (PlayerPrefs.GetInt(VIBRATION_PREF) == 1)
                    {
                        Handheld.Vibrate();
                    }
                    DestroySecretLayer();
                    StartCoroutine(HandleBomb());
                    break;
                case "Safe":
                    MovePlayerTo(newPosition);
                    break;
            }
        }
    }
    
    private IEnumerator HandleCoin()
    {
        ActiveGreenCanvas();
        EnableAllButtons(false);
        SavePlayerPosition();

        Coin coinObject = FindObjectOfType<Coin>();

        if (coinObject != null)
        {
            Animator coinAnimator = coinObject.GetComponent<Animator>();
            if (coinAnimator != null)
            {
                Debug.Log("Activate");
                coinAnimator.SetTrigger("Collected"); // Активируем триггер Collected
            }
        }

        yield return new WaitForSeconds(1f);

        if (lives != 0)
        {
            var coin = PlayerPrefs.GetInt("Coin");
            PlayerPrefs.SetInt("Coin", coin + 100);

            // Заменяем Safe на Bomb, если бомб меньше 7
            AddBombIfPossible();

            SceneManager.LoadScene("GameScene");  // Перезагрузка сцены после получения монеты
        }
    }
    
    private void AddBombIfPossible()
    {
        // Получаем текущее количество бомб из PlayerPrefs
        int currentBombs = PlayerPrefs.GetInt("Bombs", 0);

        // Если бомб меньше 7, добавляем еще одну
        if (currentBombs < 7)
        {
            // Ищем индексы всех ячеек Safe
            List<int> safeIndices = new List<int>();
            for (int i = 0; i < _objects.Length; i++)
            {
                if (_objects[i] == "Safe")
                {
                    safeIndices.Add(i);
                }
            }

            // Если есть хотя бы одна ячейка Safe, заменяем ее на бомбу
            if (safeIndices.Count > 0)
            {
                int randomIndex = Random.Range(0, safeIndices.Count);
                _objects[safeIndices[randomIndex]] = "Bomb"; // Заменяем Safe на Bomb
                Debug.Log("Replaced a Safe with a Bomb.");

                // Увеличиваем количество бомб и сохраняем в PlayerPrefs
                currentBombs++;
                PlayerPrefs.SetInt("Bombs", currentBombs);
                PlayerPrefs.Save();
            }
        }
    }
    
    private IEnumerator HandleBomb()
    {
        ActiveRedCanvas();
        EnableAllButtons(false);
        // Задержка в 1 секунду
        yield return new WaitForSeconds(1f);
    
        if (PlayerPrefs.GetInt("Lives") - 1 <= 0)
        {
            EndGame(PlayerPrefs.GetInt("Coin"));
            uiManager.EnableWinPanel(PlayerPrefs.GetInt("Coin"));
            PlayerPrefs.SetInt("Lives", 0);
            PlayerPrefs.SetInt("Coin", 0);
            livesText.text = "0";
        }
        else
        {
            lives = PlayerPrefs.GetInt("Lives");
            PlayerPrefs.SetInt("Lives", lives - 1);
            SceneManager.LoadScene("GameScene");
        }
    }

    public void UpdateCoin(int amount)
    {
        var coin = PlayerPrefs.GetInt("Coin");
        PlayerPrefs.SetInt("Coin", coin + amount);
        coin = PlayerPrefs.GetInt("Coin");
        coinsText.text = coin.ToString();
    }
    
    public void UpdateLifes(int amount)
    {
        var lives = PlayerPrefs.GetInt("Lives");
        PlayerPrefs.SetInt("Lives", lives + amount);
        lives = PlayerPrefs.GetInt("Lives");
        livesText.text = lives.ToString();
    }
    
    private void MovePlayerTo(Vector2Int newPosition)
    {
        // Меняем местами объекты на сетке: ставим игрока на новую позицию, на старую - Path
        _objects[_playerPosition.x * cols + _playerPosition.y] = "Path";
        _objects[newPosition.x * cols + newPosition.y] = "Player";
        
        // Обновляем позицию игрока
        _playerPosition = newPosition;

        // Обновляем объекты в сцене (убираем старые и ставим новые)
        ClearGrid();
        GenerateGrid();
    }

    public void ClearGrid()
    {
        foreach (var cell in _grid)
        {
            Destroy(cell);
        }
    }

    public void DestroySecretLayer()
    {
        foreach (Transform child in secretGrid.transform)
        {
            Destroy(child.gameObject);  // Удаляем все дочерние объекты внутри secretGrid
        }
    }
    
    private void EndGame(int score)
    {
        if (PlayerPrefs.GetInt("Lives") - 1 == 0)
        {
            ScoreManager.SaveScore(score);  // Сохраняем счет
            //Debug.Log("Save record"+ score);
        }
    }

    public void ResetPrefs()
    {
        PlayerPrefs.SetInt("Coin", 0);
        PlayerPrefs.SetInt("Lives", 0);
        PlayerPrefs.SetInt("Bombs", 0);
    }
    
    private void EnableAllButtons(bool isEnabled)
    {
        foreach (var button in _buttons)
        {
            button.interactable = isEnabled; // Включаем или отключаем кнопку
        }
    }

    public void ActiveGreenCanvas()
    {
        _greenCanvas.SetActive(true);
    }
    
    public void ActiveRedCanvas()
    {
        _redCanvas.SetActive(true);
    }
}