// Тищенко Никита Алексеевич БПИ-247-1 Вариант 10

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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
		/// Читает номер последнего output файла из config.txt.
		/// </summary>
		/// <returns>
		/// Номер последнего output файла в формате int.
		/// </returns>
		internal static int ReadConfig()
		{
			string config_path = Path.GetFullPath("../../../../config.txt");
			int ans;
			try // пытаемся прочитать config файл и получить оттуда номер последнего output файла
			{
				string[] config = Array.FindAll(File.ReadAllLines(config_path), tmp => tmp != ""); // выделяем все непустые строки, записанные в файл
				ans = int.Parse(config[0]);
			}
			catch // в случае неудачи номером последнего output файла считаем 0
			{
				ans = 0;
			}
			try // пытаемся обновить номер последнего output файла в config файле
			{
				File.WriteAllText(config_path, (ans + 1).ToString());
			}
			catch { } // в случае неудачи config файл не изменится
			return ans;
		}
		/// <summary>
		/// Выводит заданную строку в файл.
		/// </summary>
		internal static void OutputData(string s)
		{
			int num = ReadConfig() + 1;
			string output_path = Path.GetFullPath($"../../../../output-{num}.txt");
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
		/// Осуществляет ввод данных из файла и записывает их в качестве массива массивов целочисленных значений. 
		/// </summary>
		/// <returns>
		/// true в случае успешного ввода и false в обратном случае.
		/// </returns>
		internal static bool InputData(out string[] a)
		{
			string path = Path.GetFullPath("../../../../input.txt");
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
		/// Выделяет наборы данных из ввода и гененрирует вывод для них в формате string.
		/// </summary>
		/// <returns>
		/// Вывод для всех наборов данных в формате string.
		/// </returns>
		/// <remarks>
		/// Между выводами для каждого набора данных выдерживается интервал в одну пустую строку.
		/// </remarks>
		internal static string CalculateAll(string[] input, double a, double b, double c, double d)
		{
			string output = "";
			for (int i = 0; i + 1 < input.Length; i += 2) // перебираем строки парами i и i + 1, как они идут в наборах данных
			{                                             // если у последней строки нет пары, это значит, что последний набор данных некорректен и вывод к нему будет пустой строкой
				int[] ar1 = ArrayFromString(input[i]);
				int[] ar2 = ArrayFromString(input[i + 1]);
				output += Calculate(ar1, ar2, a, b, c, d) + "\n";
			}
			return output;
		}
		/// <summary>
		/// Обрабатывает введеный набор данных и генерирует вывод для него.
		/// </summary>
		/// <returns>
		/// Вывод для введенного набора данных в формате string.
		/// </returns>
		/// /// <remarks>
		/// В случае если набор данных некорректен вместо него выводится пустая строка.
		/// </remarks>
		internal static string Calculate(int[] ar1, int[] ar2, double a, double b, double c, double d)
		{
			string output;
			if (ar1.Length == 0 && ar2.Length == 0) // если оба массива пусты, то набор данных некорректен и будет выведена пустая строка
			{
				output = "\n";
			}
			else if (ar1.Length == ar2.Length) // если длины массивов равны, можно сгенерировать вывод
			{
				double[] ans = new double[ar1.Length];
				for (int i = 0; i < ar1.Length; i++)
				{
					ans[i] = ((a * ar1[i]) + b) / ((c * ar2[i]) + d);
				}
				output = StringFromArray(ans) + "\n";
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
				if (InputData(out string[] input))
				{
					string output = CalculateAll(input, a, b, c, d);
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