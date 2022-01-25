using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab22
{
    class Program
    {
        /*Сформировать массив случайных целых чисел (размер  задается пользователем). 
         * Вычислить сумму чисел массива и максимальное число в массиве.  
         * Реализовать  решение  задачи  с  использованием  механизма  задач продолжения.
         * */
        static void Main(string[] args)
        {
            Console.Write("Введите количество элементов массива: ");
            int n = Convert.ToInt32(Console.ReadLine());

            Func<object, int[]> func1 = new Func<object, int[]>(GetArrey);
            Task<int[]> task1 = new Task<int[]>(func1, n);

            //задача-продолжение - сортировка по возрастанию
            Func<Task<int[]>, int[]> func2 = new Func<Task<int[]>, int[]>(SortArray);
            Task<int[]> task2 = task1.ContinueWith<int[]>(func2);

            Console.Write($"\nМассив из случайных целых чисел: ");
            //вывод задача-продолжение
            Action<Task<int[]>> action1 = new Action<Task<int[]>>(PrintArray);
            Task task3 = task2.ContinueWith(action1);


            task1.Start();
            Console.ReadKey();
        }
        //метод формирует массив
        static int[] GetArrey(object a)
        {
            int n = (int)a;
            int[] array = new int[n];
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                array[i] = random.Next(0, 100); //чтобы не большие числа - диапазон от 0 до 100
            }
            return array;
        }
        //метод сортирует массив: принимает на вход готовый массив, задачу Task
        static int[] SortArray(Task<int[]> task)
        {
            int[] array = task.Result;
            for (int i = 0; i < array.Count() - 1; i++)
            {
                for (int j = i + 1; j < array.Count(); j++)
                {
                    if (array[i] > array[j])
                    {
                        int t = array[i];
                        array[i] = array[j];
                        array[j] = t;
                    }
                }
            }
            return array;
        }
        //метод для вывода на экран
        static void PrintArray(Task<int[]> task)
        {
            int[] array = task.Result;
            int max = array[0];
            for (int i = 0; i < array.Count(); i++)
            {
                Console.Write($"{array[i]} ");
            }
            Console.WriteLine();
            //максимальное число в массиве
            foreach (int x in array)
            {
                if (x > max)
                    max = x;
            }
            Console.WriteLine($"\nМаксимальное число в массиве равно: {max}");
            //сумма чисел в массиве
            int sum = 0;
            foreach (int a in array)
            {
                sum += a;
            }
            Console.WriteLine($"\nСумма чисел в массиве равна: {sum}");
        }
    }
}
