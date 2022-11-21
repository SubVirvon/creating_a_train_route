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
            bool isFinish = false;

            while (isFinish == false)
            {
                _lines.Add(CreateLine());
                int ticketsCount = SellTickets();
                Train train = CreateTrain(ticketsCount);
                SendTrain(_lines[_lines.Count - 1], train);
                SkipHour();
            }
        }

        private Line CreateLine()
        {
            Console.Clear();

            UpdateLinesInfo();

            Console.WriteLine("Создать направление");
            Console.Write("Отправной пункт: ");

            string startingPoint = Console.ReadLine();

            Console.Write("Конечный пункт: ");

            string endingPoint = Console.ReadLine();
            int lineLength = ReadInt("Длина пути: ");

            ShowLineCreationStatus("Путь успешно создан");

            return new Line(startingPoint, endingPoint, lineLength);
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
            Random random = new Random();
            int[] capacityVariants = new int[] { 30, 40, 50, 60 };
            int capacity = capacityVariants[random.Next(0, capacityVariants.Length)];
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

            ShowLinesInfo();

            Console.SetCursorPosition(0, 20);
        }

        private void ShowLinesInfo()
        {
            foreach(var line in _lines)
            {
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
            for(int i = 0; i < _lines.Count; i++)
            {
                _lines[i].SkipHour();
            }
        }
    }

    class Line
    {
        private int _length;
        private string _startingPoint;
        private string _endingPoint;
        private Train _train;

        public Line(string startingPoint, string endingPoint, int length)
        {
            _train = null;
            _length = length;
            _startingPoint = startingPoint;
            _endingPoint = endingPoint;
        }

        public void Addtrain(Train train)
        {
            _train = train;
        }

        public void ShowInfo()
        {
            if( _train != null)
            {
                if (_train.PassedDistance < _length)
                    Console.WriteLine($"{_startingPoint} - {_endingPoint} ({_length}км):\nКоличесво вагонов: {_train.ShowWagonsCount()} (вместимость: {_train.ShowWagonsCapacity()}), количество пассажиров: {_train.TicketsCount}\nВ пути: поезд имеет скорость: {_train.Speed}км/ч, пройденное расстояние: {_train.PassedDistance}");
                else
                    Console.WriteLine($"{_startingPoint} - {_endingPoint} ({_length}км): прибыл");
            }
            else
            {
                Console.WriteLine($"Линия ({_startingPoint} - {_endingPoint}) в процессе создания");
            }
        }

        public void SkipHour()
        {
            if(_train.PassedDistance < _length)
                _train.Move();
        }
    }

    class Train
    {
        private List<Wagon> _wagons;
        public int Speed { get; private set; }  
        public int PassedDistance { get; private set; }
        public int TicketsCount { get; private set; }        

        public Train(int ticketsCount)
        {
            Random random = new Random();
            int minSpeed = 100;
            int maxSpeed = 200;
            Speed = random.Next(minSpeed, maxSpeed + 1);
            TicketsCount = ticketsCount;
            PassedDistance = 0;
            _wagons = new List<Wagon>();
        }

        public void AddWagon(int capacity, int passengersCount)
        {
            _wagons.Add(new Wagon(capacity, passengersCount));
        }

        public void Move()
        {
            PassedDistance += Speed;
        }

        public int ShowWagonsCount()
        {
            return _wagons.Count;
        }

        public int ShowWagonsCapacity()
        {
            return _wagons[0].Сapacity;
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
