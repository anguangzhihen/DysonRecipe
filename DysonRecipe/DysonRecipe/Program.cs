using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonRecipe
{
    public enum ItemType
    {
        IronOre,
        Iron,
        CopperOre,
        Copper,
        Magnet,
        MagnetCoil,

        // building
        Furnace,
        Manufacture
    }

    public class Item
    {
        public Item(string name)
        {
            this.name = name;
        }

        public string name;
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

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("注意：氢和原油的生产较为复杂暂时忽略");
            Console.WriteLine();

            Data.Load();
            try
            {
                Calc();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("失败，计算出错");
                Console.ReadLine();
            }
        }

        static void Calc()
        {
            ItemPack target = new ItemPack("石墨烯", new Number(2, 3));

            var buildingEffective = new Dictionary<string, Number>()
            {
                { "电弧熔炉", 1 },
                { "制造台", new Number(1, 1)},
                { "化工厂", 1 },
                { "微型粒子对撞器", 1 },
			};

            Dictionary<string, Dictionary<string, Number>> buildingNeed = new Dictionary<string, Dictionary<string, Number>>();

            Dictionary<string, Recipe> nameToRecipes = new Dictionary<string, Recipe>();
            foreach (var recipe in Data.recipes)
            {
                if (!nameToRecipes.ContainsKey(recipe.target.name))
                {
                    nameToRecipes[recipe.target.name] = recipe;
                }
            }

            List<ItemPack> tmp = new List<ItemPack>();
            List<ItemPack> result = new List<ItemPack>();
            tmp.Add(target);

            int tmpCount = 0;
            while (tmp.Count > 0)
            {
                tmpCount++;
                if (tmpCount > 1000)
                {
                    Console.WriteLine("失败，配方产生了循环");
                    return;
                }
                var itemPack = tmp[0];
                if (!nameToRecipes.ContainsKey(itemPack.name))
                {
                    result.Add(tmp[0]);
                    tmp.RemoveAt(0);
                }
                else
                {
                    var recipe = nameToRecipes[itemPack.name];
                    foreach (var need in recipe.needs)
                    {
                        var tmpNeed = need; ;
                        tmpNeed.count = tmpNeed.count * itemPack.count;
                        tmpNeed.count /= recipe.target.count;
                        tmp.Add(tmpNeed);
                    }
                    Number buildingCount;
                    Dictionary<string, Number> dict;
                    if (!buildingNeed.TryGetValue(recipe.building, out dict))
                    {
                        buildingNeed[recipe.building] = new Dictionary<string, Number>();
                        dict = buildingNeed[recipe.building];
                    }
                    if (!dict.TryGetValue(recipe.target.name, out buildingCount))
                    {
                        buildingCount = new Number(0, 1);
                    }
                    var plusCount = recipe.time / buildingEffective[recipe.building];
                    plusCount = plusCount * itemPack.count / recipe.target.count;
                    dict[recipe.target.name] = buildingCount + plusCount;

                    tmp.RemoveAt(0);
                }
            }

            Console.WriteLine("目标：" + target + "个每秒");
            Console.WriteLine();
            Console.WriteLine("当前设备效率：");
            foreach (var eff in buildingEffective)
            {
                Console.WriteLine(string.Format("\t{0} {1:0%}", eff.Key, eff.Value.ToFloat()));
            }
            Console.WriteLine();
            Console.WriteLine("需求矿石：");
            foreach (var itemPack in result)
            {
                var tmpItemPack = itemPack;
                tmpItemPack.count *= 60;
                Console.WriteLine("\t" + tmpItemPack + "个每分");
            }
            Console.WriteLine();
            Console.WriteLine("需求设施：");
            foreach (var pair in buildingNeed)
            {
                foreach (var pair2 in pair.Value)
                {
                    var recipe = nameToRecipes[pair2.Key];
                    Number effctive = buildingEffective[recipe.building];
                    Console.WriteLine("\t" + pair.Key + "(" + pair2.Key + ") " + pair2.Value + " " + recipe.ToSpeedString(effctive));
                }
            }
            Console.ReadLine();
        }
        
    }
}
