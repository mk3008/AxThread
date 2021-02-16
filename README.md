# AxThread
AxThreadは並列処理の実装を簡易にするためのバッチ向けライブラリです。
処理をグループ管理するため直感的に実装、保守が行なえます。

# Example
ここにあげたサンプルコードの完全版は「sample」フォルダの中にある「BreakfastSample.csproj」を参照してください。

Microsfotのドキュメントにある[async および await を使用した非同期プログラミング](https://docs.microsoft.com/ja-jp/dotnet/csharp/programming-guide/concepts/async/)のお題が非常にわかりやすいので、これを例題とします。

実行したい処理は

1. コーヒーを注ぐ
1. 卵、ベーコン、トーストを並行して焼く
1. ジュースを注ぐ

とします。

このとき、async/awaitで実装したコードを抜粋すると以下のとおりです。
```
        static async Task Main(string[] args)
        {
            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(3);
            var toastTask = MakeToastWithButterAndJamAsync(2);

            var breakfastTasks = new List<Task> { eggsTask, baconTask, toastTask };
            while (breakfastTasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(breakfastTasks);
                breakfastTasks.Remove(finishedTask);
            }

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            Console.WriteLine("Breakfast is ready!");
        }
```

このコードでも特に問題はありませんが、直列処理（1、3）と並列処理（2）の呼び出し方法が違っており、「1」の処理を並列化しても良いとなった場合はメインルーチン側もそれなりに修正が必要にあります。

そこでAxThreadでは直列、並列処理別け隔てなく管理することで上記の保守性を上げることができます。
詳細はおいておくとして、AxThreadで実装した場合のメインルーチンはこのようになります。

```
        private static async Task Main(string[] args)
        {
            var proc = BuildProcess();
            await proc.ExecuteAsync();
        }
        
        private static IAxProcess BuildProcess()
        {
            var root = new AxProcessContainer();
            var burnProcs = new AxProcessContainer();

            burnProcs.IsParallel = true;

            var coffee = new CoffeeMaker();
            var egg = new EggsFryer() { HowMany = 2 };
            var bacon = new BaconFryer() { Slices = 1 };
            var toast = new Toaster() { Slices = 2 };
            var juice = new Juicer();

            burnProcs.Regist(bacon.ToAxProcess());
            burnProcs.Regist(egg.ToAxProcess());
            burnProcs.Regist(toast.ToAxProcess());

            root.Regist(coffee.ToAxProcess());
            root.Regist(burnProcs);
            root.Regist(juice.ToAxProcess());

            return root;
        }
        
    internal class CoffeeMaker : IProcess
    {
        public void Execute()
        {
            Console.WriteLine("Pouring coffee");
        }
    }
```

AxThreadは

- 各処理をプロセスとして定義し
- プロセスコンテナという入れ物にプロセス、またはプロセスコンテナを入れ
- プロセスコンテナ単位で実行

という流れで実装します。

上記のコードでいうと
メインルーチン用のプロセスコンテナ`root`を定義し、そこに

1. コーヒーを注ぐ
1. 卵、ベーコン、トーストを並行して焼く
1. ジュースを注ぐ

というプロセスを登録しています。
```
            root.Regist(coffee.ToAxProcess());
            root.Regist(burnProcs);
            root.Regist(juice.ToAxProcess());
```

「2」については、`burnProcs`というプロセスコンテナを定義し、そこに

1. 卵を焼く
1. ベーコンを焼く
1. トーストを焼く

それぞれのプロセスを登録しています。
```
            burnProcs.Regist(bacon.ToAxProcess());
            burnProcs.Regist(egg.ToAxProcess());
            burnProcs.Regist(toast.ToAxProcess());
```
`burnProcs`は並列して作業をしても良いとなっているため、`burnProcs.IsParallel = true;`と記述します。このようにすることで、burnProcsに登録されたプロセスは並列で処理されるようになります。

プロセスを直列グループ、並列グループで管理できるようになるため、あとからの変更は用意になります。

# 拡張メソッドToAxProcess() について
```
    internal class Program
    {
    ・・・
            root.Regist(coffee.ToAxProcess());
    }

    internal class CoffeeMaker : IProcess
    {
        public void Execute()
        {
            Console.WriteLine("Pouring coffee");
        }
    }
    
    public interface IProcess
    {
        void Execute();
    }
    
    public interface IAsyncProcess
    {
        public Task ExecuteAsync();
    }
```
プロセスコンテナに登録できるのはもしくは'IAxProcess'インターフェイスを実装したクラスに限られます。インターフェイスを実装するのはそれなりに面倒ですので変換用の拡張メソッドを用意してあります。
`IProcess`インターフェイス、または`IAsyncProcess`インターフェイスという非常にシンプルなインターフェイスを実装したクラスであれば拡張メソッド'ToAxProcess()'にて'IAxProcess'インターフェイスに変換が可能です。

void methodや、Task method もプロセスコンテナに登録できます。

```
        private static IAxProcess BuildProcess()
        {
            var root = new AxProcessContainer();
            var burnProcs = new AxProcessContainer();

            burnProcs.IsParallel = true;
            
            //regist void method
            root.Regist(new Action(PourCoffee).ToAxProcess());

            //regist Task method
            root.Regist(new Func<Task>(PourJuiceAsync).ToAxProcess());

            return root;
        }

        private static void PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
        }

        private static async Task PourJuiceAsync()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Pouring juice");
            });
        }
```


