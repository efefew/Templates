using UnityEngine;

public class Test : MonoBehaviour
{
    [Min(1)] public int Size;

    private void OnEnable()
    {
        int[,] maze = Maze.GetMaze(Size, Size);
        GUIUtility.systemCopyBuffer = maze.Show();
        Debug.Log(maze.Show());
    }
}