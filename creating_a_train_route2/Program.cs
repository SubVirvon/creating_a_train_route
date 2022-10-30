using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace creating_a_train_route2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Controller controller = new Controller();

            controller.Work();
        }
    }

    class Controller
    {
        private List<Line> _lines;

        public Controller()
        {
            _lines = new List<Line>();
        }

        public void Work()
        {
            while (true)
            {
                CreateLine();
                int ticketsCount = SellTickets();
                Train train = CreateTrain(ticketsCount);
                SendTrain(_lines[_lines.Count - 1], train);
                SkipHour();
            }
        }

        private void CreateLine()
        {
            Console.Clear();

            UpdateLinesInfo();

            Console.WriteLine("Создать направление");
            Console.Write("Отправной пункт: ");

            string startingPoint = Console.ReadLine();

            Console.Write("Конечный пункт: ");

            string endingPoint = Console.ReadLine();
            int lineLength = ReadInt("Длина пути: ");

            _lines.Add(new Line(startingPoint, endingPoint, lineLength));

            ShowLineCreationStatus("Путь успешно создан");
        }

        private int SellTickets()
        {
            Random random = new Random();
            int minTicketsCount = 200;
            int maxTicketsCount = 1000;
            int ticketsCount = random.Next(minTicketsCount, maxTicketsCount + 1);

            ShowLineCreationStatus($"Куплено {ticketsCount} билетов");

            return ticketsCount;
        }

        private Train CreateTrain(int ticketsCount)
        {
            const int HundredsMultiplier = 10;
            Random random = new Random();
            int minCapacity = 3;
            int maxCapacity = 6;
            int capacity = random.Next(minCapacity, maxCapacity + 1) * HundredsMultiplier;
            Train train = new Train(ticketsCount);

            while(ticketsCount > 0)
            {
                int passengersCount;

                if (ticketsCount >= capacity)
                    passengersCount = capacity;
                else
                    passengersCount = ticketsCount;

                train.AddWagon(capacity, passengersCount);
                ticketsCount -= passengersCount;
            }

            ShowLineCreationStatus("Поезд сформирован");

            return train;
        }

        private void SendTrain(Line line, Train train)
        {
            line.Addtrain(train);

            ShowLineCreationStatus("Поезд отправлен");
        }

        private void ShowLineCreationStatus(string output)
        {
            UpdateLinesInfo();

            Console.WriteLine(output + "\nЧтобы продолжить нажмите любую кнопку");
            Console.ReadKey();
        }

        private void UpdateLinesInfo()
        {
            Console.Clear();

            LinesInfo();

            Console.SetCursorPosition(0, 20);
        }

        private void LinesInfo()
        {
            foreach(var line in _lines)
            {
                if (line.Train != null)
                    line.ShowInfo();
            }
        }

        private int ReadInt(string outPutText)
        {
            bool isNumber = false;
            int number = 0;

            while (isNumber == false)
            {
                Console.Write(outPutText);

                string input = Console.ReadLine();

                if (int.TryParse(input, out number))
                    isNumber = true;
                else
                    Console.WriteLine("Некоректное число");
            }

            return number;
        }

        private void SkipHour()
        {
            for(int i = 0; i < _lines.Count - 1; i++)
            {
                _lines[i].Train.Move();
            }
        }
    }

    class Line
    {
        private int _length;
        private string _startingPoint;
        private string _endingPoint;
        public Train Train { get; private set; }

        public Line(string startingPoint, string endingPoint, int length)
        {
            Train = null;
            _length = length;
            _startingPoint = startingPoint;
            _endingPoint = endingPoint;
        }

        public void Addtrain(Train train)
        {
            Train = train;
        }

        public void ShowInfo()
        {
            if(Train.PassedDistance < _length)
                Console.WriteLine($"{_startingPoint} - {_endingPoint} ({_length}км):\nКоличесво вагонов: {Train.Wagons.Count} (вместимость: {Train.Wagons[0].Сapacity}), количество пассажиров: {Train.TicketsCount}\nВ пути: поезд имеет скорость: {Train.Speed}км/ч, пройденное расстояние: {Train.PassedDistance}");
            else
                Console.WriteLine($"{_startingPoint} - {_endingPoint} ({_length}км): прибыл");
        }
    }

    class Train
    {
        public int Speed { get; private set; }
        public int PassedDistance { get; private set; }
        public int TicketsCount { get; private set; }
        public List<Wagon> Wagons { get; private set; }

        public Train(int ticketsCount)
        {
            TicketsCount = ticketsCount;
            Speed = SetSpeed();
            PassedDistance = 0;
            Wagons = new List<Wagon>();
        }

        public void AddWagon(int capacity, int passengersCount)
        {
            Wagons.Add(new Wagon(capacity, passengersCount));
        }

        public void Move()
        {
            PassedDistance += Speed;
        }

        private int SetSpeed()
        {
            const int HundredsMultiplier = 10;
            Random random = new Random();
            int minSpeed = 10;
            int maxSpeed = 20;
            int speed = random.Next(minSpeed, maxSpeed + 1) * HundredsMultiplier;

            return speed;
        }
    }

    class Wagon
    {
        public int Сapacity { get; private set; }
        public int PassengersCount { get; private set; }

        public Wagon(int сapacity, int passengersCount)
        {
            Сapacity = сapacity;
            PassengersCount = passengersCount;
        }
    }
}
