using System;
using System.Collections;
using System.IO;

namespace LabWorkTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.WriteLine("=================================");
                Console.WriteLine("Select a task to execute:");
                Console.WriteLine("1. Task 1 (Prefix expression using Stack)");
                Console.WriteLine("2. Task 2 (Words from file using Queue)");
                Console.WriteLine("3. Task 3 (Tasks 1 and 2 using ArrayList)");
                Console.WriteLine("4. Task 4 (CD Catalog using Hashtable)");
                Console.WriteLine("0. Exit");
                Console.WriteLine("=================================");
                Console.Write("Your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": new LabTask1().Run(); break;
                    case "2": new LabTask2().Run(); break;
                    case "3": new LabTask3().Run(); break;
                    case "4": new LabTask4().Run(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid choice. Please try again."); break;
                }
            }
        }
    }

    public class LabTask1
    {
        public void Run()
        {
            Console.WriteLine("--- Task 1: Prefix Expression Evaluation (Stack) ---");
            Console.Write("Enter a prefix expression (e.g., + * 2 3 4): ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                input = "+ * 2 3 4";
                Console.WriteLine($"Using default expression: {input}");
            }

            string[] tokens = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Stack stack = new Stack();

            for (int i = tokens.Length - 1; i >= 0; i--)
            {
                string token = tokens[i];
                if (IsOperator(token))
                {
                    double a = Convert.ToDouble(stack.Pop());
                    double b = Convert.ToDouble(stack.Pop());
                    stack.Push(ApplyOperator(token, a, b));
                }
                else
                {
                    stack.Push(Convert.ToDouble(token));
                }
            }

            Console.WriteLine($"Result: {stack.Pop()}\n");
        }

        private bool IsOperator(string token) => token == "+" || token == "-" || token == "*" || token == "/";

        private double ApplyOperator(string op, double a, double b) => op switch
        {
            "+" => a + b,
            "-" => a - b,
            "*" => a * b,
            "/" => a / b,
            _ => 0
        };
    }

    public class LabTask2
    {
        public void Run()
        {
            Console.WriteLine("--- Task 2: File Processing (Queue) ---");
            string filePath = "text_data.txt";

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "Apple banana Cherry dog Elephant fox.");
                Console.WriteLine($"Created test file: {filePath}");
            }

            Queue upperQueue = new Queue();
            Queue lowerQueue = new Queue();

            string text = File.ReadAllText(filePath);
            string[] words = text.Split(new[] { ' ', '\n', '\r', '\t', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                if (char.IsUpper(word[0]))
                    upperQueue.Enqueue(word);
                else if (char.IsLower(word[0]))
                    lowerQueue.Enqueue(word);
            }

            Console.WriteLine("Words starting with an uppercase letter:");
            while (upperQueue.Count > 0) Console.Write(upperQueue.Dequeue() + " ");

            Console.WriteLine("\n\nWords starting with a lowercase letter:");
            while (lowerQueue.Count > 0) Console.Write(lowerQueue.Dequeue() + " ");
            Console.WriteLine("\n");
        }
    }

    public class LabTask3
    {
        public void Run()
        {
            Console.WriteLine("--- Task 3: Solving Tasks 1 & 2 using ArrayList ---");

            Console.WriteLine("\n[Part 1: Prefix Expression via ArrayList]");
            string input = "+ * 2 3 4";
            Console.WriteLine($"Expression: {input}");
            string[] tokens = input.Split(' ');
            ArrayList listStack = new ArrayList();

            for (int i = tokens.Length - 1; i >= 0; i--)
            {
                string token = tokens[i];
                if (token == "+" || token == "-" || token == "*" || token == "/")
                {
                    double a = Convert.ToDouble(listStack[listStack.Count - 1]); listStack.RemoveAt(listStack.Count - 1);
                    double b = Convert.ToDouble(listStack[listStack.Count - 1]); listStack.RemoveAt(listStack.Count - 1);

                    double res = token == "+" ? a + b : token == "-" ? a - b : token == "*" ? a * b : a / b;
                    listStack.Add(res);
                }
                else
                {
                    listStack.Add(Convert.ToDouble(token));
                }
            }
            Console.WriteLine($"Result: {listStack[0]}");

            Console.WriteLine("\n[Part 2: Word Sorting via ArrayList]");
            ArrayList upperList = new ArrayList();
            ArrayList lowerList = new ArrayList();
            string[] words = { "Apple", "banana", "Cherry", "dog" };

            foreach (var w in words)
            {
                if (char.IsUpper(w[0])) upperList.Add(w);
                else if (char.IsLower(w[0])) lowerList.Add(w);
            }

            Console.WriteLine("Uppercase: " + string.Join(" ", upperList.ToArray()));
            Console.WriteLine("Lowercase: " + string.Join(" ", lowerList.ToArray()) + "\n");
        }
    }

    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public override string ToString() => $"'{Title}' - {Artist}";
    }

    public class LabTask4
    {
        private Hashtable catalog = new Hashtable();

        public void Run()
        {
            Console.WriteLine("--- Task 4: CD Catalog (Hashtable) ---");

            AddCd("Rock Hits");
            AddSong("Rock Hits", new Song { Title = "Bohemian Rhapsody", Artist = "Queen" });
            AddSong("Rock Hits", new Song { Title = "Stairway to Heaven", Artist = "Led Zeppelin" });

            AddCd("Pop Classics");
            AddSong("Pop Classics", new Song { Title = "Billie Jean", Artist = "Michael Jackson" });
            AddSong("Pop Classics", new Song { Title = "Beat It", Artist = "Michael Jackson" });

            ViewCatalog();
            SearchByArtist("Michael Jackson");

            Console.WriteLine("\nRemoving song 'Bohemian Rhapsody'...");
            RemoveSong("Rock Hits", "Bohemian Rhapsody");
            ViewCd("Rock Hits");
            Console.WriteLine();
        }

        public void AddCd(string cdName)
        {
            if (!catalog.ContainsKey(cdName)) catalog.Add(cdName, new ArrayList());
        }

        public void RemoveCd(string cdName) => catalog.Remove(cdName);

        public void AddSong(string cdName, Song song)
        {
            if (catalog.ContainsKey(cdName)) ((ArrayList)catalog[cdName]).Add(song);
            else Console.WriteLine($"CD '{cdName}' not found!");
        }

        public void RemoveSong(string cdName, string songTitle)
        {
            if (catalog.ContainsKey(cdName))
            {
                ArrayList songs = (ArrayList)catalog[cdName];
                for (int i = 0; i < songs.Count; i++)
                {
                    if (((Song)songs[i]).Title == songTitle)
                    {
                        songs.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void ViewCatalog()
        {
            Console.WriteLine("\n[Full Catalog Content]");
            foreach (DictionaryEntry entry in catalog)
            {
                ViewCd((string)entry.Key);
            }
        }

        public void ViewCd(string cdName)
        {
            if (catalog.ContainsKey(cdName))
            {
                Console.WriteLine($"\nCD: {cdName}");
                ArrayList songs = (ArrayList)catalog[cdName];
                if (songs.Count == 0) Console.WriteLine("  (empty)");
                foreach (Song song in songs) Console.WriteLine($"  - {song}");
            }
        }

        public void SearchByArtist(string artist)
        {
            Console.WriteLine($"\n[Searching all songs by artist: {artist}]");
            bool found = false;
            foreach (DictionaryEntry entry in catalog)
            {
                foreach (Song song in (ArrayList)entry.Value)
                {
                    if (song.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Found on CD '{entry.Key}': {song}");
                        found = true;
                    }
                }
            }
            if (!found) Console.WriteLine("No songs found by this artist.");
        }
    }
}
