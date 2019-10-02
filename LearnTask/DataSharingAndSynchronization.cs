using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace LearnTask
{
    public class DataSharingAndSynchronization
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
            //lock (padlock)
            //{
            //    Balance += amount;
            //}

            Interlocked.Add(ref _Balance, amount);
        }

        public void Withdraw(int amount)
        {
            //lock (padlock)
            //{
            //    Balance -= amount;
            //}

            Interlocked.Add(ref _Balance, -amount);
        }

        public int GetBalance()
        {
            var tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        this.Deposit(100);
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        this.Withdraw(100);
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            return this.Balance;
        }
    }
}
