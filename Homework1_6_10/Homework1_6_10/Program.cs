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
            Country country1 = new Country("Рим", ConsoleColor.Red, 6, 6);
            Country country2 = new Country("Греция", ConsoleColor.Blue, 6, 6);
            Battle battle = new Battle(country1.SendTroop(), country2.SendTroop());
        }
    }

    class Battle
    {
        private Troop _troopCountry1;
        private Troop _troopCountry2;
        private List<Soldier> _soldiers;
        private Battlefield _battlefield;

        public Battle(Troop troopCountry1, Troop troopCountry2)
        {
            _troopCountry1 = troopCountry1;
            _troopCountry2 = troopCountry2;
            _battlefield = new Battlefield(_troopCountry1.Field, _troopCountry2.Field);
            TakeAllSoldiers();

            while (_troopCountry1.IsHaveSoldiers() && _troopCountry2.IsHaveSoldiers())
            {
                ShowData();
                ClearDiedSoldiers();
                MakeTurn();
                Console.ReadKey();
            }

            if (_troopCountry1.IsHaveSoldiers() == false && _troopCountry2.IsHaveSoldiers() == false)
            {
                Console.WriteLine("Ничья");
            }
            else if (_troopCountry1.IsHaveSoldiers() == false)
            {
                Console.WriteLine("Победил/a  " + _troopCountry2.CountryName);
            }
            else if (_troopCountry2.IsHaveSoldiers() == false)
            {
                Console.WriteLine("Победил/a  " + _troopCountry1.CountryName);
            }
            else
            {
                Console.WriteLine("Ошибка");
            }
        }

        private void ShowData()
        {
            Console.Clear();
            _battlefield.DrawMap();
            Console.WriteLine();
            _troopCountry1.ShowSoldiersInfo();
            _troopCountry2.ShowSoldiersInfo();
        }

        private void ClearDiedSoldiers()
        {
            for (int i = 0; i < _soldiers.Count; i++)
            {
                if (_soldiers[i].IsAlive() == false)
                {
                    _soldiers.Remove(_soldiers[i]);
                    i--;
                }
            }

            _troopCountry1.ClearDiedSoldiers();
            _troopCountry2.ClearDiedSoldiers();
        }

        private void MakeTurn()
        {
            foreach (var soldier in _soldiers)
            {
                _battlefield.MakeMove(soldier);
            }
        }

        private void TakeAllSoldiers()
        {
            _soldiers = new List<Soldier>();
            _soldiers.AddRange(_troopCountry1.GetSoldiers());
            _soldiers.AddRange(_troopCountry2.GetSoldiers());
        }
    }

    class Country
    {
        private const int TroopFieldWidthDefault = 10;
        private const int TroopFieldLengthDefault = 10;

        private string _name;
        private Troop _troop;
        private ConsoleColor _color;

        public Country(string name, ConsoleColor color, int troopFieldWight = TroopFieldWidthDefault, int troopFieldLength = TroopFieldLengthDefault)
        {
            _name = name;
            _color = color;
            _troop = new Troop(_name, _color, troopFieldWight, troopFieldLength);
            _troop.PlaceSoldiers();
        }

        public Troop SendTroop()
        {
            return _troop;
        }
    }

    class Troop
    {
        private ConsoleColor _color;
        private Random _random;
        private List<Soldier> _soldiers;

        public Troop(string countryName, ConsoleColor color, int fieldWight, int fieldLength)
        {
            CountryName = countryName;
            _color = color;
            _random = new Random();
            Field = new Field(fieldWight, fieldLength);
            CreateSoldiers();
        }

        public string CountryName { get; private set; }
        public Field Field { get; private set; }

        public void PlaceSoldiers()
        {
            for (int i = _soldiers.Count - 1; i >= 0; i--)
            {
                Field.PlaceSoldier(_soldiers[i]);
            }
        }

        public bool IsHaveSoldiers()
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

        public List<Soldier> GetSoldiers()
        {
            return _soldiers;
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
            int minCountSoldiers = Field.Length * Field.Width / 5;
            int maxCountSoldiers = Field.Length * Field.Width / 4;
            int countSoldiers = _random.Next(minCountSoldiers, maxCountSoldiers + 1);
            _soldiers = new List<Soldier>();

            for (int i = 1; i <= countSoldiers; i++)
            {
                _soldiers.Add(new Soldier(i, _color));
            }
        }
    }

    class Soldier
    {
        private int _number;
        private int _health;
        private Skill _skill;
        private Random _random;

        public Soldier(int number, ConsoleColor color)
        {
            _number = number;
            _random = new Random();
            Color = color;
            FillData();
        }

        public int WightCoordinate { get; private set; }
        public int LenghtCoordinate { get; private set; }
        public int FiringRadius { get; private set; }
        public bool IsBigArrow { get; private set; }
        public ConsoleColor Color { get; private set; }
        public int Damage { get; private set; }

        public void MoveUp()
        {
            WightCoordinate--;
        }

        public void MoveDown()
        {
            WightCoordinate++;
        }

        public void MoveLeft()
        {
            LenghtCoordinate--;
        }

        public void MoveRight()
        {
            LenghtCoordinate++;
        }

        public void TakeCoordinates(int wight, int lenght)
        {
            WightCoordinate = wight;
            LenghtCoordinate = lenght;
        }

        public void Draw()
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            Console.Write(_number);
            Console.ForegroundColor = color;
        }

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
            Draw();
            Console.Write(": ");
            Console.Write(_health + " хп, " + Damage + " урона");
            Console.Write(". Навык - " + _skill);
            Console.Write(". Радиус полета стрелы - " + FiringRadius);
            Console.Write(". Расскалывающиеся стрелы - ");

            if (IsBigArrow)
            {
                Console.WriteLine("Да");
            }
            else
            {
                Console.WriteLine("Нет");
            }
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
            int firingRadiusDefault = 2;
            bool isBigArrowDefault = false;

            int minDamageStrong = 3;
            int maxDamageStrong = 6;
            int minHealthBig = 7;
            int maxHealthBig = 10;
            int firingRadiusLongRange = 4;
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
            Damage = _random.Next(minDamage, maxDamage + 1);
            _health = _random.Next(minHealth, maxHealth + 1);
            FiringRadius = firingRadius;
            IsBigArrow = isBigArrow;
        }
    }

    class Battlefield : Field
    {
        private Field _troop1Field;
        private Field _troop2Field;
        private Random _random;

        public Battlefield(Field troop1Field, Field troop2Field) : base(MaxNumber(troop1Field.Width, troop2Field.Width), troop1Field.Length + troop2Field.Length)
        {
            _troop1Field = troop1Field;
            _troop2Field = troop2Field;
            _random = new Random();
            FillUselessCells();
            FillSoldiers();
        }

        public void DrawMap()
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

        public void MakeMove(Soldier soldier)
        {
            int wight = soldier.WightCoordinate;
            int lenght = soldier.LenghtCoordinate;

            if (TryFindEnemy(soldier, out FieldCell enemyCell))
            {
                enemyCell.Attack(soldier.Damage);

                if (soldier.IsBigArrow)
                {
                    MakeBigDamage(enemyCell, soldier);
                }
            }
            else if (soldier.IsAlive())
            {
                int changeMode;
                int allModes = 4;

                changeMode = _random.Next(0, allModes);

                if (changeMode == 0)
                {
                    if (wight - 1 > 0)
                    {
                        if (Cells[wight - 1, lenght].IsHaveSoldier() == false)
                        {
                            soldier.MoveUp();
                            Cells[wight - 1, lenght].FillSoldier(soldier);
                            Cells[wight, lenght].FillSoldier(null);
                        }
                    }
                }
                else if (changeMode == 1)
                {
                    if (wight + 1 < Width)
                    {
                        if (Cells[wight + 1, lenght].IsHaveSoldier() == false)
                        {
                            soldier.MoveDown();
                            Cells[wight + 1, lenght].FillSoldier(soldier);
                            Cells[wight, lenght].FillSoldier(null);
                        }
                    }
                }
                else if (changeMode == 2)
                {
                    if (lenght - 1 > 0)
                    {
                        if (Cells[wight, lenght - 1].IsHaveSoldier() == false)
                        {
                            soldier.MoveLeft();
                            Cells[wight, lenght - 1].FillSoldier(soldier);
                            Cells[wight, lenght].FillSoldier(null);
                        }
                    }
                }
                else if (changeMode == 3)
                {
                    if (lenght + 1 < Length)
                    {
                        if (Cells[wight, lenght + 1].IsHaveSoldier() == false)
                        {
                            soldier.MoveRight();
                            Cells[wight, lenght + 1].FillSoldier(soldier);
                            Cells[wight, lenght].FillSoldier(null);
                        }
                    }
                }
            }
        }

        private static int MaxNumber(int number1, int number2)
        {
            if (number1 > number2)
            {
                return number1;
            }

            return number2;
        }

        private static int MinNumber(int number1, int number2)
        {
            if (number1 < number2)
            {
                return number1;
            }

            return number2;
        }

        private void MakeBigDamage(FieldCell enemyCell, Soldier soldier)
        {
            if (enemyCell.WightCoordinate + 1 < Width)
            {
                Cells[enemyCell.WightCoordinate + 1, enemyCell.LenghtCoordinate].Attack(soldier.Damage);
            }

            if (enemyCell.LenghtCoordinate + 1 < Length)
            {
                Cells[enemyCell.WightCoordinate, enemyCell.LenghtCoordinate + 1].Attack(soldier.Damage);
            }

            if (enemyCell.LenghtCoordinate - 1 > 0)
            {
                Cells[enemyCell.WightCoordinate, enemyCell.LenghtCoordinate - 1].Attack(soldier.Damage);
            }

            if (enemyCell.WightCoordinate - 1 > 0)
            {
                Cells[enemyCell.WightCoordinate - 1, enemyCell.LenghtCoordinate].Attack(soldier.Damage);
            }

            if (enemyCell.WightCoordinate + 1 < Width && enemyCell.LenghtCoordinate + 1 < Length)
            {
                Cells[enemyCell.WightCoordinate + 1, enemyCell.LenghtCoordinate + 1].Attack(soldier.Damage);
            }

            if (enemyCell.WightCoordinate + 1 < Width && enemyCell.LenghtCoordinate - 1 > 0)
            {
                Cells[enemyCell.WightCoordinate + 1, enemyCell.LenghtCoordinate - 1].Attack(soldier.Damage);
            }

            if (enemyCell.WightCoordinate - 1 > 0 && enemyCell.LenghtCoordinate + 1 < Length)
            {
                Cells[enemyCell.WightCoordinate - 1, enemyCell.LenghtCoordinate + 1].Attack(soldier.Damage);
            }

            if (enemyCell.WightCoordinate - 1 > 0 && enemyCell.LenghtCoordinate - 1 > 0)
            {
                Cells[enemyCell.WightCoordinate - 1, enemyCell.LenghtCoordinate - 1].Attack(soldier.Damage);
            }
        }

        private bool TryFindEnemy(Soldier soldier, out FieldCell enemyCell)
        {
            for (int i = 1; i <= soldier.FiringRadius; i++)
            {
                if (soldier.WightCoordinate - i > 0)
                {
                    if (Cells[soldier.WightCoordinate - i, soldier.LenghtCoordinate].IsHaveSoldier())
                    {
                        if (Cells[soldier.WightCoordinate - i, soldier.LenghtCoordinate].IsEnemy(soldier.Color))
                        {
                            enemyCell = Cells[soldier.WightCoordinate - i, soldier.LenghtCoordinate];
                            return true;
                        }
                    }
                }
                if (soldier.WightCoordinate + i < Width)
                {
                    if (Cells[soldier.WightCoordinate + i, soldier.LenghtCoordinate].IsHaveSoldier())
                    {
                        if (Cells[soldier.WightCoordinate + i, soldier.LenghtCoordinate].IsEnemy(soldier.Color))
                        {
                            enemyCell = Cells[soldier.WightCoordinate + i, soldier.LenghtCoordinate];
                            return true;
                        }
                    }
                }
                if (soldier.LenghtCoordinate - i > 0)
                {
                    if (Cells[soldier.WightCoordinate, soldier.LenghtCoordinate - i].IsHaveSoldier())
                    {
                        if (Cells[soldier.WightCoordinate, soldier.LenghtCoordinate - i].IsEnemy(soldier.Color))
                        {
                            enemyCell = Cells[soldier.WightCoordinate, soldier.LenghtCoordinate - i];
                            return true;
                        }
                    }
                }
                if (soldier.LenghtCoordinate + i < Length)
                {
                    if (Cells[soldier.WightCoordinate, soldier.LenghtCoordinate + i].IsHaveSoldier())
                    {
                        if (Cells[soldier.WightCoordinate, soldier.LenghtCoordinate + i].IsEnemy(soldier.Color))
                        {
                            enemyCell = Cells[soldier.WightCoordinate, soldier.LenghtCoordinate + i];
                            return true;
                        }
                    }
                }
            }

            enemyCell = null;
            return false;
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
    }

    class Field
    {
        private Random _random;

        public Field(int width, int length)
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
            soldier.TakeCoordinates(cellNumber / Length, cellNumber % Length);
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
                        Cells[i, j] = new FieldCell(i, j);
                    }
                }
            }
        }

        protected void CopyCells(int endWidth, int endLength, Field field, int beginWidth = 0, int beginLength = 0)
        {
            for (int i = beginWidth; i < endWidth; i++)
            {
                for (int j = beginLength; j < endLength; j++)
                {
                    Cells[i, j] = field.Cells[i - beginWidth, j - beginLength];

                    if (beginWidth != 0 || beginLength != 0)
                    {
                        Cells[i, j].ShiftCoordinates(beginWidth, beginLength);
                    }
                }
            }
        }
    }

    class FieldCell
    {
        private Soldier _soldier;

        public FieldCell(int wightCoordinate, int lenghtCoordinate)
        {
            WightCoordinate = wightCoordinate;
            LenghtCoordinate = lenghtCoordinate;
            _soldier = null;
        }

        public int WightCoordinate { get; private set; }
        public int LenghtCoordinate { get; private set; }

        public void FillSoldier(Soldier soldier)
        {
            _soldier = soldier;

            TakeSoldierCoordinates();
        }

        public void ShiftCoordinates(int wightCoordinate, int lenghtCoordinate)
        {
            WightCoordinate += wightCoordinate;
            LenghtCoordinate += lenghtCoordinate;

            TakeSoldierCoordinates();
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
            return _soldier != null;
        }

        public bool IsEnemy(ConsoleColor color)
        {
            if (IsHaveSoldier())
            {
                if (_soldier.Color != color)
                {
                    return true;
                }
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

        private void TakeSoldierCoordinates()
        {
            if (IsHaveSoldier())
            {
                _soldier.TakeCoordinates(WightCoordinate, LenghtCoordinate);
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
