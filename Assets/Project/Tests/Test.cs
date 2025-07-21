using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnEnable()
    {
        int[,] maze = Maze.GetMaze(25, 25);
        GUIUtility.systemCopyBuffer = maze.Show();
        Debug.Log(maze.Show());
    }

}
