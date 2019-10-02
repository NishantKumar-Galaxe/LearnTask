using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearnTask
{
    class Program
    {
        static void Main(string[] args)
        {
            //Program-1

            //Here we are creating a new task and start it at same time.
            //Task.Factory.StartNew(() => write('.'));

            //Here we are creating a task and then have to start it explicitly.
            //var t = new Task(() => write('?'));
            //t.Start();

            //write('-');


            //Program-2

            //string text1 = "Nishant", text2 = "kumar";

            //Task<int> t1 = new Task<int>(TextLength, text1);
            //t1.Start();

            //Task<int> t2 = Task.Factory.StartNew(TextLength, text2);

            //Console.WriteLine($"Length of {text1} is {t1.Result}");
            //Console.WriteLine($"Length of {text2} is {t2.Result}");

            //Program-3

            //Cancellation Token
            //var cts = new CancellationTokenSource();
            //var token = cts.Token;

            ////Raise events in case token is been cancelled.
            //token.Register(() =>
            //{
            //    Console.WriteLine("Cancellation has been requested");
            //});

            //var t = new Task(() => writeForever(token));
            //t.Start();

            //Console.ReadKey();
            //cts.Cancel();

            //Program-4

            //Link multiple cancellation token

            //var planned = new CancellationTokenSource();
            //var preventive = new CancellationTokenSource();
            //var emergency = new CancellationTokenSource();

            ////Create a common cancell token, if any one of three token is cancelled task get cancelled.
            //var paranoid = CancellationTokenSource.CreateLinkedTokenSource(planned.Token, preventive.Token, emergency.Token);

            //var t = new Task(() => writeForever(paranoid.Token));
            //t.Start();

            //Console.ReadKey();
            //preventive.Cancel();

            //Program-5
            //Wait handling for a task

            //var cts = new CancellationTokenSource();
            //var token = cts.Token;

            //var t = new Task(() =>
            //{
            //    Console.WriteLine("Press any key within 5 seconds....");
            //    var cancelled = token.WaitHandle.WaitOne(5000);
            //    Console.WriteLine(cancelled ? "WellDone" : "you missed...");
            //}, token);

            //t.Start();

            //Console.ReadKey();
            ////cts.Cancel();
            //Console.WriteLine($" current state is::  {cts.IsCancellationRequested.ToString()}");


            //Program-6
            //Block a task until some work get finished

            //var cts = new CancellationTokenSource();
            //var token = cts.Token;

            //var t1 = new Task(() =>
            //{
            //    Console.WriteLine("I take 5 seconds");
            //    for (int i = 0; i < 5; i++)
            //    {
            //        token.ThrowIfCancellationRequested();
            //        Thread.Sleep(1000);
            //    }
            //}, token);
            //t1.Start();

            //Task t2 = Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(3000);
            //}, token);

            ////Wait for all task to complete
            ////Task.WaitAll(t1, t2);

            ////wait to complete any one, you can pass cancellation token as well, in that case need to pass task as an array.
            //Task.WaitAny(new[] { t1, t2 }, token);

            ////Timeout argument can also be passed to complete the task.
            ////Task.WaitAny(new[] { t1, t2 }, 2000, token);

            //Console.WriteLine($"Task t1 status is {t1.Status}");
            //Console.WriteLine($"Task t2 status is {t2.Status}");


            //Program-7

            //try
            //{
            //    testException();
            //}
            //catch (AggregateException ex)
            //{
            //    //Handle non handled/propagated exception
            //    foreach (var e in ex.InnerExceptions)
            //    {
            //        Console.WriteLine($"Exception {e.GetType()} from {e.Source}");
            //    }
            //}

            //Console.ReadKey();


            //Program-8
            DataSharingAndSynchronization obj = new DataSharingAndSynchronization();
            Console.WriteLine($"Balance is ::   {obj.GetBalance()}");


            Console.WriteLine("Main program done");
            Console.ReadKey();
        }



        public static void write(char c)
        {
            int i = 1000;
            while (i-- > 0)
            {
                Console.Write(c);
            }
        }

        public static int TextLength(object obj)
        {
            Console.WriteLine($"\n Task with ID {Task.CurrentId} processing object {obj}...");
            return obj.ToString().Length;
        }

        public static void writeForever(CancellationToken token)
        {
            int count = 0;
            while (true)
            {
                //Implementation-1
                //if (token.IsCancellationRequested)
                //{
                //    throw new OperationCanceledException();
                //}

                //Or

                //Implementation-2, recommended way
                token.ThrowIfCancellationRequested();

                // break;
                //else


                Console.WriteLine($"{count++} \t");
            }
        }

        private static void testException()
        {
            try
            {
                var t1 = Task.Factory.StartNew(() =>
                {
                    throw new InvalidOperationException("Invalid operation exception") { Source = "t1" };
                });

                var t2 = Task.Factory.StartNew(() =>
                {
                    throw new AccessViolationException("Access violation exception") { Source = "t2" };
                });

                //wait to complete all task.
                Task.WaitAll(new[] { t1, t2 });
            }

            //Handle exception using "AggregateException"
            catch (AggregateException ex)
            {
                //Scenario-1::  Handling all exception

                //foreach (var e in ex.InnerExceptions)
                //{
                //    Console.WriteLine($"Exception {e.GetType()} from {e.Source}");
                //}

                //Scenario-2:: Handle only particular type of exception

                ex.Handle((e) =>
                {
                    if (e is InvalidOperationException)
                    {
                        Console.WriteLine("Handling Invalid operation exception...");
                        return true;
                    }

                    //For access violation exception it propagated and handle from where it is called.
                    return false;
                });
            }
        }

    }
}
