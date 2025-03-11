using System.Runtime.InteropServices;
using System.Text; // Для utf-8

namespace MLOOP_L3
{
    internal class Program
    {
        // Кольори
        static string NORMAL = Console.IsOutputRedirected ? "" : "\x1b[39m";
        static string RED = Console.IsOutputRedirected ? "" : "\x1b[91m";
        static string GREEN = Console.IsOutputRedirected ? "" : "\x1b[92m";
        static string UNDERLINE = Console.IsOutputRedirected ? "" : "\x1b[4m";
        static string NOUNDERLINE = Console.IsOutputRedirected ? "" : "\x1b[24m";
        static string BLUE = Console.IsOutputRedirected ? "" : "\x1b[94m";
        static string YELLOW = Console.IsOutputRedirected ? "" : "\x1b[93m";

        // Змінні для зміни розміра екрану
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        const uint SWP_NOZORDER = 0x0004;
        const uint SWP_NOSIZE = 0x0001;
        const int HWND_TOP = 0;

        // Змінні для маніпулюцій із розміром вікна
        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_SIZEBOX = 0x40000;

        static IntPtr handle = GetConsoleWindow();

        static Random rnd = new Random();

        static void PressAnyKeyToContinue()
        {
            Console.WriteLine(" Натисніть на будь-яку клавішу для продовження ");
            Console.ReadKey();
        }

        static void PrintTextFile(string FileName) // Просто вивід текстового файлу
        {
            using (StreamReader readtext = new StreamReader(FileName))
            {
                while (!readtext.EndOfStream)
                {
                    Console.WriteLine(readtext.ReadLine());
                }
            }
        }

        public static void DrawTextImage(int x, int y, string fileName) // Вивід текстового файлу по кооридантам
        {
            if (x >= Console.BufferWidth || y >= Console.BufferHeight)
            {
                return; // Навіщо малювати те, що повністю за межами екрану?
            }

            int currentY = y;
            int lineOffset = 0;

            if (y < 0)
            {
                lineOffset = -y;
                currentY = 0;
            }

            int lineCount = 0;

            using (StreamReader strReader = new StreamReader(fileName))
            {
                string line;

                while (lineCount < lineOffset && (line = strReader.ReadLine()) != null)
                {
                    lineCount++;
                }

                lineCount = 0;

                while ((line = strReader.ReadLine()) != null && currentY < Console.BufferHeight)
                {
                    int charOffset = 0;
                    int displayX = x;

                    if (x < 0)
                    {
                        charOffset = -x;
                        displayX = 0;
                    }

                    if (charOffset < line.Length)
                    {
                        int availableWidth = Console.BufferWidth - displayX;
                        string displayLine = line.Substring(charOffset);

                        if (displayLine.Length > availableWidth)
                        {
                            displayLine = displayLine.Substring(0, availableWidth);
                        }

                        Console.SetCursorPosition(displayX, currentY);
                        Console.Write(displayLine);
                    }

                    currentY++;
                    lineCount++;
                }
            }
        }

        static int GetMinimalElement(int[] inArray, ref int indexOfMinElement)
        {
            int currentIteration = 0;
            int minElement = int.MaxValue;
            foreach(int element in inArray)
            {
                if (minElement > element)
                {
                    minElement = element;
                    indexOfMinElement = currentIteration;
                }
                currentIteration++;
            }
            return minElement;
        }

        static int GetMinimalElement(int[] inArray)
        {
            int minElement = 0;
            foreach (int element in inArray)
            {
                if (minElement > element)
                {
                    minElement = element;
                }
            }
            return minElement;
        }

        static int GetMaximalElement(int[] inArray, ref int indexOfMaxElement)
        {
            int currentIteration = 0;
            int maxElement = int.MinValue;
            foreach (int element in inArray)
            {
                if (maxElement < element)
                {
                    maxElement = element;
                    indexOfMaxElement = currentIteration;
                }
                currentIteration++;
            }
            return maxElement;
        }

