using OpenDARTCSharp;
using System;

internal class Program
{
    private static void Main()
    {
        string apiKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        var client = new OpenDARTClient(apiKey);

        var corpCodes = client.LoadCorpCodes(true);
        var corpCode = corpCodes[0];
        Console.WriteLine(corpCode);
        Console.WriteLine();

        var company = client.LoadCompany(corpCode);
        Console.WriteLine(company);
        Console.WriteLine();

        client.LoadList();

        return;
    }
}