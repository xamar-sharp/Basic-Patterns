using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System.Threading;
using static System.Console;
namespace Patterns
{
    class Program
    {
        static void Main(string[] args)
        {
            IModelPrinter printer = new ConsoleModelPrinter();
            IModelHandler handler = new ConsoleModelHandler();
            IModelBuilder builder = new ConsoleModelBuilder();
            IModelFacade facade = new ModelFacade(printer, handler, builder);
            facade.Collect(new Image());
        }
    }
    #region Порождающие паттерны проектирования
    /// <summary>
    /// Model which building in Fluent Builder
    /// </summary>
    public class Melody
    {
        /// <summary>
        /// Temp of melody
        /// </summary>
        /// <exception cref="OverflowException"/>
        /// <returns>Temp of melody</returns>
        public double Temp { get; internal set; }
        public int Volume { get; internal set; }
        public string Name { get; internal set; }
    }
    /// <summary>
    /// Builder in Fluent Builder
    /// </summary>
    public class Instrument
    {
        /// <summary>
        /// Building object
        /// </summary>
        private Melody Melody { get; }
        public Instrument()
        {
            Melody = new Melody();
        }
        /// <summary>
        /// Build "Temp" property in building object
        /// </summary>
        /// <param name="temp">Setting modificator</param>
        /// <returns>This instance of builder</returns>
        public Instrument SetTemp(double temp)
        {
            Melody.Temp = temp;
            return this;
        }
        /// <summary>
        /// Build "Volume" property in building object
        /// </summary>
        /// <param name="volume">Setting modificator</param>
        /// <returns>This instance of builder</returns>
        public Instrument SetVolume(int volume)
        {
            Melody.Volume = volume;
            return this;
        }
        /// <summary>
        /// Build "Name" property in building object
        /// </summary>
        /// <param name="name">Setting modificator</param>
        /// <returns>This instance of builder</returns>
        public Instrument SetName(string name)
        {
            Melody.Name = name;
            return this;
        }
        /// <summary>
        /// Build an target object
        /// </summary>
        /// <returns>Builded object</returns>
        public Melody Build()
        {
            return Melody;
        }
    }
    public interface IClone<out T> where T : IClone<T>
    {
        T Clone();
    }
    public interface ISimpleEntity
    {
        public string Name { get; }
    }
    public interface ICloneableEntity : ISimpleEntity, IClone<ICloneableEntity>
    {

    }
    public struct Dog : ICloneableEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        /// <summary>
        /// Initializes dog
        /// </summary>
        /// <param name="name">name of dog</param>
        /// <param name="age">age of dog</param>
        public Dog(string name, int age)
        {
            Name = name;
            Age = age;
        }
        public ICloneableEntity Clone()
        {
            return new Dog(Name, Age);
        }
    }
    public class FinalGameBoss
    {
        private static FinalGameBoss _singletone { get; }
        public double Life { get; private set; }
        public double Damage { get; private set; }
        public string Name { get; private set; }
        static FinalGameBoss()
        {
            _singletone = new FinalGameBoss();
        }
        public void SetLife(double life)
        {
            Life = life;
        }
        public void SetDamage(double damage)
        {
            Damage = damage;
        }
        public void SetName(string name)
        {
            Name = name;
        }
        public static FinalGameBoss GetInstance()
        {
            return _singletone;
        }
    }
    public sealed class FinalBuilder
    {
        protected FinalGameBoss Boss { get; private set; }
        public FinalBuilder()
        {
            Boss = FinalGameBoss.GetInstance();
        }
        public void Set(double life, double damage, string name)
        {
            Boss.SetDamage(damage);
            Boss.SetLife(life);
            Boss.SetName(name);
        }
        public FinalGameBoss Build()
        {
            return Boss;
        }
    }
    public interface IAbstractFactory
    {
        BuildedItem Create(int value);
    }
    public class SwordFactory : IAbstractFactory
    {
        public BuildedItem Create(int value)
        {
            return new Sword(value);
        }
    }
    public class ShieldFactory : IAbstractFactory
    {
        public BuildedItem Create(int value)
        {
            return new Shield(value);
        }
    }
    public abstract class BuildedItem
    {
        public abstract int Value { get; protected set; }
        public abstract void Browse();
        public BuildedItem(int value)
        {
            Value = value;

        }

    }
    public class Sword : BuildedItem
    {
        public override int Value { get; protected set; }
        public override void Browse()
        {
            WriteLine(string.Format("Attack: {0}", Value));
        }
        public Sword(int value) : base(value)
        {

        }
    }
    public class Shield : BuildedItem
    {
        public override int Value { get; protected set; }
        public override void Browse()
        {
            WriteLine($"Defense: {Value}");
        }
        public Shield(int value) : base(value)
        {

        }
    }
    public class Road
    {
        int _height;
        public int Height
        {
            get => _height;
            set => _height = value;
        }
        int _width;
        public int Width
        {
            get => _width;
            set => _width = value;
        }
        public void SetHeight(int height)
        {
            Height = height;

        }
        public void SetWidth(int width)
        {
            Width = Width;
        }
    }
    public class RoadBuilder
    {
        public Road Road { get; set; } = new Road();
        bool isBuilded = false;
        public void Set(int height, int width)
        {
            Road.SetHeight(height);
            Road.SetWidth(width);
            isBuilded = true;
        }
        public Road Build()
        {
            if (isBuilded)
                return Road;
            else
                return null;
        }
    }
    public static class Constants
    {
        public static readonly string LocalDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static Constants()
        {

        }
    }
    public interface IEntity
    {
        Guid Id { get; }
        string Discriminator { get; }
    }
    public interface IRepository<out T> where T : IEnumerable<IEntity>
    {
        T Data { get; }
        void AddEntity(IEntity entity);
        void RemoveEntity(Guid id);
        void UpdateEntity(IEntity entity);
        IEntity GetEntity(Guid guid);
    }
    public class RepositoryFactory
    {
        public Type RepositoryType { get; set; }
        public RepositoryFactory(Type repositoryType)
        {
            if (repositoryType == typeof(IEntity))
            {
                RepositoryType = repositoryType;
            }
            else
            {
                RepositoryType = typeof(SimpleRepository);
            }
        }
        public object CreateRepository()
        {
            if (RepositoryType.GetConstructors().FirstOrDefault(ctor => ctor.GetParameters().Length == 0) is not null)
            {
                return (Activator.CreateInstance(RepositoryType) as IRepository<IEnumerable<IEntity>>);
            }
            else
            {
                return new SimpleRepository(new EntityValidator());
            }
        }
    }
    public interface IEntityValidator<T> where T : IEntity
    {
        bool Validate(T entity);
    }
    public class VideoValidator : IEntityValidator<Video>
    {
        public bool Validate(Video video)
        {
            return video.Data is not null;
        }
    }
    public class EntityValidator : IEntityValidator<IEntity>
    {
        public bool Validate(IEntity entity)
        {
            return entity.Discriminator is not null && entity.Id != Guid.Empty;
        }
    }
    public class Video : IEntity
    {
        public Guid Id { get; }
        public string Discriminator { get => this.GetType().FullName; }
        public IEntityValidator<Video> Validator { get; set; }
        public byte[] Data { get; set; }
        public async Task<bool> TrySaveVideo(string fileName = null)
        {
            if (Validator.Validate(this))
            {
                if (fileName is null)
                {
                    fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".mp4";
                }
                else
                {
                    fileName = Path.GetFileNameWithoutExtension(fileName) + ".mp4";
                }
                await using (FileStream stream = File.Create(Path.Combine(Constants.LocalDataFolder, fileName)))
                {
                    if (stream.CanTimeout)
                        stream.WriteTimeout = 10;
                    if (stream.CanWrite)
                        await stream.WriteAsync(Data);
                    else
                        return false;
                }
                return true;
            }
            return false;
        }
    }
    public class EntityComparer : IEqualityComparer<IEntity>
    {
        public bool Equals(IEntity entity1, IEntity entity2)
        {
            return entity1.Id.Equals(entity2.Id);
        }
        public int GetHashCode(IEntity entity)
        {
            return entity.GetHashCode();
        }
    }
    public class SimpleRepository : IRepository<HashSet<IEntity>>
    {
        public IEntityValidator<IEntity> Validator { get; set; }
        public HashSet<IEntity> Data { get; }
        public SimpleRepository(IEntityValidator<IEntity> validator)
        {
            Data = new HashSet<IEntity>(10, new EntityComparer());
            Validator = validator;
        }
        public SimpleRepository()
        {
            Data = new HashSet<IEntity>(10, new EntityComparer());
            Validator = new EntityValidator();
        }
        public void AddEntity(IEntity entity)
        {
            if (Validator.Validate(entity))
            {
                Data.Add(entity);
            }
        }
        public void RemoveEntity(Guid id)
        {
            Data.RemoveWhere((entity) => entity.Id == id);
        }
        public void UpdateEntity(IEntity entity)
        {
            IEntity oldEntity = Data.FirstOrDefault(comparingEntity => comparingEntity.Id == entity.Id);
            if (oldEntity.Discriminator == entity.Discriminator)
            {
                oldEntity = entity;
            }
        }
        public IEntity GetEntity(Guid guid)
        {
            return Data.FirstOrDefault(comparingEntity => comparingEntity.Id == guid);
        }
    }
    #endregion
    #region Поведенческие паттерны проектирования
    public interface IVisitable
    {
        void Visit(IVisitor visitor);
    }
    public interface IVisitor
    {
        void VisitForest(Forest forest);
        void VisitPark(Park park);
        void VisitShop(Shop shop);
    }
    public class Forest : IVisitable
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public void Visit(IVisitor visitor)
        {
            visitor.VisitForest(this);
        }
    }
    public class Park : IVisitable
    {
        public double Normality { get; set; }
        public void Visit(IVisitor visitor)
        {
            visitor.VisitPark(this);
        }
    }
    public class Shop : IVisitable
    {
        public bool IsHighPrices { get; set; }
        public void Visit(IVisitor visitor)
        {
            visitor.VisitShop(this);
        }
    }
    public class SimplePeople : IVisitor
    {
        public void VisitForest(Forest forest)
        {
            WriteLine($"I am in {forest.Name}.Forest in {forest.Color} color!");
        }
        public void VisitShop(Shop shop)
        {
            WriteLine($"Shop has {(shop.IsHighPrices ? "bad" : "best")} prices!");
        }
        public void VisitPark(Park park)
        {
            WriteLine($"Park is in {park.Normality * 100}% normality!!!");
        }
    }
    public struct Node
    {
        public IEnumerable<Node> Nodes { get; set; }
        public int Weight { get; set; }
        public static Node Empty { get => new Node(null, -1); }
        public Node(IEnumerable<Node> nodes, int weight)
        {
            if (nodes != null)
                Nodes = nodes;
            else
                Nodes = new List<Node>(5);
            Weight = weight;
        }
        public Node? SearchNode(int weight)
        {
            if (Weight == weight)
            {
                return this;
            }
            else
            {
                foreach (var node in Nodes)
                {
                    return node.SearchNode(weight);
                }
                return Node.Empty;
            }
        }
    }
    public class Hero
    {
        public double Gold { get; set; }
        public int Life { get; set; }
        public short Level { get; set; }
        public Hero()
        {
            Gold = 0;
            Level = 1;
            Life = 100;
        }
        public void SaveState(SavePoint point)
        {
            point.State.Gold = Gold;
            point.State.Level = Level;
            point.State.Life = Life;
        }
        public void LoadState(SavePoint point)
        {
            Gold = point.State.Gold;
            Level = point.State.Level;
            Life = point.State.Life;
        }
    }
    public struct SavePoint
    {
        public Hero State { get; internal set; }
        public SavePoint(double gold, int life, short level)
        {
            State = new Hero() { Gold = gold, Life = life, Level = level };
        }
    }
    public interface ICommand
    {
        Task ExecuteAsync(object param);
        void Cancel();
    }
    public interface IExecuter
    {
        Task InvokeAsync(ICommand command, object param);
        void Stop(ICommand command);
    }
    public struct SumCommand : ICommand
    {
        public CancellationTokenSource Source { get; }
        public async Task ExecuteAsync(object param)
        {
            await Task.Factory.StartNew((state) =>
            {
                if (state is not UInt32)
                {
                    throw new ArgumentException();
                }
                int counter = 0;
                for (int x = 0; x < (UInt32)state; x++)
                {
                    counter++;
                }
                WriteLine(counter);
            }, param, Source.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
        }
        public SumCommand(CancellationTokenSource source)
        {
            Source = source;
        }
        public void Cancel()
        {
            Source.Cancel();
        }
    }
    public class CommandExecuter : IExecuter
    {
        private volatile int _counter = 0;
        public async Task InvokeAsync(ICommand command, object param)
        {
            await command.ExecuteAsync(param);
            Interlocked.Increment(ref _counter);
            WriteLine($"{_counter} command was completed!");
        }
        public void Stop(ICommand command)
        {
            command.Cancel();
            WriteLine("Command was canceled!");
        }
    }
    public interface IState
    {
        string Name { get; }
        void Change(IPeople people);
    }
    public interface IPeople
    {
        IState State { get; set; }
        void Change();
    }
    public struct GoodState : IState
    {
        public string Name { get => "i feel good:-)"; }
        public void Change(IPeople people)
        {
            people.State = new SadState();
        }
    }
    public struct SadState : IState
    {
        public string Name { get => "i feel sad:-("; }
        public void Change(IPeople people)
        {
            people.State = new GoodState();
        }
    }
    public struct Person : IPeople
    {
        public IState State { get; set; }
        public Person(IState state)
        {
            State = state;
        }
        public void Change()
        {
            State.Change(this);
        }
    }
    public interface IEnumerator<out T>
    {
        T[] Items { get; }
        int Position { get; }
        T Current { get; }
        bool MoveNext { get; }
        void Reset();
    }
    public struct SafeEnumerator<T> : IEnumerator<T>
    {
        public T[] Items { get; }
        public int Position { get; private set; }
        public T Current { get => Items[Position]; }
        public bool MoveNext { get => Position++ < Items.Length - 1; }
        public void Reset()
        {
            Position = -1;
        }
        public SafeEnumerator(IEnumerable<T> items)
        {
            Items = items.ToArray();
            Position = -1;
        }
    }
    public interface ILexicalWord
    {
        object Interprete(ILexicalContext ctx);
    }
    public interface ICompositeLexicalWord : ILexicalWord
    {
        ILexicalWord FirstWord { get; }
        ILexicalWord SecondWord { get; }
        object InterpreteComposite(ILexicalContext ctx);
    }
    public interface ISimpleLexicalWord : ILexicalWord
    {
        object InterpreteSimple(ILexicalContext ctx);
    }
    public interface ILexicalContext
    {
        Dictionary<string, object> TempData { get; }
        bool AddData(string key, object value);
        bool RemoveData(string key);
        object this[string key] { get; }
    }
    public class DoubleAddWord : ICompositeLexicalWord
    {
        public ILexicalWord FirstWord { get; }
        public ILexicalWord SecondWord { get; }
        public DoubleAddWord(ILexicalWord firstWord, ILexicalWord secondWord)
        {
            FirstWord = firstWord;
            SecondWord = secondWord;
        }
        public object Interprete(ILexicalContext ctx)
        {
            return InterpreteComposite(ctx);
        }
        public object InterpreteComposite(ILexicalContext ctx)
        {
            var firstValue = FirstWord.Interprete(ctx);
            var secondValue = SecondWord.Interprete(ctx);
            if (firstValue is not Int32 || secondValue is not Int32)
            {
                throw new HellSharpException("variables was not Int32");
            }
            return (int)firstValue + (int)secondValue;
        }
    }
    public class AddWord : ISimpleLexicalWord
    {
        public object Interprete(ILexicalContext ctx)
        {
            return InterpreteSimple(ctx);
        }
        public object InterpreteSimple(ILexicalContext ctx)
        {
            if (ctx.TempData.Keys.Count > 0)
            {
                string firstVariable = ctx.TempData.Keys.LastOrDefault();
                object firstValue = ctx.TempData[firstVariable];
                if (!ctx.RemoveData(firstVariable))
                {
                    throw new HellSharpException("variable was not removed!");
                }
                string secondVariable = ctx.TempData.Keys.LastOrDefault();
                object secondValue = ctx.TempData[secondVariable];
                if (!ctx.RemoveData(secondVariable))
                {
                    throw new HellSharpException("variable was not removed!");
                }
                if (firstValue is not Int32 || secondValue is not Int32)
                {
                    throw new HellSharpException("variable was not int in add operation!");
                }
                return ((Int32)firstValue) + ((Int32)secondValue);
            }
            else
            {
                throw new HellSharpException("variables was not defined!");
            }
        }
    }
    public class HellSharpException : Exception
    {
        public HellSharpException(string message) : base("H# program throwned an exception: " + message)
        {

        }
    }
    public class HellSharpContext : ILexicalContext
    {
        public Dictionary<string, object> TempData { get; internal set; }
        public HellSharpContext()
        {
            TempData = new Dictionary<string, object>(20);
        }
        public bool AddData(string key, object value)
        {
            return TempData.TryAdd(key, value);
        }
        public bool RemoveData(string key)
        {
            return TempData.Remove(key);
        }
        public object this[string key]
        {
            get
            {
                return TempData[key];
            }
            internal set
            {
                TempData[key] = value;
            }
        }
    }
    public interface IMailMediator
    {
        IEnumerable<IUser> Users { get; }
        void SendMail(string userName, string message);
        void SetUsers(IEnumerable<IUser> users);
    }
    public interface IUser
    {
        string Name { get; }
        IMailMediator Mediator { get; }
        void SendMail(string userName, string message);
        void ReceiveMail(string mail);
    }
    public class EmailUser : IUser
    {
        public string Name { get; protected set; }
        public IMailMediator Mediator { get; }
        public EmailUser(IMailMediator mediator, string name)
        {
            Mediator = mediator;
            Name = name;
        }
        public void SendMail(string userName, string message)
        {
            Mediator.SendMail(userName, message);
        }
        public void ReceiveMail(string message)
        {
            WriteLine($"{Name} получил сообщение {message}!");
        }
    }
    public static class Datas
    {
        public static IEnumerable<string> BadWords { get; }
        static Datas()
        {
            BadWords = new HashSet<string>() { "Fuck", "Blyat", "Dick", "Porn", "War" };
        }
    }
    public class EmailMediator : IMailMediator
    {
        public IEnumerable<IUser> Users { get; private set; }
        public EmailMediator(IEnumerable<IUser> users = null)
        {
            if (users is not null)
            {
                Users = users;
            }
        }
        public void SetUsers(IEnumerable<IUser> users)
        {
            Users = users;
        }
        public void SendMail(string userName, string message)
        {
            foreach (var badWord in Datas.BadWords)
            {
                if (message.ToLower().Contains(badWord.ToLower()))
                    return;
            }
            Users.FirstOrDefault(user => user.Name == userName).ReceiveMail(message);
        }
    }
    public abstract class RandomObject
    {
        public abstract double Height { get; protected set; }
        public abstract double Width { get; protected set; }
        public abstract double Length { get; protected set; }
        public RandomObject()
        {
            Height = GenerateHeight();
            Width = GenerateWidth();
            Length = GenerateLength();
        }
        public abstract double GenerateHeight();
        public abstract double GenerateWidth();
        public abstract double GenerateLength();
    }
    public class Box : RandomObject
    {
        public override double Height { get; protected set; }
        public override double Width { get; protected set; }
        public override double Length { get; protected set; }
        public static Random Random { get; }
        static Box()
        {
            Random = new Random();
        }
        public Box() : base()
        {

        }
        public override double GenerateHeight()
        {
            return Random.NextDouble();
        }
        public override double GenerateWidth()
        {
            return Random.NextDouble();
        }
        public override double GenerateLength()
        {
            return Random.NextDouble();
        }
    }
    public interface IObserver
    {
        IEnumerable<IObservable> Observables { get; }
        void Notify(IObservable observable);
    }
    public interface IObservable
    {
        int Id { get; }
        IEnumerable<IObserver> Observers { get; }
        void PropertyChanged();
    }
    public class Music : IObservable
    {
        public IEnumerable<IObserver> Observers { get; }
        private string _name;
        private int _temp;
        private double _volume;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                PropertyChanged();
            }
        }
        public int Temp
        {
            get
            {
                return _temp;
            }
            set
            {
                _temp = value;
                PropertyChanged();
            }
        }
        public double Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                PropertyChanged();
            }
        }
        public int Id { get; }
        public Music(IEnumerable<IObserver> observers, string name, int temp, double volume)
        {
            Observers = observers;
            Name = name;
            Temp = temp;
            Volume = volume;
            Id = new Random().Next(int.MinValue, int.MaxValue);
        }
        public void PropertyChanged()
        {
            foreach (IObserver observer in Observers)
            {
                observer.Notify(this);
            }
        }
    }
    public class People : IObserver
    {
        public IEnumerable<IObservable> Observables { get; }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public People(IEnumerable<IObservable> observables, string name)
        {
            Name = name;
            Observables = observables;
        }
        public void Notify(IObservable observable)
        {
            WriteLine(Name + ":" + observable.Id + " was changed!!!!");
        }
    }
    public interface IStrategy<in T>
    {
        object MadeAction(T instance);
    }
    public interface IPrinter
    {
        void HandleValue(object obj);
    }
    public class FormatNameStrategy : IStrategy<Robot>
    {
        public object MadeAction(Robot robot)
        {
            return robot.Name.Trim().ToLower().Split('.')[0];
        }
    }
    public class ConsolePrinter : IPrinter
    {
        public void HandleValue(object obj)
        {
            WriteLine(JsonSerializer.Serialize(obj));
        }
    }
    public class Robot
    {
        public string Name { get; protected set; }
        public IPrinter Printer { get; }
        public Robot(string name, IPrinter printer)
        {
            Printer = printer;
            Name = name;
        }
        public void ExecuteStrategy(IStrategy<Robot> strategy)
        {
            var value = strategy.MadeAction(this);
            if (value is not null)
            {
                Printer.HandleValue(value);
            }
        }
    }
    #endregion
    #region Структурные паттерны проектирования
    public interface IModelPrinter
    {
        void Print(IModel model);
    }
    public interface IModelBuilder
    {
        void Build(IModel model);
    }
    public interface IModelHandler
    {
        void Handle(IModel model);
    }
    public interface IModel
    {
        string Name { get; set; }
        string Information { get; set; }
        byte[] Data { get; set; }
    }
    public interface IModelFacade
    {
        IModelPrinter Printer { get; }
        IModelHandler Handler { get; }
        IModelBuilder Builder { get; }
        void Collect(IModel model);
    }
    public class Image : IModel
    {
        public string Name { get; set; }
        public string Information { get; set; }
        public byte[] Data { get; set; }
    }
    public class ConsoleModelPrinter : IModelPrinter
    {
        public void Print(IModel model)
        {
            WriteLine(JsonSerializer.Serialize(model));
        }
    }
    public class ConsoleModelBuilder : IModelBuilder
    {
        public void Build(IModel model)
        {
            model.Name = "<default>";
            model.Information = "none";
            model.Data = new byte[256];
            new Random().NextBytes(model.Data);
        }
    }
    public class ConsoleModelHandler : IModelHandler
    {
        public void Handle(IModel model)
        {
            WriteLine($"Model has {model.Data.Length} bytes of data");
        }
    }
    public class ModelFacade : IModelFacade
    {
        public IModelPrinter Printer { get; }
        public IModelHandler Handler { get; }
        public IModelBuilder Builder { get; }
        public ModelFacade(IModelPrinter printer, IModelHandler handler, IModelBuilder builder)
        {
            Printer = printer;
            Handler = handler;
            Builder = builder;
        }
        public void Collect(IModel model)
        {
            Builder.Build(model);
            Handler.Handle(model);
            Printer.Print(model);
        }
    }

    public class Square
    {
        public double Height { get; }
        public double Width { get; }
        public string Color { get; private set; }
        public Square()
        {
            Height = 35;
            Width = 100;
            Color = "red";
        }
        public void ColorizeHome(string color)
        {
            Color = color;
        }
    }
    public class CompositorNode
    {
        public IEnumerable<CompositorNode> Children { get; }
        public CompositorNode(IEnumerable<CompositorNode> childs)
        {
            Children = childs;
        }
        public void AddChild(CompositorNode node)
        {

            Children.Append(node);
        }
        public void RemoveChild(CompositorNode node)
        {
            Children.ToList().Remove(node);
        }
    }
    public interface IProxy
    {
        IDbConnection Connection { get; }
        IEnumerable<string> Connect();
        void Add(string str);
    }
    public interface IDbConnection
    {
        int Timeout { get; }
        IEnumerable<string> Data { get; }
        void AddData(string str);
    }
    public class MssqlProxy : IProxy
    {
        IEnumerable<string> LocalData { get; }
        public IDbConnection Connection { get; }
        public IEnumerable<string> Connect()
        {
            if (Connection.Timeout > 50)
            {
                return LocalData;
            }
            else
            {
                return Connection.Data;
            }
        }
        public void Add(string str)
        {
            if (Connection.Timeout > 100)
            {
                LocalData.Append(str);
            }
            else
            {
                Connection.AddData(str);
            }
        }
        public MssqlProxy(IEnumerable<string> localData, IDbConnection connection)
        {
            LocalData = localData;
            Connection = connection;
        }
    }
    public class MssqlConnection : IDbConnection
    {
        public IEnumerable<string> Data { get; }
        public int Timeout { get => 100; }
        public void AddData(string str)
        {
            Data.Append(str);
        }
        public MssqlConnection(IEnumerable<string> data)
        {
            Data = data;
        }
    }
    public interface IAbstraction
    {
        IRealization Realization { get; }
        void InvokeAbstract();
    }
    public interface IRealization
    {
        void AbstractMethod();
    }
    public class Abstraction : IAbstraction
    {
        public IRealization Realization { get; }
        public Abstraction(IRealization realization)
        {
            Realization = realization;
        }
        public void InvokeAbstract()
        {
            Realization.AbstractMethod();
        }
    }
    public class Realization : IRealization
    {
        public void AbstractMethod()
        {
            WriteLine("Concrete realization!!!");
        }
    }
    public abstract class PizzaDecorator
    {
        public EmptyPizza Pizza { get; }
        public PizzaDecorator(EmptyPizza pizza)
        {
            Pizza = pizza;
        }
        public virtual int GetCalories()
        {
            return 100;
        }
    }
    public class EmptyPizza
    {
        public int Calories { get => 100; }
        public string Name { get => "Simple pizza"; }
        public void Eat()
        {
            WriteLine("So small portion....");
        }
    }
    public class EmptyPizzaDecorator : PizzaDecorator
    {
        public EmptyPizzaDecorator(EmptyPizza pizza) : base(pizza)
        {

        }
        public sealed override int GetCalories()
        {
            return base.GetCalories();
        }
    }

    public class CheesePizzaDecorator : PizzaDecorator
    {
        public CheesePizzaDecorator(EmptyPizza pizza) : base(pizza)
        {

        }
        public override int GetCalories()
        {
            return base.GetCalories() + 50;
        }
    }
    public interface ILiveObject
    {
        void Walk();
    }
    public class Cat : ILiveObject
    {
        public void Walk()
        {
            WriteLine("Shork...shork...");
        }
    }
    public class StoneAdapter : ILiveObject
    {
        public Stone Stone { get; }
        public StoneAdapter(Stone stone)
        {
            Stone = stone;
        }
        public void Walk()
        {
            Stone.Down();
        }
    }
    public class Stone
    {
        public void Down()
        {
            WriteLine("Stone down into water...");
        }
    }
    #endregion
}