        static int GetMaximalElement(int[] inArray)
        {
            int maxElement = 0;
            foreach (int element in inArray)
            {
                if (maxElement < element)
                {
                    maxElement = element;
                }
            }
            return maxElement;
        }

        static int[] ReadFromFile(string FileName = "array.txt")
        {
            int arrayLength = 0;
            int[] someArray;
            using (StreamReader readtext = new StreamReader(FileName))
            {
                if(!int.TryParse(readtext.ReadLine(), out arrayLength)) // Перша стрічка - довжина масиву
                { 
                    arrayLength = 2;
                    someArray = new int[] { 1, 2 };
                    return someArray;
                }

                someArray = new int[arrayLength];
                for (int i = 0; i < arrayLength; i++)
                {
                    int.TryParse(readtext.ReadLine(), out someArray[i]);
                }
            }
            return someArray;
        }

        static int[][] ReadMatrixFromFile(string FileName = "BlueOrRedPill.txt")
        {
            int[][] someMatrix;
            string[] targetLine;

            int x, y;
            int width, height;
            using (StreamReader readtext = new StreamReader(FileName))
            {
                targetLine = readtext.ReadLine().Split(); // Перша стрічка - ширина та висота масиву
                if (!int.TryParse(targetLine[0], out width))
                {
                    width = 2;
                }

                if (!int.TryParse(targetLine[1], out height))
                {
                    height = 2;
                }

                someMatrix = new int[height][];

                for (y = 0; y < height; y++)
                {
                    someMatrix[y] = new int[width];
                    x = 0;

                    targetLine = readtext.ReadLine().Split();
                    foreach (string number in targetLine)
                    {
                        if (!int.TryParse(number, out someMatrix[y][x]))
                        {
                            someMatrix[y][x] = rnd.Next(-100, 255);
                        }
                        x++;
                    }
                }
            }
            return someMatrix;
        }

        static void Task1()
        {
            Console.WriteLine();

            int choice = 0;
            do
            {
                Console.Write($"\n Введіть варіант вводу:\n {UNDERLINE}1){NOUNDERLINE} З файлу;\n " +
                    $"{UNDERLINE}2){NOUNDERLINE} Випадкова генерація;\n " +
                    $"{UNDERLINE}3){NOUNDERLINE} Консольний ввід.\n > ");
                int.TryParse(Console.ReadLine(), out choice);
            } while (choice < 1 || choice > 3);

            int[] numbers = null;
            int numbersLength;

            switch (choice)
            {
                case 1:
                    numbers = ReadFromFile();
                    break;
                case 2:
                    numbersLength = 25;
                    numbers = new int[numbersLength];
                    for (int i = 0; i < numbersLength; i++)
                    {
                        numbers[i] = rnd.Next(1, 101);
                    }
                    break;
                case 3:
                    Console.Write(" Введіть довжину масиву\n > ");
                    if (!int.TryParse(Console.ReadLine(), out numbersLength)) { numbersLength = 10; }

                    numbers = new int[numbersLength];
                    for (int i = 0; i < numbersLength; i++)
                    {
                        Console.Write($" Введіть номер масиву №{GREEN}{i}{NORMAL}\n > ");
                        if (!int.TryParse(Console.ReadLine(), out numbers[i])) { numbers[i] = rnd.Next(1, 101); }
                    }
                    break;

            }

            Console.WriteLine(" Масив до перетворення: ");
            Console.WriteLine(" " + string.Join(", ", numbers));

            int indexOfMinElement = 0;
            int minElement = GetMinimalElement(numbers, ref indexOfMinElement);

            int indexOfMaxElement = 0;
            int maxElement = GetMaximalElement(numbers, ref indexOfMaxElement);

            for (int i = indexOfMaxElement + 1; i < numbers.Length; i++)
            {
                numbers[i] += minElement;
            }

            Console.WriteLine($"\n Мінімальний елемент: {GREEN}{minElement}{NORMAL}\n" +
                $" Індекс мінімального елементу: {GREEN}{indexOfMinElement}{NORMAL}\n" +
                $" Максимальний елемент: {GREEN}{maxElement}{NORMAL}\n" +
                $" Індекс максимального елементу: {GREEN}{indexOfMaxElement}{NORMAL}\n");

            Console.WriteLine(" Масив після перетворення має таку структуру: ");
            Console.WriteLine(" " + string.Join(", ", numbers));

            Console.WriteLine();
            PressAnyKeyToContinue();
        }

