// Тищенко Никита Алексеевич БПИ-247-1 Вариант 10

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileReader
{
	internal class Program
	{
		/// <summary>
		/// Проверяет, хочет ли пользователь закончить работу с программой. 
		/// </summary>
		/// <returns>
		/// true если пользователь хочет завершить работу и false в обратном случае.
		/// </returns>
		internal static bool IsFinish()
		{
			while (true) // цикл работает, пока пользователь не введет корретный ответ
			{
				Console.WriteLine("Хотите продолжить работу с программой? (Да/Нет)");
				string answer = Console.ReadLine()!.ToLower();
				if (answer == "да")
				{
					return false;
				}
				else if (answer == "нет")
				{
					return true;
				}
				else
				{
					Console.WriteLine("Некорректный ввод");
				}
			}
		}
		/// <summary>
		/// Осуществляет ввод коэффициента по имени.
		/// </summary>
		/// <returns>
		/// Коэффициент в формате double.
		/// </returns>
		internal static double InputFactor(string name)
		{
			double x;
			Console.WriteLine($"Введите параметр {name}:");
			while (double.TryParse(Console.ReadLine(), out x) == false || x <= 0 || x >= 1) // цикл не заканчивается, пока пользователь не введет корретный ответ
			{
				Console.WriteLine($"Неверный ввод, введите параметр {name} еще раз:");
			}
			return x;
		}
		/// <summary>
		/// Выделяет массив корректных элементов из строки.
		/// </summary>
		/// <returns>
		/// Массив корректных элементов.
		/// </returns>
		internal static int[] ArrayFromString(string s)
		{
			string[] ar = s.Split();
			string[] correct = Array.FindAll(ar, tmp => int.TryParse(tmp, out int x)); // выделяем корректные данные
			int[] ans = new int[correct.Length];
			for (int i = 0; i < correct.Length; i++)
			{
				ans[i] = int.Parse(correct[i]);
			}
			return ans;
		}
		/// <summary>
		/// Генерирует строку из массива вещественных чисел.
		/// </summary>
		/// <returns>
		/// Сгенерированная строка с вещественными числами точностью три знака.
		/// </returns>
		internal static string StringFromArray(double[] ar)
		{
			string ans = "";
			foreach (double x in ar)
			{
				ans += $"{x:0.000} ";
			}
			return ans;
		}
		/// <summary>
		/// Осуществляет ввод данных из файла и записывает их в качестве массива массивов целочисленных значений. 
		/// </summary>
		/// <returns>
		/// true в случае успешного ввода и false в обратном случае.
		/// </returns>
		internal static bool InputData(out string[] a)
		{
			string path = Path.GetFullPath("../../../../WorkingFiles/input.txt");
			a = [];
			try // пытаемся прочитать данные из входного файла
			{
				a = Array.FindAll(File.ReadAllLines(path), tmp => tmp != "");
				return true;
			}
			catch (FileNotFoundException) // в случае отсутствия файла сообщаем об этом пользователю и завершаем ввод
			{
				Console.WriteLine("Входной файл на диске отсутствует");
				return false;
			}
			catch (DirectoryNotFoundException) // в случае отсутствия папки с файлами сообщаем об этом пользователю и завершаем ввод
			{
				Console.WriteLine("Отсутствует папка с файлами");
				return false;
			}
			catch // если папка и файл на месте, значит файл прочитать невозможно
			{
				Console.WriteLine("Проблемы с чтением данных из файла");
				return false;
			}
		}
		/// <summary>
		/// Выводит заданную строку в файл.
		/// </summary>
		internal static void OutputData(string s)
		{
			string output_path = Path.GetFullPath($"../../../../WorkingFiles/output.txt");
			try // пытаемся записать данные в файл
			{
				File.WriteAllText(output_path, s);
			}
			catch (DirectoryNotFoundException) // в случае отсутствия папки с файлами сообщаем об этом пользователю
			{
				Console.WriteLine("Отсутствует папка с файлами");
			}
			catch // если папка на месте, значит записать данные в файл невозможно
			{
				Console.WriteLine("Проблемы с записью данных в файл");
			}
		}
		/// <summary>
		/// Выделяет набор данных из массива строк на входе.
		/// </summary>
		/// <returns>
		/// true в случае когда удалось получить корректный набор данных и false в обратном случае
		/// </returns>
		internal static bool GetArrays(string[] input, out int[] ar1, out int[] ar2)
		{
			ar1 = [];
			ar2 = [];
			if (input.Length < 2) // если в файле было меньше двух строк, то набора данных нет
			{
				Console.WriteLine("Корректных данных в файле нет");
				return false;
			}
			ar1 = ArrayFromString(input[0]);
			ar2 = ArrayFromString(input[1]);
			if (ar1.Length == ar2.Length && ar1.Length == 0) // если оба массива пустые, то корректных данных нет
			{
				Console.WriteLine("Корректных данных в файле нет");
				return false;
			}
			return true;
		}
		/// <summary>
		/// Обрабатывает введеный набор данных и генерирует вывод для него.
		/// </summary>
		/// <returns>
		/// Вывод для введенного набора данных в формате string.
		/// </returns>
		internal static string Calculate(int[] ar1, int[] ar2, double a, double b, double c, double d)
		{
			string output;
			if (ar1.Length == ar2.Length) // если длины массивов равны, можно сгенерировать вывод
			{
				double[] ans = new double[ar1.Length];
				for (int i = 0; i < ar1.Length; i++)
				{
					ans[i] = ((a * ar1[i]) + b) / ((c * ar2[i]) + d);
				}
				output = StringFromArray(ans);
			}
			else
			{
				output = "0\n";
			}
			return output;
		}
		/// <summary>
		/// Точка входа в программу. Запускает программу, пока пользователь не решит прекратить работу.
		/// </summary>
		internal static void Main()
		{
			while (true) // цикл работает, пока пользователь не решит прекратить
			{
				double a = InputFactor("a");
				double b = InputFactor("b");
				double c = InputFactor("c");
				double d = InputFactor("d");
				if (InputData(out string[] input) && GetArrays(input, out int[] ar1, out int[] ar2)) // если есть корректный набор данных, то делаем вывод в файл
				{
					string output = Calculate(ar1, ar2, a, b, c, d);
					OutputData(output);
				}
				if (IsFinish())
				{
					break;
				}
			}
		}
	}
}