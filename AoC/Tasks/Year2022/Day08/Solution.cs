namespace AoC.Tasks.Year2022.Day08;

public class Solution : ISolver
{
    private readonly List<string> _list;

    public Solution(string input) => _list = InputHelper.ToStringList(input);

    public object PartOne()
    {
        var grid = CreateGrid(_list.Count, _list.First().Length).ToArray();
        var visibleTrees = CreateGrid(_list.Count, _list.First().Length, 0).ToArray();

        PopulateGridWithTrees(grid);
        CheckVisibleTrees(grid, visibleTrees);
        int count = CountVisibleTrees(visibleTrees);
        return count;
    }

    public object PartTwo()
    {
        var grid = CreateGrid(_list.Count, _list.First().Length).ToArray();
        var visibleTrees = CreateGrid(_list.Count, _list.First().Length, 0).ToArray();

        PopulateGridWithTrees(grid);
        CheckViewDistances(grid, visibleTrees);
        int result = GetMaxViewDistance(visibleTrees);

        return result;
        // 431460 too low
        // 2298780 Too high
        // 3130512 Too high
    }

    private int GetMaxViewDistance(int[][] visibleTrees)
    {
        int viewDistance = int.MinValue;
        for (int row = 1; row < visibleTrees.Length - 1; row++)
        {
            for (int col = 1; col < visibleTrees[row].Length - 1; col++)
            {
                viewDistance = Math.Max(visibleTrees[row][col], viewDistance);
            }
        }

        return viewDistance;
    }

    private int CountVisibleTrees(int[][] visibleTrees)
    {
        int count = 0;
        for (int row = 1; row < visibleTrees.Length - 1; row++)
        {
            for (int col = 1; col < visibleTrees[row].Length - 1; col++)
            {
                count += visibleTrees[row][col];
            }
        }

        return count;
    }

    private void CheckVisibleTrees(int[][] grid, int[][] visibleTrees)
    {
        for (int row = 1; row < grid.Length - 1; row++)
        {
            for (int col = 1; col < grid[row].Length - 1; col++)
            {
                var res = IsVisibleTree(grid, row, col);
                visibleTrees[row][col] = res ? 1 : 0;
            }
        }
    }

    private void CheckViewDistances(int[][] grid, int[][] visibleTrees)
    {
        for (int row = 1; row < grid.Length - 1; row++)
        {
            for (int col = 1; col < grid[row].Length - 1; col++)
            {
                var res = CalculateViewSight(grid, row, col);
                visibleTrees[row][col] = res;
            }
        }
    }

    private int CalculateViewSight(int[][] grid, int row, int col)
    {
        var element = grid[row][col];

        var left = grid[row].ToList().Take(col).Skip(1).ToList();
        var right = grid[row].ToList().Skip(col + 1).SkipLast(1).ToList();
        var top = grid.ToList().Select(x => x[col]).Take(row).Skip(1).ToList();
        var below = grid.ToList().Select(x => x[col]).Skip(row + 1).SkipLast(1).ToList();
        right.Reverse();
        below.Reverse();
        var leftStack = new Stack<int>(left);
        var rightStack = new Stack<int>(right);
        var topStack = new Stack<int>(top);
        var belowStack = new Stack<int>(below);

        int topViewSight = 0;
        while (topStack.Any())
        {
            var val = topStack.Pop();
            topViewSight++;

            if (val >= element)
            {
                break;
            }
        }

        int belowViewSight = 0;
        while (belowStack.Any())
        {
            var val = belowStack.Pop();
            belowViewSight++;

            if (val >= element)
            {
                break;
            }
        }

        int rightViewSight = 0;
        while (rightStack.Any())
        {
            var val = rightStack.Pop();
            rightViewSight++;

            if (val >= element)
            {
                break;
            }
        }

        int leftViewSight = 0;
        while (leftStack.Any())
        {
            var val = leftStack.Pop();
            leftViewSight++;

            if (val >= element)
            {
                break;
            }
        }

        var result = Math.Max(1, topViewSight) * Math.Max(1, belowViewSight) * Math.Max(1, leftViewSight) * Math.Max(1, rightViewSight);
        return result;
    }

    private bool IsVisibleTree(int[][] grid, int row, int col)
    {
        var element = grid[row][col];

        var left = grid[row].ToList().Take(col);
        var right = grid[row].ToList().Skip(col + 1);
        var top = grid.ToList().Select(x => x[col]).Take(row);
        var below = grid.ToList().Select(x => x[col]).Skip(row + 1);

        if (element > left.Max())
        {
            return true;
        }

        if (element > right.Max())
        {
            return true;
        }

        if (element > top.Max())
        {
            return true;
        }

        if (element > below.Max())
        {
            return true;
        }

        return false;
    }

    private void PopulateGridWithTrees(int[][] grid)
    {
        for (int row = 1; row < grid.Count() - 1; row++)
        {
            var cols = _list.ElementAt(row - 1).ToCharArray().Select(x => x.ToString()).Select(int.Parse).ToList();
            int colIndex = 1;
            cols.ForEach(x => grid[row][colIndex++] = x);
        }
    }

    private static IEnumerable<int[]> CreateGrid(int rows, int col, int val = -1)
    {
        var list = new List<int[]>();
        for (int i = 0; i < rows + 2; i++)
        {
            var arr = Enumerable.Range(0, col + 2).ToArray();
            Array.Fill(arr, val, 0, arr.Length);
            list.Add(arr);
        }

        return list;
    }
}