        static int GetNumOfPositiveElements(int[] inArray)
        {
            int numOfPositiveElements = 0;
            foreach (int element in inArray)
            {
                if (element > 0)
                {
                    numOfPositiveElements++;
                }
            }
            return numOfPositiveElements;
        }

        static int GetNumOfNegativeElements(int[] inArray)
        {
            int numOfNEgativeElements = 0;
            foreach (int element in inArray)
            {
                if (element < 0)
                {
                    numOfNEgativeElements++;
                }
            }
            return numOfNEgativeElements;
        }

        static void PrintMatrix(int[][] inMatrix)
        {
            int i, j;
            Console.Write(GREEN);
            for (i = 0; i < inMatrix.Length; i++)
            {
                Console.Write(" ");
                for (j = 0; j < inMatrix[0].Length; j++)
                {
                    Console.Write($"\t{inMatrix[i][j]}");
                }
                Console.WriteLine("");
            }
            Console.WriteLine(NORMAL);
        }

        static void Task2()
        {
            Console.WriteLine();

            int choice = 0;
            do
            {
                Console.Write($"\n Введіть варіант вводу:\n {UNDERLINE}1){NOUNDERLINE} З файлу;\n " +
                    $"{UNDERLINE}2){NOUNDERLINE} Випадкова генерація;\n " +
                    $"{UNDERLINE}3){NOUNDERLINE} Консольний ввід.\n > ");
                int.TryParse(Console.ReadLine(), out choice);
            } while (choice < 1 || choice > 3);

            int i, j;

            int m = 1, n = 1; // m - кількість рядків, n - кількість стовпців
            int[][] matrix = null;

            switch (choice)
            {
                case 1:
                    matrix = ReadMatrixFromFile();
                    n = matrix.Length;
                    m = matrix[0].Length;
                    break;
                case 2:
                    m = 5;
                    n = 4;
                    matrix = new int[n][];
                    for (i = 0; i < n; i++)
                    {
                        matrix[i] = new int[m];
                        for (j = 0; j < m; j++)
                        {
                            matrix[i][j] = rnd.Next(-100, 101);
                        }
                    }
                    break;
                case 3:
                    Console.Write(" Введіть кількість стовпців матриці\n > ");
                    if (!int.TryParse(Console.ReadLine(), out m)) { m = 10; }

                    Console.Write(" Введіть кількість рядків матриці\n > ");
                    if (!int.TryParse(Console.ReadLine(), out n)) { n = 10; }

                    matrix = new int[n][];
                    for (i = 0; i < n; i++)
                    {
                        matrix[i] = new int[m];
                        for (j = 0; j < m; j++)
                        {
                            Console.Write($" Введіть елемент [{GREEN}{i}{NORMAL}][{GREEN}{j}{NORMAL}]\n > ");
                            if (!int.TryParse(Console.ReadLine(), out matrix[i][j])) { matrix[i][j] = rnd.Next(-100, 101); }
                        }
                    }
                    break;

            }

            int[] logicalVectorB = new int[n];
            for (i = 0; i < n; i++)
            {
                if (GetNumOfPositiveElements(matrix[i]) > GetNumOfNegativeElements(matrix[i]))
                {
                    logicalVectorB[i] = 1;
                } 
                else
                {
                    logicalVectorB[i] = 0;
                }
            }

            Console.WriteLine($"\n Логічний вектор B:\n {string.Join(", ", logicalVectorB)}\n");
            Console.WriteLine("\n Матриця до перетворення: ");
            PrintMatrix(matrix);


            int[] currentColumn = new int[n];
            for (i = 0; i < m; i++) // i проходиться по рядкам
            {
                for (j = 0; j < n; j++) // j - по стовпцям
                {
                    currentColumn[j] = matrix[j][i];
                }

                Array.Sort(currentColumn);

                for (j = 0; j < n; j++)
                {
                    matrix[j][i] = currentColumn[n - j - 1];
                }
            }

            Console.WriteLine("\n Матриця після перетворення: ");
            PrintMatrix(matrix);

            Console.WriteLine();
            PressAnyKeyToContinue();
        }

