// See https://aka.ms/new-console-template for more information
namespace Object_and_Classes
{
    public class BankAccount
    {
        public decimal Balance { get; private set; }
        public string AccountNumber { get; private set; }
        public string OwnerName { get; private set; }


        public BankAccount(decimal balance, string accountNumber, string ownerName)
        {
            Balance = balance;
            AccountNumber = accountNumber;
            OwnerName = ownerName;
        }

        public void Deposit(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("Amount should be positive");
            }

            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("Amount should be positive");
            }

            if (Balance < amount)
            {
                throw new InvalidOperationException("Insufficient balance");
            }

            Balance -= amount;
        }

        public void DisplayBalance()
        {
            Console.WriteLine(Balance);
        }

    }

    public class Program
    {
        public static void Main()
        {
            BankAccount myAccount = new BankAccount(10000m, "00114424", "Ilia");
            Console.WriteLine($"The bank account number: {myAccount.AccountNumber},owner name: {myAccount.OwnerName}, current balance: {myAccount.Balance}");
            myAccount.Deposit(1000);
            myAccount.Withdraw(500);
            myAccount.DisplayBalance();

        }
    }
}
