using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DysonRecipeWin
{
	class Calc
	{
	}


	public struct Number
	{
		public int num;
		public int dividedNum;

		public Number(string str)
		{
			var strs = str.Split('/');
			if (strs.Length == 1)
			{
				num = int.Parse(strs[0]);
				dividedNum = 1;
			}
			else
			{
				num = int.Parse(strs[0]);
				dividedNum = int.Parse(strs[1]);
			}
		}

		public Number(int num, int dividedNum)
		{
			this.num = num;
			this.dividedNum = dividedNum;
			ProcessResult();
		}

		void ProcessResult()
		{
			var tmp = GetLargestCommonDivisor(num, dividedNum);
			num /= tmp;
			dividedNum /= tmp;
		}

		static int GetLargestCommonDivisor(int n1, int n2)
		{
			int max = n1 > n2 ? n1 : n2;
			int min = n1 < n2 ? n1 : n2;
			int remainder;
			while (min != 0)
			{
				remainder = max % min;
				max = min;
				min = remainder;
			}
			return max;
		}

		static int GetLeastCommonMutiple(int n1, int n2)
		{
			return n1 * n2 / GetLargestCommonDivisor(n1, n2);
		}

		public static Number operator +(Number n1, Number n2)
		{
			var divNum = GetLeastCommonMutiple(n1.dividedNum, n2.dividedNum);
			var num = n1.num * divNum / n1.dividedNum + n2.num * divNum / n2.dividedNum;
			var number = new Number(num, divNum);
			number.ProcessResult();
			return number;
		}

		public static Number operator *(Number n1, Number n2)
		{
			var number = new Number(n1.num * n2.num, n1.dividedNum * n2.dividedNum);
			number.ProcessResult();
			return number;
		}

		public static Number operator /(Number n1, Number n2)
		{
			var number = new Number(n1.num * n2.dividedNum, n1.dividedNum * n2.num);
			number.ProcessResult();
			return number;
		}

		public static implicit operator Number(int num)
		{
			return new Number(num, 1);
		}

		public override string ToString()
		{
			if (dividedNum == 1)
				return num.ToString();
			return num + "/" + dividedNum;
		}

		public float ToFloat()
		{
			return (float)num / dividedNum;
		}
	}

	public class BuildingNeed
	{
		public string building;
		public string itemName;
		public Number count;
		public List<BuildingNeed> childs = new List<BuildingNeed>();
		public int level;
	}

	public class BuildingTargetPack
	{
		public BuildingNeed parent;
		public ItemPack itemPack;
	}
}