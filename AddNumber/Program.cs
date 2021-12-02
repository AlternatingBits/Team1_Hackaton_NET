using System.Net.Http.Headers;
using System.Net.Http.Json;

var client = new HttpClient();
// De base url die voor alle calls hetzelfde is
client.BaseAddress = new Uri("http://involved-htf-challenge.azurewebsites.net");
 
// De token die je gebruikt om je team te authenticeren, deze kan je via de swagger ophalen met je teamname + password
var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiNDQiLCJuYmYiOjE2Mzg0MzcwMzMsImV4cCI6MTYzODUyMzQzMywiaWF0IjoxNjM4NDM3MDMzfQ.unASlknxxtCrp10ad9SWlBJDrRe8uaeKoZeTFCyz03FkPTpaaYMJE6j-s1kFJsVUaPB7XrotGciLYfuv2RyIRg";
// We stellen de token in zodat die wordt meegestuurd bij alle calls, anders krijgen we een 401 Unauthorized response op onze calls
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

var startUrl = "api/path/1/easy/Start";

var startResponse = await client.GetAsync(startUrl);

var sampleUrl = "api/path/1/easy/Sample";

var sampleGetResponse = await client.GetFromJsonAsync<List<int>>(sampleUrl);
Console.WriteLine("sample:");
foreach (var sample in sampleGetResponse)
{
    Console.WriteLine(sample);
}
Console.WriteLine("end");
// Je zoekt het antwoord
var sampleAnswer = GetAnswer(sampleGetResponse);

Console.WriteLine(sampleAnswer);

// We sturen het antwoord met een POST request
// Het antwoord dat we moeten versturen is een getal dus gebruiken we int
// De response die we krijgen zal ons zeggen of ons antwoord juist was
var samplePostResponse = await client.PostAsJsonAsync<int>(sampleUrl, sampleAnswer);
// Om te zien of ons antwoord juist was moeten we de response uitlezen
// Een 200 status code betekent dus niet dat je antwoord juist was!
var samplePostResponseValue = await samplePostResponse.Content.ReadAsStringAsync();
Console.WriteLine(samplePostResponseValue);
// De url om de puzzle challenge data op te halen
//var puzzleUrl = "api/path/1/easy/Puzzle";
// We doen de GET request en wachten op de het antwoord
// De response die we verwachten is een lijst van getallen dus gebruiken we List<int>
//var puzzleGetResponse = await client.GetFromJsonAsync<List<int>>(puzzleUrl);

// Je zoekt het antwoord
//var puzzleAnswer = GetAnswer(puzzleGetResponse);
//Console.WriteLine(puzzleAnswer);
 
// We sturen het antwoord met een POST request
// Het antwoord dat we moeten versturen is een getal dus gebruiken we int
// De response die we krijgen zal ons zeggen of ons antwoord juist was
//var puzzlePostResponse = await client.PostAsJsonAsync<int>(puzzleUrl, puzzleAnswer);
// Om te zien of ons antwoord juist was moeten we de response uitlezen
// Een 200 status code betekent dus niet dat je antwoord juist was!
//var puzzlePostResponseValue = await puzzlePostResponse.Content.ReadAsStringAsync();
//Console.WriteLine(puzzlePostResponseValue);

int GetAnswer(List<int> numbers)
{
    //var firstSum = numbers.Select(number => (long)number).Aggregate((result, next) => result + next);
    //Console.WriteLine($"firstSum: {firstSum}");
    return (int)NumberSum(numbers);
}

long NumberSum(List<int> numbers)
{
    var sum = numbers
        .Sum();
    Console.WriteLine($"sum: {sum}");
    
    var sumNumberList = sum.ToString().Select(character => int.Parse(character.ToString()));
    
    if (sum.ToString().Length > 1)
        return NumberSum(sumNumberList.ToList());
    else
        return sum;
}