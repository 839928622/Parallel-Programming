using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Task
{
    public class BankAccount
    {
        public object padlock = new object();
        private int balance;

        public int Balance { get => balance; private set => balance = value; }
        public void Deposit(int amount)
        {

            // += 
            // opration 1 : temp <- get_Balance() + amount // read 
            // set_Balance(temp) // write 

            Interlocked.Add(ref balance, amount);

        }

        public void Withdraw(int amount)
        {
            Interlocked.Add( ref balance, -amount);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var ba = new BankAccount();

            var taskList = new List<System.Threading.Tasks.Task>();

            for (int i = 0; i < 10; i++)
            {
                taskList.Add(System.Threading.Tasks.Task.Factory.StartNew
                    (
                        () =>
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                ba.Deposit(100);
                            }
                        }
                    ));

                taskList.Add(System.Threading.Tasks.Task.Factory.StartNew
                   (
                       () =>
                       {
                           for (int j = 0; j < 100; j++)
                           {
                               ba.Withdraw(100);
                           }
                       }
                   ));
                System.Threading.Tasks.Task.WaitAll(taskList.ToArray());
                Console.WriteLine($"Final balance is {ba.Balance }");
            }


            // the balance is different each round,cuz Deposit and Withdraw are not atomic
            Console.ReadKey();

        }
    }
}
