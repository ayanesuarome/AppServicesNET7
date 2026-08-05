await foreach(int number in GetNumbersAsync())
{
    WriteLine($"Number: {number}");
}


async static IAsyncEnumerable<int> GetNumbersAsync()
{
    Random ramdom = Random.Shared;
    // simulate work
    await Task.Delay(ramdom.Next(1500, 3000));
    yield return ramdom.Next(0, 9);
    await Task.Delay(ramdom.Next(1500, 3000));
    yield return ramdom.Next(10, 19);
    await Task.Delay(ramdom.Next(1500, 3000));
    yield return ramdom.Next(20, 29);
}