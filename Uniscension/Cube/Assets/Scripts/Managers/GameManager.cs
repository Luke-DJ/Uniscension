using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public ArrayList players;

    public static GameManager _instance;

    private ArrayList cubes;

    [HideInInspector] public float currentScore;

    private void Awake()
    {
        if (_instance == null)
        {
            players = new ArrayList();
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void SetupPlayer(GameObject playerObject, int index)
    {
        Player p = playerObject.GetComponent<Player>();
        p.playerIndex = index;

        players.Add(playerObject);
    }

    private void Update()
    {
        GL.Clear(false, true, Color.black);
    }
}