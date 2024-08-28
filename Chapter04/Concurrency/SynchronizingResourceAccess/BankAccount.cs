namespace SynchronizingResourceAccess;

internal class BankAccount
{
    public decimal Balance => _balance;

    private static readonly Semaphore _semaphore = new(
        initialCount: 0,
        maximumCount: 1);

    private static readonly SemaphoreSlim _semaphoreSlim = new(
        initialCount: 0,
        maxCount: 1);

    private decimal _balance;

    public void Deposit(decimal amount)
    {
        if(!_semaphore.WaitOne(TimeSpan.FromSeconds(15)))
        {
            return;
        }
        
        try
        {
            _balance += amount;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task DepositAsync(decimal amount, CancellationToken cancellationToken)
    {
        if(!await _semaphoreSlim.WaitAsync(TimeSpan.FromSeconds(15), cancellationToken))
        {
            return;
        }

        try
        {
            _balance += amount;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