        static void ClearInputBuffer() // Вирішує проблему, коли користувач натискає Enter до того, як можна вводити значення
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        static void WriteTextAt(int x, int y, string text) // Виводить текст на конкретних координатах
        {
            string[] lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(lines[i]);
            }
        }

        static void WriteTextAt(int x, int y, string text, ref int currentY) // Не тільки виводить текст на конкретних координатах, а й збільшує лічільник стрічок
        {
            string[] lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                currentY += 1;
                Console.SetCursorPosition(x, y + i);
                Console.Write(lines[i]);
            }
        }

        static void Setup()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(NORMAL);
            Console.OutputEncoding = Encoding.UTF8;
            SetWindowPos(handle, (IntPtr)HWND_TOP, 10, 120, 1900, 800, SWP_NOZORDER);
            Console.CursorVisible = false;
            int style = GetWindowLong(handle, GWL_STYLE);
            style &= ~(WS_MAXIMIZEBOX | WS_SIZEBOX);
            SetWindowLong(handle, GWL_STYLE, style);
        }

        static void PrintTitle(string date, int numOfLaboratory, string title)
        {
            Console.WriteLine($"\n " + date);
            Console.WriteLine($" Лабораторна робота №{numOfLaboratory}");
            Console.WriteLine($" Тема: {title}");
            Console.WriteLine(" Виконав Соломка Борис");
            Console.WriteLine(" №24");
        }

        // Структури даних для представлення графа
        static int vertices;
        static int[][] adjacencyList; // Фактично, матриця суміжностей
        static int[,] weights;
        static int[] adjacencyCount; // кількість суміжних вершин для кожної вершини

        // Ініціалізація графа
        static void InitializeGraph(int v, int maxEdges)
        {
            vertices = v;
            adjacencyList = new int[vertices][];
            adjacencyCount = new int[vertices];
            weights = new int[vertices, vertices];

            // Ініціалізуємо масиви для кожної вершини
            for (int i = 0; i < vertices; i++)
            {
                adjacencyList[i] = new int[maxEdges]; // максимальна кількість ребер для вершини
                adjacencyCount[i] = 0; // поки немає жодних ребер
            }
        }

        // Додавання ребра з вагою
        static void AddEdge(int source, int destination, int weight)
        {
            // Додаємо ребро source -> destination
            adjacencyList[source][adjacencyCount[source]] = destination;
            adjacencyCount[source]++;

            // Додаємо ребро destination -> source (для неорієнтованого графу)
            adjacencyList[destination][adjacencyCount[destination]] = source;
            adjacencyCount[destination]++;

            // Зберігаємо вагу ребра
            weights[source, destination] = weight;
            weights[destination, source] = weight; // для неорієнтованого графу
        }

        // Метод для знаходження всіх гамільтонових шляхів
        static int[][][] FindAllHamiltonianPaths()
        {
            // Максимальна кількість можливих гамільтонових шляхів
            int maxPaths = Factorial(vertices);
            int[][][] allPaths = new int[maxPaths][][];
            int pathCount = 0;

            bool[] visited = new bool[vertices];
            int[] path = new int[vertices];

            // Стартуємо з кожної вершини
            for (int startVertex = 0; startVertex < vertices; startVertex++)
            {
                Array.Fill(visited, false);
                path[0] = startVertex;
                visited[startVertex] = true;

                FindAllHamiltonianPathsUtil(visited, path, 1, allPaths, ref pathCount);
            }

            // Обрізаємо масив до фактичної кількості знайдених шляхів
            int[][][] result = new int[pathCount][][];
            for (int i = 0; i < pathCount; i++)
            {
                result[i] = allPaths[i];
            }

            return result;
        }

        // Допоміжна функція для пошуку всіх гамільтонових шляхів
        static void FindAllHamiltonianPathsUtil(bool[] visited, int[] path, int pos, int[][][] allPaths, ref int pathCount)
        {
            // Якщо шлях містить всі вершини
            if (pos == vertices)
            {
                // Копіюємо поточний шлях в результат
                allPaths[pathCount] = new int[1][];
                allPaths[pathCount][0] = new int[vertices];
                Array.Copy(path, allPaths[pathCount][0], vertices);
                pathCount++;
                return;
            }

            int current = path[pos - 1];
            for (int i = 0; i < adjacencyCount[current]; i++)
            {
                int neighbor = adjacencyList[current][i];
                if (!visited[neighbor])
                {
                    // Додаємо сусіда до шляху
                    visited[neighbor] = true;
                    path[pos] = neighbor;

                    // Рекурсивно шукаємо шляхи
                    FindAllHamiltonianPathsUtil(visited, path, pos + 1, allPaths, ref pathCount);

                    // Backtracking
                    visited[neighbor] = false;
                }
            }
        }

        // Метод для знаходження гамільтонових шляхів між вказаними вершинами
        static int[][][] FindHamiltonianPathsBetween(int start, int end)
        {
            // Максимальна кількість можливих гамільтонових шляхів
            int maxPaths = Factorial(vertices - 1);
            int[][][] paths = new int[maxPaths][][];
            int pathCount = 0;

            bool[] visited = new bool[vertices];
            int[] path = new int[vertices];

            path[0] = start;
            visited[start] = true;

            FindHamiltonianPathsBetweenUtil(visited, path, 1, paths, ref pathCount, end);

            // Обрізаємо масив до фактичної кількості знайдених шляхів
            int[][][] result = new int[pathCount][][];
            for (int i = 0; i < pathCount; i++)
            {
                result[i] = paths[i];
            }

            return result;
        }

        // Допоміжна функція для пошуку гамільтонових шляхів між вказаними вершинами
        static void FindHamiltonianPathsBetweenUtil(bool[] visited, int[] path, int pos, int[][][] paths, ref int pathCount, int end)
        {
            int current = path[pos - 1];

            // Якщо досягли кінцевої вершини і відвідали всі вершини
            if (current == end && pos == vertices)
            {
                // Копіюємо поточний шлях в результат
                paths[pathCount] = new int[1][];
                paths[pathCount][0] = new int[vertices];
                Array.Copy(path, paths[pathCount][0], vertices);
                pathCount++;
                return;
            }

            for (int i = 0; i < adjacencyCount[current]; i++)
            {
                int neighbor = adjacencyList[current][i];
                if (!visited[neighbor])
                {
                    // Додаємо сусіда до шляху
                    visited[neighbor] = true;
                    path[pos] = neighbor;

                    // Рекурсивно шукаємо шляхи
                    FindHamiltonianPathsBetweenUtil(visited, path, pos + 1, paths, ref pathCount, end);

                    visited[neighbor] = false;
                }
            }
        }

        // Метод для знаходження всіх гамільтонових контурів
        static int[][][] FindAllHamiltonianCircuits()
        {
            // Максимальна кількість можливих гамільтонових контурів
            int maxCircuits = Factorial(vertices - 1);
            int[][][] allCircuits = new int[maxCircuits][][];
            int circuitCount = 0;

            bool[] visited = new bool[vertices];
            int[] path = new int[vertices + 1]; // +1 для замикання контуру

            // Починаємо з вершини 0
            path[0] = 0;
            visited[0] = true;

            FindAllHamiltonianCircuitsUtil(visited, path, 1, allCircuits, ref circuitCount, 0);

            // Обрізаємо масив до фактичної кількості знайдених контурів
            int[][][] result = new int[circuitCount][][];
            for (int i = 0; i < circuitCount; i++)
            {
                result[i] = allCircuits[i];
            }

            return result;
        }

        // Допоміжна функція для пошуку всіх гамільтонових контурів
        static void FindAllHamiltonianCircuitsUtil(bool[] visited, int[] path, int pos, int[][][] allCircuits, ref int circuitCount, int startVertex)
        {
            // Якщо шлях містить всі вершини
            if (pos == vertices)
            {
                int lastVertex = path[pos - 1];
                bool hasEdgeToStart = false;

                // Перевіряємо, чи існує ребро від останньої вершини до початкової
                for (int i = 0; i < adjacencyCount[lastVertex]; i++)
                {
                    if (adjacencyList[lastVertex][i] == startVertex)
                    {
                        hasEdgeToStart = true;
                        break;
                    }
                }

                if (hasEdgeToStart)
                {
                    // Додаємо початкову вершину, щоб замкнути контур
                    path[pos] = startVertex;

                    // Копіюємо поточний контур в результат
                    allCircuits[circuitCount] = new int[1][];
                    allCircuits[circuitCount][0] = new int[vertices + 1];
                    Array.Copy(path, allCircuits[circuitCount][0], vertices + 1);
                    circuitCount++;
                }
                return;
            }

            int current = path[pos - 1];
            for (int i = 0; i < adjacencyCount[current]; i++)
            {
                int neighbor = adjacencyList[current][i];
                if (!visited[neighbor])
                {
                    // Додаємо сусіда до шляху
                    visited[neighbor] = true;
                    path[pos] = neighbor;

                    // Рекурсивно шукаємо контури
                    FindAllHamiltonianCircuitsUtil(visited, path, pos + 1, allCircuits, ref circuitCount, startVertex);

                    // Backtracking
                    visited[neighbor] = false;
                }
            }
        }

        // Метод для знаходження мінімального гамільтонового контуру
        static int[] FindMinHamiltonianCircuit()
        {
            int[][][] allCircuits = FindAllHamiltonianCircuits();
            if (allCircuits.Length == 0)
                return new int[0];

            int minWeight = int.MaxValue;
            int[] minCircuit = null;

            for (int i = 0; i < allCircuits.Length; i++)
            {
                int weight = CalculateCircuitWeight(allCircuits[i][0]);
                if (weight < minWeight)
                {
                    minWeight = weight;
                    minCircuit = new int[allCircuits[i][0].Length];
                    Array.Copy(allCircuits[i][0], minCircuit, allCircuits[i][0].Length);
                }
            }

            Console.WriteLine($"Мінімальна вага контуру: {minWeight}");
            return minCircuit;
        }

        // Метод для знаходження максимального гамільтонового контуру
        static int[] FindMaxHamiltonianCircuit()
        {
            int[][][] allCircuits = FindAllHamiltonianCircuits();
            if (allCircuits.Length == 0)
                return new int[0];

            int maxWeight = int.MinValue;
            int[] maxCircuit = null;

            for (int i = 0; i < allCircuits.Length; i++)
            {
                int weight = CalculateCircuitWeight(allCircuits[i][0]);
                if (weight > maxWeight)
                {
                    maxWeight = weight;
                    maxCircuit = new int[allCircuits[i][0].Length];
                    Array.Copy(allCircuits[i][0], maxCircuit, allCircuits[i][0].Length);
                }
            }

            Console.WriteLine($"Максимальна вага контуру: {maxWeight}");
            return maxCircuit;
        }

        // Обчислення ваги контуру
        static int CalculateCircuitWeight(int[] circuit)
        {
            int weight = 0;
            for (int i = 0; i < circuit.Length - 1; i++)
            {
                weight += weights[circuit[i], circuit[i + 1]];
            }
            return weight;
        }

        static int Factorial(int n)
        {
            if (n <= 1)
                return 1;
            return n * Factorial(n - 1);
        }

        // Виведення знайдених шляхів
        static void PrintPaths(int[][][] paths, string label)
        {
            Console.WriteLine($"\n{label} ({paths.Length}):");
            for (int i = 0; i < paths.Length; i++)
            {
                Console.Write($"{i + 1}. ");
                Console.WriteLine(string.Join(" -> ", paths[i][0]));
            }
        }

        // Виведення знайденого контуру
        static void PrintCircuit(int[] circuit, string label)
        {
            Console.WriteLine($"\n{label}:");
            if (circuit.Length == 0)
            {
                Console.WriteLine("Гамільтонового контуру не знайдено");
                return;
            }
            Console.WriteLine(string.Join(" -> ", circuit));
        }

        static void ReadGraphFromFile(string fileName = "graph.txt")
        {
            using (StreamReader readText = new StreamReader(fileName))
            {
                string line = readText.ReadLine();
                if (line == null) { return; }

                // Зчитуємо кількість вершин
                vertices = int.Parse(line);

                // Зчитуємо максимальну кількість ребер (можна обчислити або вказати у файлі)
                line = readText.ReadLine();
                if (line == null) { return; }

                int maxEdgesPerVertex = int.Parse(line.Trim());

                // Ініціалізуємо граф
                InitializeGraph(vertices, maxEdgesPerVertex);

                // Зчитуємо ребра та їх ваги
                while ((line = readText.ReadLine()) != null)
                {
                    string[] parts = line.Split();
                    if (parts.Length >= 3) // Формат: вершина1 вершина2 вага
                    {
                        int source = int.Parse(parts[0]);
                        int destination = int.Parse(parts[1]);
                        int weight = int.Parse(parts[2]);

                        AddEdge(source, destination, weight);
                    }
                }
            }
        }

        static void Task3()
        {
            // Приклад використання
            int vertices = 5;
            int maxEdgesPerVertex = 4; // максимальна кількість ребер для вершини
            InitializeGraph(vertices, maxEdgesPerVertex);

            ReadGraphFromFile();

            // Знаходимо всі гамільтонові шляхи
            int[][][] allPaths = FindAllHamiltonianPaths();
            PrintPaths(allPaths, "Всі гамільтонові шляхи");

            // Знаходимо гамільтонові шляхи між вершинами 0 і 4
            int[][][] pathsBetween = FindHamiltonianPathsBetween(0, 4);
            PrintPaths(pathsBetween, "Гамільтонові шляхи від 0 до 4");

            // Знаходимо мінімальний і максимальний гамільтонові контури
            int[] minCircuit = FindMinHamiltonianCircuit();
            PrintCircuit(minCircuit, "Мінімальний гамільтоновий контур");

            int[] maxCircuit = FindMaxHamiltonianCircuit();
            PrintCircuit(maxCircuit, "Максимальний гамільтоновий контур");
            Console.WriteLine();
            PressAnyKeyToContinue();
        }

        static void Main(string[] args)
        {
            Setup();
            PrintTitle($"06.03.2025", 3, "Одношарові та багатошарові масиви");

            bool isRunning = true;
            while (isRunning)
            {
                Console.Write($"\n Введіть відповідний номер:\n {UNDERLINE}0){NOUNDERLINE} Вихід;\n " +
                    $"{UNDERLINE}1){NOUNDERLINE} Завдання №3.1.24;\n " +
                    $"{UNDERLINE}2){NOUNDERLINE} Завдання №3.1.28;\n " +
                    $"{GREEN}{UNDERLINE}3){NOUNDERLINE} ДОДАТКОВЕ ЗАВДАННЯ.{NORMAL}\n > ");
                int userChoice;
                if (!int.TryParse(Console.ReadLine(), out userChoice)) { break; }
                Console.Clear();

                switch (userChoice)
                {
                    case 0:
                        isRunning = false;
                        break;
                    case 1:
                        Task1();
                        break;
                    case 2:
                        Task2();
                        break;
                    case 3:
                        Task3();
                        break;
                    default:
                        Console.WriteLine(" Введено некоректне число.");
                        break;
                }
                Console.Clear();
            }
        }
    }
}
