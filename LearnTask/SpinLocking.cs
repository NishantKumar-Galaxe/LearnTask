using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearnTask
{
    public class SpinLocking
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

            SpinLock s1 = new SpinLock();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        var lockTaken = false;
                        try
                        {
                            s1.Enter(ref lockTaken);
                            this.Deposit(100);
                        }
                        finally
                        {
                            if (lockTaken) s1.Exit();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        var lockTaken = false;
                        try
                        {
                            s1.Enter(ref lockTaken);
                            this.Withdraw(100);
                        }
                        finally
                        {
                            if (lockTaken) s1.Exit();
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            return this.Balance;
        }
    }
}
