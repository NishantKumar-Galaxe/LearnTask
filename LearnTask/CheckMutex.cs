using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearnTask
{
    public class CheckMutex
    {
        public int _Balance;
        public int Balance
        {
            get { return _Balance; }
            private set { _Balance = value; }
        }

        public object padlock = new object();

        public void Deposit(int amount)
        {
            Balance += amount;
        }

        public void Withdraw(int amount)
        {
            Balance -= amount;
        }

        public int GetBalance()
        {
            var tasks = new List<Task>();

            Mutex mutex = new Mutex();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        var lockTaken = mutex.WaitOne();
                        try
                        {
                            this.Deposit(100);
                        }
                        finally
                        {
                            if (lockTaken) mutex.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        var lockTaken = mutex.WaitOne();
                        try
                        {
                            this.Withdraw(100);
                        }
                        finally
                        {
                            if (lockTaken) mutex.ReleaseMutex();
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            return this.Balance;
        }
    }
}
