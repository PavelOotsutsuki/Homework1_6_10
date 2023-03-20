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
            Country country1 = new Country("Рим", ConsoleColor.Red, 6 , 6);
            Country country2 = new Country("Греция", ConsoleColor.Blue, 6 , 6);
            Battle battle = new Battle(country1, country2);
            battle.Begin();
        }
    }

    class Battle
    {
        //private Troop _troopCountry1;
        //private Troop _troopCountry2;
        private Country _country1;
        private Country _country2;

        private Battlefield _battlefield;

        //public Battle(Troop troopCountry1, Troop troopCountry2)
        //{
        //    _troopCountry1 = troopCountry1;
        //    _troopCountry2 = troopCountry2;
        //    CreateBattlefield();
        //}

        public Battle(Country country1, Country country2)
        {
            _country1 = country1;
            _country2 = country2;
        }

        public void Begin()
        {
            CreateBattlefield();
        }

        private void CreateBattlefield()
        {
            _battlefield = new Battlefield(_country1, _country2);
        }
    }

    class Country
    {
        private const int TroopFieldWidthDefault = 10;
        private const int TroopFieldLengthDefault = 10;

        private string _name;
        private Troop _troop;
        private ConsoleColor _color;

        public Country (string name, ConsoleColor color, int troopFieldWight = TroopFieldWidthDefault, int troopFieldLength = TroopFieldLengthDefault)
        {
            _name = name;
            _color = color;
            _troop = new Troop(_name, _color, troopFieldWight, troopFieldLength);
            _troop.PlaceSoldiers();
        }

        public int GetTroopFieldWight()
        {
            return _troop.GetFieldWight();
        }

        public int GetTroopFieldLenght()
        {
            return _troop.GetFieldLength();
        }

        public FieldCell GetTroopFieldCell(int wight, int lenght)
        {

        }
    }

    class Troop
    {
        private string _countryName;
        private ConsoleColor _color;
        private Random _random;
        private List<Soldier> _soldiers;
        private Field _field;

        public Troop(string countryName, ConsoleColor color, int fieldWight, int fieldLength)
        {
            _countryName = countryName;
            _color = color;
            _random = new Random();
            _field = new Field(fieldWight, fieldLength);
            CreateSoldiers();
        }

        public void PlaceSoldiers()
        {
            for (int i = _soldiers.Count-1; i >= 0; i-- )
            {
                _field.PlaceSoldier(_soldiers[i]);
                _soldiers.Remove(_soldiers[i]);
            }
        }

        private void CreateSoldiers()
        {
            int minCountSoldiers = _field.Length * _field.Width / 5;
            int maxCountSoldiers = _field.Length * _field.Width / 4;
            int countSoldiers = _random.Next(minCountSoldiers, maxCountSoldiers + 1);
            _soldiers = new List<Soldier>();

            for (int i = 1; i <= countSoldiers; i++)
            {
                _soldiers.Add(new Soldier(i, _color));
            }
        }

        public int GetFieldWight()
        {
            return _field.Width;
        }

        public int GetFieldLength()
        {
            return _field.Length;
        }
    }

    class Soldier
    {
        private ConsoleColor _color;
        private int _number;
        private int _damage;
        private int _health;
        private int _firingRadius;
        private bool _isBigArrow;
        private Skill _skill;
        private Random _random;

        public Soldier(int number, ConsoleColor color)
        {
            _number = number;
            _random = new Random();
            _color = color;
            FillData();
        }

        public void Draw()
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = _color;
            Console.Write(_number);
            Console.ForegroundColor = color;
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
            int firingRadiusDefault = 8;
            bool isBigArrowDefault = false;

            int minDamageStrong = 3;
            int maxDamageStrong = 6;
            int minHealthBig = 7;
            int maxHealthBig = 10;
            int firingRadiusLongRange = 16;
            bool isBigArrowBigRange = true;

            FillSkill();

            switch (_skill)
            {
                case Skill.Strong:
                    FillCharacteristics(minDamageStrong, maxDamageStrong, minHealthDefault, maxHealthDefault, firingRadiusDefault, isBigArrowDefault);
                    break;

                case Skill.Big:
                    FillCharacteristics(minDamageDefault, maxDamageDefault, minHealthBig, maxHealthBig, firingRadiusDefault, isBigArrowDefault);
                    break;

                case Skill.LongRange:
                    FillCharacteristics(minDamageDefault, maxDamageDefault, minHealthDefault, maxHealthDefault, firingRadiusLongRange, isBigArrowDefault);
                    break;

                case Skill.BigRange:
                    FillCharacteristics(minDamageDefault, maxDamageDefault, minHealthDefault, maxHealthDefault, firingRadiusDefault, isBigArrowBigRange);
                    break;

                default:
                    Console.WriteLine("Ошибка выбора навыка");
                    Console.ReadKey();
                    break;
            }
        }

        private void FillCharacteristics(int minDamage, int maxDamage, int minHealth, int maxHealth, int firingRadius, bool isBigArrow)
        {
            _damage = _random.Next(minDamage, maxDamage + 1);
            _health = _random.Next(minHealth, maxHealth + 1);
            _firingRadius = firingRadius;
            _isBigArrow = isBigArrow;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
        }

        public bool IsAlive()
        {
            return _health > 0;
        }
    }

    class Battlefield: Field
    {
        private Country _country1;
        private Country _country2;

        public Battlefield (Country country1, Country country2) : base(MaxNumber(country1.GetTroopFieldWight(), country2.GetTroopFieldWight()), country1.GetTroopFieldLenght() + country2.GetTroopFieldLenght())
        {
            _country1 = country1;
            _country2 = country2;
            FillUselessCells();
            FillSoldiers();
            FullWriteLine();
        }

        static private int MaxNumber(int number1, int number2)
        {
            if (number1 > number2)
            {
                return number1;
            }

            return number2;
        }

        static private int MinNumber(int number1, int number2)
        {
            if (number1 < number2)
            {
                return number1;
            }

            return number2;
        }

        private void FillSoldiers()
        {
            CopyCells(_troop1Field.Width, _troop1Field.Length, _troop1Field);
            CopyCells(_troop2Field.Width, Length, _troop2Field, beginLength: _troop1Field.Length);
        }

        private void FillUselessCells()
        {
            if (_troop1Field.Width != _troop2Field.Width)
            {
                int endWidth = Width;
                int endLength;
                int beginWidth = Width - MinNumber(_troop1Field.Width, _troop2Field.Width);
                int beginLength;

                if (_troop1Field.Width > _troop2Field.Width)
                {
                    endLength = Length;
                    beginLength = _troop1Field.Length;
                }
                else
                {
                    endLength = _troop1Field.Length;
                    beginLength = 0;
                }

                FillCells(endWidth, endLength, true, beginWidth, beginLength);
            }
        }

        private void FullWriteLine()
        {
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    if (Cells[i, j] == null)
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Cells[i, j].DrawSoldier();
                    }

                }

                Console.WriteLine();
            }
        }
    }

    class Field
    {
        private Random _random;

        public Field (int width, int length)
        {
            Width = width;
            Length = length;
            _random = new Random();
            Cells = new FieldCell[Width, Length];
            FillCells(Width, Length, false);
        }

        public int Width { get; private set; }
        public int Length { get; private set; }
        public FieldCell[,] Cells { get; protected set; }

        public void PlaceSoldier(Soldier soldier)
        {
            int minCellNumber = 0;
            int maxCellNumber = Width * Length - 1;
            int cellNumber;

            do
            {
                cellNumber = _random.Next(minCellNumber, maxCellNumber + 1);
            }
            while (Cells[cellNumber / Length, cellNumber % Length].IsHaveSoldier());

            Cells[cellNumber / Length, cellNumber % Length].FillSoldier(soldier);
        }

        protected void FillCells(int endWidth, int endLength, bool isNull, int beginWidth = 0, int beginLength = 0)
        {
            for (int i = beginWidth; i < endWidth; i++)
            {
                for (int j = beginLength; j < endLength; j++)
                {
                    if (isNull == true)
                    {
                        Cells[i, j] = null;
                    }
                    else
                    {
                        Cells[i, j] = new FieldCell();
                    }
                
                }
            }
        }

        protected void CopyCells(int endWidth, int endLength, Country country, int beginWidth = 0, int beginLength = 0)
        {
            for (int i = beginWidth; i < endWidth; i++)
            {
                for (int j = beginLength; j < endLength; j++)
                {
                    Cells[i, j] = field.Cells[i - beginWidth, j - beginLength];
                }
            }
        }
    }

    class FieldCell
    {
        private Soldier _soldier;

        public FieldCell()
        {
            _soldier = null;
        }

        public void FillSoldier(Soldier soldier)
        {
            _soldier = soldier;
        }

        public void Attack(int damage)
        {
            if (IsHaveSoldier())
            {
                _soldier.TakeDamage(damage);

                if (_soldier.IsAlive() == false)
                {
                    _soldier = null;
                }
            }
        }

        public bool IsHaveSoldier()
        {
            if (_soldier != null)
            {
                return true;
            }

            return false;
        }

        public void DrawSoldier()
        {
            if (IsHaveSoldier())
            {
                _soldier.Draw();
            }
            else
            {
                Console.Write(".");
            }
        }
    }

    enum Skill
    {
        Strong,
        Big,
        LongRange,
        BigRange,
        Length
    }
}
