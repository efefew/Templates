#region

using System.Collections.Generic;
using System;
using UnityEngine;
using Random = System.Random;

#endregion

public static class Maze
{
    // Значения ячеек:
    // -1 = стена
    //  0 = проход
    //  1 = путь решения
    public static int[,] GetMaze(int width, int height)
    {
        // Если размеры чётные, уменьшаем на 1, чтобы точно получить «узловую» сетку
        if (width  % 2 == 0) width--;
        if (height % 2 == 0) height--;

        int[,] maze = new int[width, height];
        // Инициализируем все клетки как стены
        for (int x = 0; x < width;  x++) 
            for (int y = 0; y < height; y++) 
                maze[x, y] = -1;

        Random rand = new();

        // Начнём от (1,1)
        Carve(1, 1);

        // Теперь найдём кратчайший путь BFS от (1,1) до (width-2, height-2)
        (int x, int y)[,] from = new (int x, int y)[width, height];
        bool[,] visited = new bool[width, height];
        Queue<(int x, int y)> queue = new Queue<(int x,int y)>();
        (int x, int y) target = (x: width - 2, y: height - 2);

        queue.Enqueue((1,1));
        visited[1,1] = true;
        from[1,1] = (-1, -1);

        int[] dx4 = { 0,  1,  0, -1 };
        int[] dy4 = { 1,  0, -1,  0 };

        while (queue.Count > 0)
        {
            (int cx, int cy) = queue.Dequeue();
            if (cx == target.x && cy == target.y) break;
            for (int d = 0; d < 4; d++)
            {
                int nx = cx + dx4[d], ny = cy + dy4[d];
                if (nx >= 0 && nx < width && ny >= 0 && ny < height
                    && !visited[nx,ny] && maze[nx,ny] == 0)
                {
                    visited[nx,ny] = true;
                    from[nx,ny] = (cx, cy);
                    queue.Enqueue((nx, ny));
                }
            }
        }

        // Восстанавливаем путь, если дошли
        (int x, int y) cur = target;
        int value = 1;
        if (visited[cur.x, cur.y])
        {
            while (cur.x != -1)
            {
                maze[cur.x, cur.y] = value;
                cur = from[cur.x, cur.y];
                value++;
            }
        }

        return maze;

        // Рекурсивная генерация через DFS (рекурсивно)
        void Carve(int x, int y)
        {
            maze[x, y] = 0;  // делаем проход
            // случайный порядок направлений: N, S, E, W
            var dirs = new (int dx, int dy)[] { (0, -2), (0, +2), (+2, 0), (-2, 0) };
            for (int i = dirs.Length - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);
                (dirs[i], dirs[j]) = (dirs[j], dirs[i]);
            }

            foreach ((int dx, int dy) in dirs)
            {
                int nx = x + dx, ny = y + dy;
                if (nx > 0 && nx < width  && ny > 0 && ny < height
                    && maze[nx, ny] == -1)
                {
                    // «Сносим» стену между (x,y) и (nx,ny)
                    maze[x + dx/2, y + dy/2] = 0;
                    Carve(nx, ny);
                }
            }
        }
    }
}