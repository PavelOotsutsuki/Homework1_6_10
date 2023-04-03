using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Homework1_6_10
{
    class Program
    {
        static void Main(string[] args)
        {
            Troop troop1 = new Troop("Рим");
            Troop troop2 = new Troop("Греция");
            Battle battle = new Battle(troop1, troop2);
        }
    }

    class Battle
    {
        private Troop _troop1;
        private Troop _troop2;

        public Battle(Troop troop1, Troop troop2)
        {
            _troop1 = troop1;
            _troop2 = troop2;

            while (_troop1.HaveSoldiers() && _troop2.HaveSoldiers())
            {
                ShowData();
                ClearDiedSoldiers();
                MakeTurn();
                Console.ReadKey();
            }

            if (_troop1.HaveSoldiers() == false && _troop2.HaveSoldiers() == false)
            {
                Console.WriteLine("Ничья");
            }
            else if (_troop1.HaveSoldiers() == false)
            {
                Console.WriteLine("Победил/a  " + _troop2.CountryName);
            }
            else if (_troop2.HaveSoldiers() == false)
            {
                Console.WriteLine("Победил/a  " + _troop1.CountryName);
            }
            else
            {
                Console.WriteLine("Ошибка");
            }
        }

        private void ShowData()
        {
            Console.Clear();
            _troop1.ShowSoldiersInfo();
            Console.WriteLine();
            _troop2.ShowSoldiersInfo();
        }

        private void ClearDiedSoldiers()
        {
            _troop1.ClearDiedSoldiers();
            _troop2.ClearDiedSoldiers();
        }

        private void MakeTurn()
        {
            List<int> damage = new List<int>();

            _troop2.TakeDamage(_troop1.GetListDamage());
            _troop1.TakeDamage(_troop2.GetListDamage());
        }
    }

    class Troop
    {
        private Random _random;
        private List<Soldier> _soldiers;

        public Troop(string countryName)
        {
            CountryName = countryName;
            _random = new Random();
            CreateSoldiers();
        }

        public string CountryName { get; private set; }

        public bool HaveSoldiers()
        {
            return _soldiers.Count > 0;
        }

        public void ClearDiedSoldiers()
        {
            for (int i = 0; i < _soldiers.Count; i++)
            {
                if (_soldiers[i].IsAlive() == false)
                {
                    _soldiers.Remove(_soldiers[i]);
                    i--;
                }
            }
        }

        public void TakeDamage(List<int> damageList)
        {
            int attackedSoldierIndex;

            if (HaveSoldiers())
            {
                foreach (int damage in damageList)
                {
                    attackedSoldierIndex = _random.Next(0, _soldiers.Count);
                    _soldiers[attackedSoldierIndex].TakeDamage(damage);
                }
            }
        }

        public List<int> GetListDamage()
        {
            List<int> damageList = new List<int>();

            foreach (var soldier in _soldiers)
            {
                damageList.Add(soldier.Damage);
            }

            return damageList;
        }

        public void ShowSoldiersInfo()
        {
            foreach (var soldier in _soldiers)
            {
                soldier.ShowInfo();
            }
        }

        private void CreateSoldiers()
        {
            int minCountSoldiers = 7;
            int maxCountSoldiers = 9;
            int countSoldiers = _random.Next(minCountSoldiers, maxCountSoldiers + 1);
            _soldiers = new List<Soldier>();

            for (int i = 1; i <= countSoldiers; i++)
            {
                _soldiers.Add(new Soldier(i));
            }
        }
    }

    class Soldier
    {
        private int _number;
        private int _health;
        private Skill _skill;
        private Random _random;

        public Soldier(int number)
        {
            _number = number;
            _random = new Random();
            FillData();
        }

        public int WightCoordinate { get; private set; }
        public int LenghtCoordinate { get; private set; }
        public int Damage { get; private set; }

        public void TakeDamage(int damage)
        {
            _health -= damage;
        }

        public bool IsAlive()
        {
            return _health > 0;
        }

        public void ShowInfo()
        {
            Console.Write(_number);
            Console.Write(": ");
            Console.Write(_health + " хп, " + Damage + " урона");
            Console.WriteLine(". Навык - " + _skill);
        }

        private void FillSkill()
        {
            int minSkill = 0;
            int maxSkill = (int)Skill.Length;

            _skill = (Skill)_random.Next(minSkill, maxSkill);
        }

        private void FillData()
        {
            int minDamageDefault = 1;
            int maxDamageDefault = 3;
            int minHealthDefault = 4;
            int maxHealthDefault = 6;

            int minDamageStrong = 3;
            int maxDamageStrong = 6;
            int minHealthBig = 7;
            int maxHealthBig = 10;

            FillSkill();

            switch (_skill)
            {
                case Skill.Strong:
                    FillCharacteristics(minDamageStrong, maxDamageStrong, minHealthDefault, maxHealthDefault);
                    break;

                case Skill.Big:
                    FillCharacteristics(minDamageDefault, maxDamageDefault, minHealthBig, maxHealthBig);
                    break;

                case Skill.Experienced:
                    FillCharacteristics(minDamageStrong, maxDamageStrong, minHealthBig, maxHealthBig);
                    break;

                case Skill.Beginner:
                    FillCharacteristics(minDamageDefault, maxDamageDefault, minHealthDefault, maxHealthDefault);
                    break;

                default:
                    Console.WriteLine("Ошибка выбора навыка");
                    Console.ReadKey();
                    break;
            }
        }

        private void FillCharacteristics(int minDamage, int maxDamage, int minHealth, int maxHealth)
        {
            Damage = _random.Next(minDamage, maxDamage + 1);
            _health = _random.Next(minHealth, maxHealth + 1);
        }
    }

    enum Skill
    {
        Strong,
        Big,
        Experienced,
        Beginner,
        Length
    }
}